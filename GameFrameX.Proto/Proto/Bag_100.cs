using System;
using ProtoBuf;
using System.Collections.Generic;
using GameFrameX.NetWork.Abstractions;
using GameFrameX.NetWork.Messages;

namespace GameFrameX.Proto.Proto
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("")]
    public sealed class BagItem
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Item ID")]
        public int ItemId { get; set; }

        /// <summary>
        /// Item count
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Item count")]
        public long Count { get; set; }
    }

    /// <summary>
    /// Request bag data
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request bag data")]
    [MessageTypeHandler(6553610)]
    public sealed class ReqBagInfo : MessageObject, IRequestMessage
    {

        public override void Clear()
        {
        }
    }

    /// <summary>
    /// Response bag data
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Response bag data")]
    [MessageTypeHandler(6553611)]
    public sealed class RespBagInfo : MessageObject, IResponseMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        [ProtoMap(DisableMap = true)]
        public Dictionary<int, long> ItemDic { get; set; } = new Dictionary<int, long>();

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            ItemDic.Clear();
            ErrorCode = default;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("")]
    [MessageTypeHandler(6553612)]
    public sealed class NotifyBagItem : MessageObject, INotifyMessage
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Item ID")]
        public int ItemId { get; set; }

        /// <summary>
        /// Final item count
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Final item count")]
        public long Count { get; set; }

        /// <summary>
        /// Changed value
        /// </summary>
        [ProtoMember(3)]
        [System.ComponentModel.Description("Changed value")]
        public long Value { get; set; }

        public override void Clear()
        {
            ItemId = default;
            Count = default;
            Value = default;
        }
    }

    /// <summary>
    /// Notify bag data changed
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Notify bag data changed")]
    [MessageTypeHandler(6553613)]
    public sealed class NotifyBagInfoChanged : MessageObject, INotifyMessage
    {
        /// <summary>
        /// Changed items, key: item ID, value: count
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Changed items, key: item ID, value: count")]
        [ProtoMap(DisableMap = true)]
        public Dictionary<int, NotifyBagItem> ItemDic { get; set; } = new Dictionary<int, NotifyBagItem>();

        public override void Clear()
        {
            ItemDic.Clear();
        }
    }

    /// <summary>
    /// Request pet composition
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request pet composition")]
    [MessageTypeHandler(6553614)]
    public sealed class ReqComposePet : MessageObject, IRequestMessage
    {
        /// <summary>
        /// Fragment ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Fragment ID")]
        public int FragmentId { get; set; }

        public override void Clear()
        {
            FragmentId = default;
        }
    }

    /// <summary>
    /// Response pet composition
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Response pet composition")]
    [MessageTypeHandler(6553615)]
    public sealed class RespComposePet : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Composed pet ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Composed pet ID")]
        public int PetId { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            PetId = default;
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Request use item
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request use item")]
    [MessageTypeHandler(6553616)]
    public sealed class ReqUseItem : MessageObject, IRequestMessage
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Item ID")]
        public int ItemId { get; set; }

        /// <summary>
        /// Item count
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Item count")]
        public long Count { get; set; }

        public override void Clear()
        {
            ItemId = default;
            Count = default;
        }
    }

    /// <summary>
    /// Response use item
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Response use item")]
    [MessageTypeHandler(6553617)]
    public sealed class RespUseItem : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Item ID")]
        public int ItemId { get; set; }

        /// <summary>
        /// Item count
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Item count")]
        public long Count { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            ItemId = default;
            Count = default;
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Request discard item
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Request discard item")]
    [MessageTypeHandler(6553618)]
    public sealed class ReqDiscardItem : MessageObject, IRequestMessage
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Item ID")]
        public int ItemId { get; set; }

        /// <summary>
        /// Item count
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Item count")]
        public long Count { get; set; }

        public override void Clear()
        {
            ItemId = default;
            Count = default;
        }
    }

    /// <summary>
    /// Response discard item
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Response discard item")]
    [MessageTypeHandler(6553619)]
    public sealed class RespDiscardItem : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Item ID")]
        public int ItemId { get; set; }

        /// <summary>
        /// Item count
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Item count")]
        public long Count { get; set; }

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            ItemId = default;
            Count = default;
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Sell item
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Sell item")]
    [MessageTypeHandler(6553620)]
    public sealed class ReqSellItem : MessageObject, IRequestMessage
    {
        /// <summary>
        /// Item ID
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Item ID")]
        public int ItemId { get; set; }

        public override void Clear()
        {
            ItemId = default;
        }
    }

    /// <summary>
    /// Sell item response
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Sell item response")]
    [MessageTypeHandler(6553621)]
    public sealed class RespItemChange : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Changed items
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Changed items")]
        [ProtoMap(DisableMap = true)]
        public Dictionary<long, long> ItemDic { get; set; } = new Dictionary<long, long>();

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            ItemDic.Clear();
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Add items
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Add items")]
    [MessageTypeHandler(6553622)]
    public sealed class ReqAddItem : MessageObject, IRequestMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        [ProtoMap(DisableMap = true)]
        public Dictionary<int, long> ItemDic { get; set; } = new Dictionary<int, long>();

        public override void Clear()
        {
            ItemDic.Clear();
        }
    }

    /// <summary>
    /// Add items response
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Add items response")]
    [MessageTypeHandler(6553623)]
    public sealed class RespAddItem : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Changed items
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Changed items")]
        [ProtoMap(DisableMap = true)]
        public Dictionary<int, long> ItemDic { get; set; } = new Dictionary<int, long>();

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            ItemDic.Clear();
            ErrorCode = default;
        }
    }

    /// <summary>
    /// Remove items
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Remove items")]
    [MessageTypeHandler(6553624)]
    public sealed class ReqRemoveItem : MessageObject, IRequestMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        [ProtoMap(DisableMap = true)]
        public Dictionary<int, long> ItemDic { get; set; } = new Dictionary<int, long>();

        public override void Clear()
        {
            ItemDic.Clear();
        }
    }

    /// <summary>
    /// Remove items response
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Remove items response")]
    [MessageTypeHandler(6553625)]
    public sealed class RespRemoveItem : MessageObject, IResponseMessage
    {
        /// <summary>
        /// Changed items
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("Changed items")]
        [ProtoMap(DisableMap = true)]
        public Dictionary<int, long> ItemDic { get; set; } = new Dictionary<int, long>();

        /// <summary>
        /// Response error code
        /// </summary>
        [ProtoMember(2047)]
        [System.ComponentModel.Description("Response error code")]
        public int ErrorCode { get; set; }

        public override void Clear()
        {
            ItemDic.Clear();
            ErrorCode = default;
        }
    }

}
