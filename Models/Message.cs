using System;
using System.Collections.Generic;

namespace Pneumail.Models
{
    /// <summary>
    /// Represents a email message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The unique ID of this message
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The sender's email address
        /// </summary>
        public EmailAddress Sender { get; set; }
        /// <summary>
        /// List of email address for the To field
        /// </summary>
        public List<EmailAddress> Recipients { get; set; }
        /// <summary>
        /// List of email addresses for the CC field
        /// </summary>
        public List<EmailAddress> Copied { get; set; }
        /// <summary>
        /// List of email addresses for the BCC field
        /// </summary>
        public List<EmailAddress> BlindCopied { get; set; }
        /// <summary>
        /// The Subject of the message
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The content of the message, typicall HTML or plaintext string
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// List of url paths to the attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; }
        /// <summary>
        /// Optional previous message Id used to indicate a reply
        /// </summary>
        public Guid? PreviousId { get; set; }
        /// <summary>
        /// Convience method for check for a previous id
        /// </summary>
        public bool IsReply
        {
            get
            {
                return this.PreviousId.HasValue;
            }
        }
        public Message()
        {

        }
    }
}