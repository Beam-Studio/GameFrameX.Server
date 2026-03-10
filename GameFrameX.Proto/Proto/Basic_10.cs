using System;
using ProtoBuf;
using System.Collections.Generic;
using GameFrameX.NetWork.Abstractions;
using GameFrameX.NetWork.Messages;

namespace GameFrameX.Proto.Proto
{
    /// <summary>
    /// Request heartbeat
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request heartbeat")]
    [MessageTypeHandler(655370)]
    public sealed class ReqHeartBeat : MessageObject, IRequestMessage, IHeartBeatMessage
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Timestamp")]
        public long Timestamp { get; set; }

        public override void Clear()
        {
            Timestamp = default;
        }
    }

    /// <summary>
    /// Server heartbeat result notification; uses notify instead of RPC since some logic needs to process heartbeat results
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Server heartbeat result notification; uses notify instead of RPC since some logic needs to process heartbeat results")]
    [MessageTypeHandler(655371)]
    public sealed class NotifyHeartBeat : MessageObject, INotifyMessage, IHeartBeatMessage
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Timestamp")]
        public long Timestamp { get; set; }

        public override void Clear()
        {
            Timestamp = default;
        }
    }

    /// <summary>
    /// Notify client that server capacity is full
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Notify client that server capacity is full")]
    [MessageTypeHandler(655372)]
    public sealed class NotifyServerFullyLoaded : MessageObject, INotifyMessage
    {

        public override void Clear()
        {
        }
    }

}
