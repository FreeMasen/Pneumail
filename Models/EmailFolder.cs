using System;
using MailKit;

namespace Pneumail.Models
{
    public class EmailFolder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int LastModSequence { get; set; }
        public int Count { get; set; }
        public int UnreadCount { get; set; }
        public uint UidValidity { get; set; }
        public uint? UidNext { get; set; }

        public Guid EmailServiceId { get; set; }
        public EmailService EmailService { get; set; }
        public EmailFolder()
        {

        }

        public EmailFolder(IMailFolder folder)
        {
            this.Name = folder.Name;
            this.LastModSequence = (int)folder.HighestModSeq;
            this.Count = folder.Count;
            this.UidValidity = folder.UidValidity;
            this.UidNext = folder.UidNext?.Id;
        }
    }
}