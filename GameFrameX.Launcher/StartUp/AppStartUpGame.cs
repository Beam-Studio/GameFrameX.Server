// ==========================================================================================
//  Copyrights, trademarks, patents, and related rights of the GameFrameX organization
//  and its derivative projects are protected by the laws of the People's Republic of China
//  and relevant international regulations.
//
//  Usage of this project must strictly comply with applicable laws, regulations,
//  and open-source licenses.
//
//  This project is dual-licensed under the MIT License and Apache License 2.0.
//  Please refer to the LICENSE file in the root directory for the full license text.
//
//  It is prohibited to use this project for any activities that endanger national security,
//  disrupt social order, or infringe upon the legitimate rights of others.
//  Any legal disputes arising from secondary development based on this project
//  shall be borne solely by the developer; the project organization and contributors
//  assume no responsibility.
//
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.DataBase.Abstractions;
using GameFrameX.Foundation.Utility;

namespace GameFrameX.Launcher.StartUp;

/// <summary>
/// Game server
/// </summary>
[StartUpTag(GlobalConst.GameServiceName)]
internal sealed class AppStartUpGame : AppStartUpBase
{
    public override async Task StartAsync()
    {
        string exitMessage = null;
        try
        {
            LogHelper.Info($"Starting server {Setting.ServerType}");
            var hotfixPath = Directory.GetCurrentDirectory() + "/hotfix";
            if (!Directory.Exists(hotfixPath))
            {
                Directory.CreateDirectory(hotfixPath);
            }

            LogHelper.Debug("Configuring actor limit rules...");
            ActorLimit.Init(ActorLimit.RuleType.None);
            LogHelper.Debug("Actor limit rules configured.");

            LogHelper.Debug("Starting database service...");
            var initResult = await GameDb.Init<MongoDbService>(new DbOptions { ConnectionString = Setting.DataBaseUrl, Name = Setting.DataBaseName, });
            if (initResult == false)
            {
                throw new InvalidOperationException("Failed to start database service");
            }

            LogHelper.DebugConsole("Database service started.");

            LogHelper.DebugConsole("Registering components...");
            await ComponentRegister.Init(typeof(AppsHandler).Assembly);
            LogHelper.DebugConsole("Components registered.");

            LogHelper.DebugConsole("Loading hotfix module...");
            await HotfixManager.LoadHotfixModule(Setting);
            LogHelper.DebugConsole("Hotfix module loaded.");

            LogHelper.DebugConsole("Entering game main loop...");
            GlobalSettings.LaunchTime = TimerHelper.GetUtcNow();
            GlobalSettings.IsAppRunning = true;
            LogHelper.Info($"Server {Setting.ServerType} started.");
            exitMessage = await AppExitToken;
        }
        catch (Exception e)
        {
            LogHelper.Info($"Server execution exception: {e}");
            LogHelper.Fatal(e);
        }

        LogHelper.Info("Shutting down server...");
        await HotfixManager.Stop(exitMessage);
        LogHelper.Info("Server shut down successfully.");
    }

    protected override void Init()
    {
        if (Setting == null)
        {
            Setting = new AppSetting
            {
                ServerId = GlobalConst.GameServiceServerId,
                ServerType = GlobalConst.GameServiceName,
                IsEnableTcp = true,
                InnerPort = 29100,
                MetricsPort = 29090,
                HttpPort = 28080,
                IsEnableHttp = true,
                WsPort = 29110,
                MinModuleId = 10,
                HttpIsDevelopment = true,
                MaxModuleId = 9999,
                DiscoveryCenterHost = "127.0.0.1",
                TagName = "GameFrameX",
                DiscoveryCenterPort = 21001,
                DataBaseUrl = "mongodb+srv://gameframex:f9v42aU9DVeFNfAF@gameframex.8taphic.mongodb.net/?retryWrites=true&w=majority",
                DataBaseName = "gameframex",
            };
        }

        base.Init();
    }
}