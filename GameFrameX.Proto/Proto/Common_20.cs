using System;
using ProtoBuf;
using System.Collections.Generic;
using GameFrameX.NetWork.Abstractions;
using GameFrameX.NetWork.Messages;

namespace GameFrameX.Proto.Proto
{
    /// <summary>
    /// Result code
    /// </summary>
    [System.ComponentModel.Description("Result code")]
    public enum ResultCode
    {
        /// <summary>
        /// Success
        /// </summary>
        [System.ComponentModel.Description("Success")]
        Success = 0,

        /// <summary>
        /// Failed
        /// </summary>
        [System.ComponentModel.Description("Failed")]
        Failed = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    [System.ComponentModel.Description("")]
    public enum PhoneType
    {
        /// <summary>
        /// Mobile phone
        /// </summary>
        [System.ComponentModel.Description("Mobile phone")]
        Mobile = 0,

        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("")]
        Home = 1,

        /// <summary>
        /// Work number
        /// </summary>
        [System.ComponentModel.Description("Work number")]
        Work = 2,
    }

    /// <summary>
    /// Operation status code
    /// </summary>
    [System.ComponentModel.Description("Operation status code")]
    public enum OperationStatusCode
    {
        /// <summary>
        /// Success
        /// </summary>
        [System.ComponentModel.Description("Success")]
        Ok = 0,

        /// <summary>
        /// Configuration error
        /// </summary>
        [System.ComponentModel.Description("Configuration error")]
        ConfigErr = 1,

        /// <summary>
        /// Invalid client parameter
        /// </summary>
        [System.ComponentModel.Description("Invalid client parameter")]
        ParamErr = 2,

        /// <summary>
        /// Insufficient resources
        /// </summary>
        [System.ComponentModel.Description("Insufficient resources")]
        CostNotEnough = 3,

        /// <summary>
        /// Service not enabled
        /// </summary>
        [System.ComponentModel.Description("Service not enabled")]
        Forbidden = 4,

        /// <summary>
        /// Not found
        /// </summary>
        [System.ComponentModel.Description("Not found")]
        NotFound = 5,

        /// <summary>
        /// Already exists
        /// </summary>
        [System.ComponentModel.Description("Already exists")]
        HasExist = 6,

        /// <summary>
        /// Account not found or empty
        /// </summary>
        [System.ComponentModel.Description("Account not found or empty")]
        AccountCannotBeNull = 7,

        /// <summary>
        /// Unable to execute database modification
        /// </summary>
        [System.ComponentModel.Description("Unable to execute database modification")]
        Unprocessable = 8,

        /// <summary>
        /// Unknown platform
        /// </summary>
        [System.ComponentModel.Description("Unknown platform")]
        UnknownPlatform = 9,

        /// <summary>
        /// Normal notification
        /// </summary>
        [System.ComponentModel.Description("Normal notification")]
        Notice = 10,

        /// <summary>
        /// Feature not enabled, main message blocked
        /// </summary>
        [System.ComponentModel.Description("Feature not enabled, main message blocked")]
        FuncNotOpen = 11,

        /// <summary>
        /// Other
        /// </summary>
        [System.ComponentModel.Description("Other")]
        Other = 12,

        /// <summary>
        /// Internal server error
        /// </summary>
        [System.ComponentModel.Description("Internal server error")]
        InternalServerError = 13,

        /// <summary>
        /// Notify client that server capacity is full
        /// </summary>
        [System.ComponentModel.Description("Notify client that server capacity is full")]
        ServerFullyLoaded = 14,
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("")]
    public sealed class PhoneNumber
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        public string Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("")]
        public PhoneType Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("")]
    public sealed class Person
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        public string Name { get; set; }

        /// <summary>
        /// Unique ID number for this person.
        /// </summary>
        [ProtoMember(2)]
        [System.ComponentModel.Description("Unique ID number for this person.")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(3)]
        [System.ComponentModel.Description("")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(4)]
        [System.ComponentModel.Description("")]
        public List<PhoneNumber> Phones { get; set; } = new List<PhoneNumber>();
    }

    /// <summary>
    /// Our address book file is just one of these.
    /// </summary>
    [ProtoContract]
    [System.ComponentModel.Description("Our address book file is just one of these.")]
    public sealed class AddressBook
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [System.ComponentModel.Description("")]
        public List<Person> People { get; set; } = new List<Person>();
    }

}
