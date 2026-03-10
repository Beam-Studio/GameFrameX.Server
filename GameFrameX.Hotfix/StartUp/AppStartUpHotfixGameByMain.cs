// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//  
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//  
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//  
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//  
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Apps.Common.Session;
using GameFrameX.SuperSocket.Connection;
using GameFrameX.SuperSocket.Server.Abstractions.Session;
using GameFrameX.SuperSocket.WebSocket.Server;

namespace GameFrameX.Hotfix.StartUp;

/// <summary>
/// Game server. Starts last.
/// </summary>
internal partial class AppStartUpHotfixGame
{
    public override async Task StartAsync()
    {
        // Start network service
        // Set compression and decompression
        await StartServerAsync<DefaultMessageDecoderHandler, DefaultMessageEncoderHandler>(new DefaultMessageCompressHandler(), new DefaultMessageDecompressHandler(), HotfixManager.GetListHttpHandler(), HotfixManager.GetHttpHandler);
        // Start HTTP service
        // await HttpServer.Start(Setting.HttpPort, Setting.HttpsPort, HotfixManager.GetListHttpHandler(), HotfixManager.GetHttpHandler, null, Setting.HttpUrl);
    }

    public async Task RunServer(bool reload = false)
    {
        // Always load config regardless of restart
        await ConfigComponent.Instance.LoadConfig();
        if (reload)
        {
            ActorManager.ClearAgent();
            return;
        }

        await StartAsync();
    }


    protected override ValueTask OnDisconnected(IAppSession appSession, CloseEventArgs disconnectEventArgs)
    {
        LogHelper.Info("External client disconnected. Session: " + appSession.SessionID + "  " + disconnectEventArgs.Reason);
        SessionManager.Remove(appSession.SessionID);
        return ValueTask.CompletedTask;
    }

    protected override async ValueTask OnConnected(IAppSession appSession)
    {
        LogHelper.Info("External client connected. SessionID:" + appSession.SessionID + " RemoteEndPoint:" + appSession.RemoteEndPoint);
        var netChannel = new DefaultNetWorkChannel(appSession, Setting, null, appSession is WebSocketSession);
        var count = SessionManager.Count();
        if (count > Setting.MaxClientCount)
        {
            // Max online player limit reached
            await netChannel.WriteAsync(new NotifyServerFullyLoaded(), (int)OperationStatusCode.ServerFullyLoaded);
            netChannel.Close();
            return;
        }

        var session = new Session(appSession.SessionID, netChannel);
        SessionManager.Add(session);
    }

    /// <summary>
    /// Handle received message
    /// </summary>
    /// <param name="appSession"></param>
    /// <param name="message"></param>
    protected override async ValueTask PackageHandler(IAppSession appSession, IMessage message)
    {
        if (message is NetworkMessagePackage messagePackage)
        {
            var netWorkChannel = SessionManager.GetChannel(appSession.SessionID);

            if (netWorkChannel.IsNull())
            {
                return;
            }

            var actorId = netWorkChannel.GetData<long>(GlobalConst.ActorIdKey);
            if (messagePackage.Header.OperationType == (byte)MessageOperationType.HeartBeat)
            {
                if (Setting.IsDebug && Setting.IsDebugReceive && Setting.IsDebugReceiveHeartBeat)
                {
                    LogHelper.Debug($"---Received {messagePackage.ToFormatMessageString(actorId)}");
                }

                // Heartbeat message reply
                ReplyHeartBeat(netWorkChannel, (MessageObject)messagePackage.DeserializeMessageObject());
                return;
            }

            if (Setting.IsDebug && Setting.IsDebugReceive)
            {
                LogHelper.Debug($"---Received {messagePackage.ToFormatMessageString(actorId)}");
            }

            var handler = HotfixManager.GetTcpHandler(messagePackage.Header.MessageId);
            if (handler == null)
            {
                LogHelper.Error($"No handler found for [{messagePackage.Header.MessageId}][{messagePackage.MessageType}]");
                return;
            }

            // Execute message dispatch
            try
            {
                await InvokeMessageHandler(handler, messagePackage.DeserializeMessageObject(), netWorkChannel);
            }
            catch (Exception exception)
            {
                LogHelper.Fatal(exception);
            }
        }
    }

    public override async Task StopAsync(string message = "")
    {
        await base.StopAsync(message);
        // Disconnect all connections
        await SessionManager.RemoveAll();
        // Cancel all pending timers
        await QuartzTimer.Stop();
        // Ensure all actor tasks complete
        await ActorManager.AllFinish();
        // Save all data
        await GlobalTimer.Stop();
        // Remove all actors
        await ActorManager.RemoveAll();
    }
}