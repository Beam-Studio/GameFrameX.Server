using System;
using ProtoBuf;
using System.Collections.Generic;
using GameFrameX.NetWork.Abstractions;
using GameFrameX.NetWork.Messages;

namespace GameFrameX.Proto.Proto
{
    /// <summary>
    /// Request account login
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request account login")]
    [MessageTypeHandler(19660810)]
    public sealed class ReqLogin : MessageObject, IRequestMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("")]
        public string Platform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(3)]
        [System.ComponentModel.Description("")]
        public int SdkType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(4)]
        [System.ComponentModel.Description("")]
        public string SdkToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(5)]
        [System.ComponentModel.Description("")]
        public string Device { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [ProtoMember(6)]
        [System.ComponentModel.Description("Password")]
        public string Password { get; set; }

        public override void Clear()
        {
            UserName = default;
            Platform = default;
            SdkType = default;
            SdkToken = default;
            Device = default;
            Password = default;
        }
    }

    /// <summary>
    /// Account login response
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Account login response")]
    [MessageTypeHandler(19660811)]
    public sealed class RespLogin : MessageObject, IResponseMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        public int Code { get; set; }

        /// <summary>
        /// Account name
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Account name")]
        public string RoleName { get; set; }

        /// <summary>
        /// Account ID
        /// </summary>
        [ProtoMember(3)]
        [System.ComponentModel.Description("Account ID")]
        public long Id { get; set; }

        /// <summary>
        /// Account level
        /// </summary>
        [ProtoMember(4)]
        [System.ComponentModel.Description("Account level")]
        public uint Level { get; set; }

        /// <summary>
        /// Creation time
        /// </summary>
        [ProtoMember(5)]
        [System.ComponentModel.Description("Creation time")]
        public long CreateTime { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            Code = default;
            RoleName = default;
            Id = default;
            Level = default;
            CreateTime = default;
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Request player creation
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request player creation")]
    [MessageTypeHandler(19660812)]
    public sealed class ReqPlayerCreate : MessageObject, IRequestMessage
    {
        /// <summary>
        /// Account ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Account ID")]
        public long Id { get; set; }

        /// <summary>
        /// Player name
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Player name")]
        public string Name { get; set; }

        public override void Clear()
        {
            Id = default;
            Name = default;
        }
    }

    /// <summary>
    /// Player creation response
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Player creation response")]
    [MessageTypeHandler(19660813)]
    public sealed class RespPlayerCreate : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Player info
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Player info")]
        public PlayerInfo PlayerInfo { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            PlayerInfo = default;
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Request player list
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request player list")]
    [MessageTypeHandler(19660814)]
    public sealed class ReqPlayerList : MessageObject, IRequestMessage
    {
        /// <summary>
        /// Account ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Account ID")]
        public long Id { get; set; }

        public override void Clear()
        {
            Id = default;
        }
    }

    /// <summary>
    /// Player list response
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Player list response")]
    [MessageTypeHandler(19660815)]
    public sealed class RespPlayerList : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Player list
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Player list")]
        public List<PlayerInfo> PlayerList { get; set; } = new List<PlayerInfo>();

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            PlayerList.Clear();
            ErrorCode = default;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("")]
    public sealed class PlayerInfo
    {
        /// <summary>
        /// Player ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Player ID")]
        public long Id { get; set; }

        /// <summary>
        /// Player name
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Player name")]
        public string Name { get; set; }

        /// <summary>
        /// Player level
        /// </summary>
        [ProtoMember(3)]
        [System.ComponentModel.Description("Player level")]
        public uint Level { get; set; }

        /// <summary>
        /// Player state
        /// </summary>
        [ProtoMember(4)]
        [System.ComponentModel.Description("Player state")]
        public int State { get; set; }

        /// <summary>
        /// Player avatar
        /// </summary>
        [ProtoMember(5)]
        [System.ComponentModel.Description("Player avatar")]
        public uint Avatar { get; set; }

        /// <summary>
        /// Player current experience
        /// </summary>
        [ProtoMember(6)]
        [System.ComponentModel.Description("Player current experience")]
        public ulong CurrentExp { get; set; }
    }

    /// <summary>
    /// Request player login
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request player login")]
    [MessageTypeHandler(19660816)]
    public sealed class ReqPlayerLogin : MessageObject, IRequestMessage
    {
        /// <summary>
        /// Player ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Player ID")]
        public long Id { get; set; }

        public override void Clear()
        {
            Id = default;
        }
    }

    /// <summary>
    /// Player login response
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Player login response")]
    [MessageTypeHandler(19660817)]
    public sealed class RespPlayerLogin : MessageObject, IResponseMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        public int Code { get; set; }

        /// <summary>
        /// Creation time
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Creation time")]
        public long CreateTime { get; set; }

        /// <summary>
        /// Player info
        /// </summary>
        [ProtoMember(3)]
        [System.ComponentModel.Description("Player info")]
        public PlayerInfo PlayerInfo { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            Code = default;
            CreateTime = default;
            PlayerInfo = default;
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Error code response returned for every client request
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Error code response returned for every client request")]
    [MessageTypeHandler(19660818)]
    public sealed class RespErrorCode : MessageObject, IResponseMessage
    {
        /// <summary>
        /// 0: no error
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("0: no error")]
        public long ErrCode { get; set; }

        /// <summary>
        /// Error description (valid when non-zero)
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Error description (valid when non-zero)")]
        public string Desc { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            ErrCode = default;
            Desc = default;
            ErrorCode = default;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("")]
    [MessageTypeHandler(19660819)]
    public sealed class RespPrompt : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Prompt type (1: tip, 2: marquee, 3: priority marquee, 4: popup, 5: popup return to login, 6: popup exit game)
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Prompt type (1: tip, 2: marquee, 3: priority marquee, 4: popup, 5: popup return to login, 6: popup exit game)")]
        public int Type { get; set; }

        /// <summary>
        /// Prompt content
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Prompt content")]
        public string Content { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            Type = default;
            Content = default;
            ErrorCode = default;
        }
    }

}
