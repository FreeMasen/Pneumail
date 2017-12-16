using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

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
        public int SourceId { get; set; }
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

        public DateTime Date { get; set; }
        public bool IsComplete { get; set; }
        public bool IsDelayed { get; set; }
        public DateTime? Redelivery { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Message()
        {

        }

        public string ToJson()
        {
            var jb = new StringBuilder();
            jb.Append("{");
            jb.Append("\"id\":\"");
            jb.Append(Id.ToString());
            jb.Append("\",");
            jb.Append("\"sourceId\":");
            jb.Append(SourceId);
            jb.Append(",\"sender\":\"");
            jb.Append(Sender.ToString().Replace("\"", ""));
            jb.Append("\",\"recipients\":[");

            for (var i = 0; i < Recipients.Count();i++)
            {
                var r = Recipients[i];
                jb.Append("\"");
                jb.Append(r.ToString().Replace("\"", ""));
                jb.Append("\"");
                if (i < Recipients.Count() - 1)
                    jb.Append(",");
            }
            jb.Append("],\"copied\":[],\"blindCopied\":[],\"subject\":\"");
            jb.Append(Subject.Replace("\"", ""));
            jb.Append("\",\"content\":\"");
            jb.Append(Content.Replace("\n", "\\n").Replace("\"", ""));
            jb.Append("\",\"isReply\":false,\"date\":\"");
            jb.Append(Date.ToString());
            jb.Append("\",\"isComplete\":false,\"isDelayed\":false,\"categoryId\":\"");
            jb.Append(CategoryId.ToString());
            jb.Append("\"}");
            return jb.ToString();
        }
    }
}