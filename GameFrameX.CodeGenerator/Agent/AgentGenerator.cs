using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameFrameX.CodeGenerator.Utils;
using GameFrameX.Core.Abstractions.Attribute;
using GameFrameX.Utility.Setting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameFrameX.CodeGenerator.Agent;

[Generator]
public class AgentGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        ResLoader.LoadDll();
        context.RegisterForSyntaxNotifications(() => new AgentFilter());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is AgentFilter receiver)
        {
            receiver.ClearPartialList();
            var serviceAttributeName = nameof(ServiceAttribute).Replace("Attribute", string.Empty);
            var threadSafeAttributeName = nameof(ThreadSafeAttribute).Replace("Attribute", string.Empty);
            var discardAttributeName = nameof(DiscardAttribute).Replace("Attribute", string.Empty);
            var timeOutAttributeName = nameof(TimeOutAttribute).Replace("Attribute", string.Empty);

            var partialClassCount = new Dictionary<string, int>();

            foreach (var agent in receiver.AgentList)
            {
                var fullName = agent.GetFullName();
                var info = new AgentInfo();
                info.Super = agent.Identifier.Text;
                info.Name = info.Super + GlobalConst.WrapperNameSuffix;
                //info.Space = Tools.GetNameSpace(fullName);
                info.Space = GlobalConst.HotfixNameSpaceNamePrefix + GlobalConst.WrapperNameSuffix + ".Agent";

                string outFileName = null;

                var isPartialClass = agent.Modifiers.ToList().FindIndex(s => s.Text == "partial") >= 0;
                if (isPartialClass)
                {
                    info.Partial = "partial";
                    partialClassCount.TryGetValue(info.Name, out var count);
                    partialClassCount[info.Name] = count + 1;
                    outFileName = $"{info.Name}{count}.g.cs";
                }
                else
                {
                    outFileName = $"{info.Name}.g.cs";
                }

                // Process usings
                var root = agent.SyntaxTree.GetCompilationUnitRoot();
                foreach (var element in root.Usings)
                {
                    info.UsingSpaces.Add(element.Name.ToString());
                }

                info.UsingSpaces.Add(Tools.GetNameSpace(fullName));

                foreach (var member in agent.Members)
                {
                    if (member is MethodDeclarationSyntax method)
                    {
                        if (method.Identifier.Text.Equals("Active")
                            || method.Identifier.Text.Equals("Inactive"))
                        {
                            continue;
                        }

                        var mth = new MthInfo();
                        // Modifiers
                        foreach (var m in method.Modifiers)
                        {
                            if (m.Text.Equals("virtual"))
                            {
                                mth.IsVirtual = true;
                                mth.Modify += "override ";
                            }
                            else
                            {
                                mth.Modify += m.Text + " ";
                            }

                            if (m.Text.Equals("public"))
                            {
                                mth.IsPublic = true;
                            }

                            if (m.Text.Equals("static"))
                            {
                                mth.IsStatic = true;
                            }

                            if (m.Text.Equals("async"))
                            {
                                mth.IsAsync = true;
                            }
                        }

                        if (mth.IsStatic)
                        {
                            continue;
                        }

                        mth.ReturnType = method.ReturnType?.ToString() ?? "void"; //Task<T>
                        // Iterate attributes
                        foreach (var attributeListSyntax in method.AttributeLists)
                        {
                            var attrName = attributeListSyntax.ToString().RemoveWhitespace() + "Attribute";
                            // if (attStr.Contains("[Api]") || attStr.Contains("[Service]"))
                            if (attrName.IndexOf(serviceAttributeName, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                mth.IsApi = true;
                            }
                            else if (attrName.IndexOf(discardAttributeName, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                mth.Discard = true;
                                if (mth.IsAsync)
                                {
                                    mth.Modify = mth.Modify.Replace("async ", "");
                                    mth.IsAsync = false;
                                }
                            }
                            else if (attrName.IndexOf(timeOutAttributeName, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                mth.HasTimeout = true;
                                var argStr = attributeListSyntax.Attributes[0].ArgumentList.Arguments[0].ToString();
                                if (argStr.IndexOf(":", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    mth.TimeOut = int.Parse(argStr.Split(':')[1].Trim());
                                }
                                else
                                {
                                    mth.TimeOut = int.Parse(argStr);
                                }
                            }
                            else if (attrName.IndexOf(threadSafeAttributeName, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                mth.IsThreadSafe = true;
                            }
                        }

                        if (mth.IsThreadSafe && mth.HasTimeout)
                        {
                            context.LogError($"{fullName}.{method.Identifier.Text} cannot specify timeout for methods marked with [{threadSafeAttributeName}]");
                        }

                        if (!mth.IsApi && !mth.Discard && mth.HasTimeout)
                        {
                            context.LogError($"{fullName}.{method.Identifier.Text} [{timeOutAttributeName}] attribute can only be used with [Api] or [{discardAttributeName}]");
                        }

                        // Skip methods without any attributes
                        if (!mth.IsApi && !mth.Discard && !mth.IsThreadSafe)
                        {
                            continue;
                        }

                        // Skip if thread-safe but not discarded
                        if (mth.IsThreadSafe && !mth.Discard)
                        {
                            continue;
                        }

                        if (mth.IsApi && !mth.IsThreadSafe && !mth.ReturnType.Contains("Task"))
                        {
                            context.LogError($"{fullName}.{method.Identifier.Text}, non-[{threadSafeAttributeName}] [Api] methods must be async");
                        }

                        if ((mth.IsApi || mth.Discard || mth.IsThreadSafe) && !mth.IsVirtual)
                        {
                            context.LogError($"{fullName}.{method.Identifier.Text} methods marked with [AsyncApi], [{threadSafeAttributeName}], or [{discardAttributeName}] must be declared as virtual");
                        }

                        if (mth.IsVirtual)
                        {
                            info.Methods.Add(mth);
                            mth.Name = method.Identifier.Text;
                            mth.ParamDeclare = method.ParameterList.ToString(); //(int a, List<int> list)
                            // mth.ReturnType = method.ReturnType.ToString();   //Task<T>
                            if (mth.Discard && !mth.ReturnType.Equals(nameof(Task)) && !mth.ReturnType.Equals(nameof(ValueTask)))
                            {
                                context.LogError($"{fullName}.{method.Identifier.Text} [Discard] attribute can only be applied to methods returning Task or ValueTask");
                            }

                            mth.Constraint = method.ConstraintClauses.ToString(); //where T : class, new() where K : BagState
                            mth.Typeparams = method.TypeParameterList?.ToString(); //"<T, K>"	
                            foreach (var p in method.ParameterList.Parameters)
                            {
                                mth.Params.Add(p.Identifier.Text);
                            }
                        }
                    }
                }

                var source = AgentTemplate.Run(info);
                context.AddSource(outFileName, source);
            }
        }
    }
}