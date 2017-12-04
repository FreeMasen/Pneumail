using System;
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
        public string Address { get; set; }
        /// <summary>
        /// The user's external username for this service
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The user's external password for this service
        /// </summary>
        public string Password { get; set; }
    }
}