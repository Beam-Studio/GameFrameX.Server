/*using System.Net;
using System.Timers;
using GameFrameX.Launcher;
using GameFrameX.NetWork;
using GameFrameX.NetWork.Messages;
using GameFrameX.Proto.BuiltIn;
using GameFrameX.SuperSocket.ClientEngine;
using GameFrameX.SuperSocket.Server.Abstractions.Session;
using Timer = System.Timers.Timer;

namespace GameFrameX.Hotfix.StartUp;

internal partial class AppStartUpHotfixGame
{
    AsyncTcpSession _gatewayClient;

    private Timer _gateWayReconnectionTimer;
    private Timer _gateWayHeartBeatTimer;
    private ReqHeartBeat _reqGatewayActorHeartBeat;

    private void SendToGatewayMessage(IMessage message)
    {
        if (!_gatewayClient.IsConnected)
        {
            return;
        }

        var span = messageEncoderHandler.Handler(message);
        if (Setting.IsDebug && Setting.IsDebugSend)
        {
            LogHelper.Debug(message.ToSendMessageString(Setting.ServerType, ServerType.Gateway));
        }

        var result = _gatewayClient.TrySend(span);
        if (result)
        {
            _gateWayHeartBeatTimer.Reset();
        }
    }

    protected override void ConnectServerHandler()
    {
        ConnectToGateWay();
    }

    protected override void DisconnectServerHandler()
    {
        DisconnectToGateWay();
    }

    private void StartGatewayClient()
    {
        _gateWayReconnectionTimer = new Timer
        {
            Interval = 5000
        };
        _gateWayReconnectionTimer.Elapsed += GateWayReconnectionTimerOnElapsed;
        _gateWayReconnectionTimer.Start();
        _gateWayHeartBeatTimer = new Timer
        {
            Interval = 5000
        };
        _gateWayHeartBeatTimer.Elapsed += GateWayHeartBeatTimerOnElapsed;
        _gateWayHeartBeatTimer.Start();
        _reqGatewayActorHeartBeat = new ReqHeartBeat();
        _gatewayClient = new AsyncTcpSession();
        _gatewayClient.Closed += GateWayClientOnClosed;
        _gatewayClient.DataReceived += GateWayClientOnDataReceived;
        _gatewayClient.Connected += GateWayClientOnConnected;
        _gatewayClient.Error += GateWayClientOnError;
    }

    private void GateWayHeartBeatTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
        _reqGatewayActorHeartBeat.Timestamp = TimeHelper.UnixTimeSeconds();
        _reqGatewayActorHeartBeat.UpdateUniqueId();
        SendToGatewayMessage(_reqGatewayActorHeartBeat);
    }

    private void GateWayReconnectionTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
        ConnectToGateWay();
    }

    private void GateWayClientOnError(object sender, SuperSocket.ClientEngine.ErrorEventArgs errorEventArgs)
    {
        LogHelper.Info("Gateway server connection error!" + errorEventArgs);
        GateWayClientOnClosed(sender, errorEventArgs);
    }

    private void GateWayClientOnConnected(object sender, EventArgs e)
    {
        // Connected to gateway server, stop reconnection
        _gateWayReconnectionTimer.Stop();
        _gateWayHeartBeatTimer.Start();
        var appSession = sender as IGameAppSession;
        var netChannel = new DefaultNetWorkChannel(appSession, messageEncoderHandler);
        GameClientSessionManager.SetSession(appSession.SessionID, netChannel); // Remove
        LogHelper.Info("Gateway server connection established!");
        ReqRegisterGameServer reqRegisterGameServer = new ReqRegisterGameServer
        {
            ServerType = Setting.ServerType,
            ServerID = Setting.ServerId,
            MinModuleMessageID = Setting.MinModuleId,
            MaxModuleMessageID = Setting.MaxModuleId,
            ServerName = Setting.ServerName
        };
        SendToGatewayMessage(reqRegisterGameServer);
    }

    private async void GateWayClientOnDataReceived(object sender, DataEventArgs dataEventArgs)
    {
        IGameAppSession appSession = (IGameAppSession)sender;
        var messageData = dataEventArgs.Data.ReadBytes(dataEventArgs.Offset, dataEventArgs.Length);
        var message = messageDecoderHandler.Handler(messageData);
        if (message is MessageObject messageObject)
        {
            if (Setting.IsDebug && Setting.IsDebugReceive)
            {
                LogHelper.Info($"Received gateway message: {messageObject.ToReceiveMessageString(ServerType.Gateway, ServerType)}");
            }

            var handler = HotfixManager.GetTcpHandler(message.MessageId);
            if (handler == null)
            {
                LogHelper.Error($"No handler found for [{message.MessageId}][{messageObject.GetType()}]");
                return;
            }

            handler.Message = messageObject;
            handler.NetWorkChannel = GameClientSessionManager.GetSession(appSession.SessionID);
            await handler.Init();
            await handler.InnerAction();
        }
    }

    private void GateWayClientOnClosed(object sender, EventArgs eventArgs)
    {
        LogHelper.Info("Gateway server disconnected! Starting reconnection.");
        // Disconnected from gateway, start reconnection
        _gateWayReconnectionTimer.Start();
        _gateWayHeartBeatTimer.Stop();
    }

    private void ConnectToGateWay()
    {
        if (ConnectTargetServer == null)
        {
            return;
        }

        var endPoint = new IPEndPoint(IPAddress.Parse((string)ConnectTargetServer.TargetIP), ConnectTargetServer.TargetPort);
        _gatewayClient.Connect(endPoint);
    }

    private void DisconnectToGateWay()
    {
        _gatewayClient?.Close();
    }
}*/