using System;
using System.Collections.Generic;
using System.Net;

namespace Pneumail.Models
{
    /// <summary>
    /// An Email Service Provider that the
    /// user has decided to have our client
    /// handle messages to/from
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// The unique id for this service
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The URL for the provider's STMP server
        /// </summary>
        public string InboundAddress { get; set; }
        public string OutboundAddress { get; set; }
        /// <summary>
        /// The user's external username for this service
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The user's external password for this service
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// The port to connect to the service provider
        /// </summary>
        public int InboundPort { get; set; }
        public int OutboundPort { get; set; }
        /// <summary>
        /// The service's source folder information
        /// </summary>
        public List<EmailFolder> Folders { get; set; }
        /// <summary>
        /// Parent object, user
        /// </summary>
        public string UserId { get; set; }
        public NetworkCredential Credentials() {
            return new NetworkCredential(Username, Password);
        }

        public string ToJson()
        {
            return $@"{{
                    ""id"": ""{Id}"",
                    ""InboundAddress: ""{InboundAddress}"",
                    ""OutboundAddress"": ""{OutboundAddress}"",
                    ""Username"": ""{Username}"",
                    ""Password"": ""{Password}"",
                    ""InboundPort"": {InboundPort},
                    ""OutboundPort"": {OutboundPort}
                }}";
        }
    }
}