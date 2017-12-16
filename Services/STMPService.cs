
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using Pneumail.Models;

namespace Pneumail.Services
{
    public class STMPService
    {
        public void SendMessage(string content,
                                string subject,
                                string name,
                                EmailService service,
                                List<string> to,
                                List<string> cc = null,
                                List<string> bcc = null)
        {
            var msg = new MimeKit.MimeMessage();
            msg.From.Add(new MailboxAddress(name, service.Username.ToString()));
            msg.To.AddRange(to.Select(s => new MailboxAddress(s)));
            if (cc != null)
            {
                msg.Cc.AddRange(cc.Select(s => new MailboxAddress(s)));
            }
            if (bcc != null)
            {
                msg.Bcc.AddRange(bcc.Select(s => new MailboxAddress(s)));
            }
            msg.Body = new TextPart ("plain") {
				Text = content,
            };
            using (var sender = new MailKit.Net.Smtp.SmtpClient())
            {
                sender.Connect(service.OutboundAddress, service.OutboundPort);
                sender.Authenticate(service.Credentials());
                sender.Send(msg);
                sender.Disconnect(true);
            }
        }
    }
}