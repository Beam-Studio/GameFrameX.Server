using System.Collections.Generic;
using System.Text;

namespace GameFrameX.CodeGenerator.Agent;

public class MthInfo
{
    /// <summary>
    ///     Method name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Return type
    /// </summary>
    public string ReturnType { get; set; }


    /// <summary>
    /// Method signature
    /// </summary>
    public string Declare
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append(Modify);
            sb.Append(ReturnType);
            sb.Append(" ");
            sb.Append(Name);
            sb.Append(Typeparams);
            sb.Append(ParamDeclare);
            //sb.Append(" ");
            //sb.Append(Constraint);
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Whether it is an API
    /// </summary>
    public bool IsApi { get; set; }

    /// <summary>
    ///     Modifier string
    /// </summary>
    public string Modify { get; set; }

    /// <summary>
    ///     Whether it is public
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    ///     Whether it is static
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    ///     Whether it is virtual
    /// </summary>
    public bool IsVirtual { get; set; }

    /// <summary>
    ///     Whether it is async
    /// </summary>
    public bool IsAsync { get; set; }

    public List<string> Params { get; } = new();

    /// <summary>
    /// Attribute list
    /// </summary>
    public List<string> AttributeList { get; private set; } = new();

    public bool Discard { get; set; }

    /// <summary>
    ///     Whether it has a timeout
    /// </summary>
    public bool HasTimeout { get; set; }

    /// <summary>
    ///     Timeout duration
    /// </summary>
    public int TimeOut { get; set; } = int.MaxValue;

    /// <summary>
    ///     Whether it is thread-safe
    /// </summary>
    public bool IsThreadSafe { get; set; }

    /// <summary>
    ///     Constraints
    /// </summary>
    public string Constraint { get; set; }

    /// <summary>
    ///     Type parameters
    /// </summary>
    public string Typeparams { get; set; }

    /// <summary>
    ///     Parameter declaration
    /// </summary>
    public string ParamDeclare { get; set; }

    /// <summary>
    ///     Parameter string
    /// </summary>
    public string ParamString
    {
        get
        {
            if (Params.Count > 0)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < Params.Count; i++)
                {
                    sb.Append(Params[i]);
                    if (i != Params.Count - 1)
                    {
                        sb.Append(",");
                    }
                }

                return sb.ToString();
            }

            return "";
        }
    }
}

public class AgentInfo
{
    /// <summary>
    ///     Namespace
    /// </summary>
    public string Space { get; set; }

    /// <summary>
    /// Partial class
    /// </summary>
    public string Partial { get; set; } = "";

    /// <summary>
    ///     Class name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Base class
    /// </summary>
    public string Super { get; set; }

    /// <summary>
    ///     Method list
    /// </summary>
    public List<MthInfo> Methods { get; set; } = new();

    /// <summary>
    ///     Referenced namespaces
    /// </summary>
    public List<string> UsingSpaces { get; set; } = new();
}