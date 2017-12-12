using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using Pneumail.Models;

namespace Pneumail.Services 
{

    public interface IIncomingEmailService
    {
        Task GetMessages(EmailService service);
    }
    public class IMAPService: IIncomingEmailService
    {
        private static SortedSet<Guid> CurrentlyFetching = new SortedSet<Guid>();
        public IMAPService()
        {

        }

        public async Task GetMessages(EmailService service)
        {
            try {
                if (!IMAPService.CurrentlyFetching.Add(service.Id)) {
                    return;
                }
                var client = new ImapClient();
                await client.ConnectAsync(service.Address, service.Port, SecureSocketOptions.SslOnConnect);
                if (client.IsConnected)
                {
                    await client.AuthenticateAsync(Encoding.UTF8, service.Credentials());
                    if (client.IsAuthenticated)
                    {

                        foreach (var folder in await client.GetFoldersAsync(client.PersonalNamespaces.First()))
                        {
                            var mappedFolder = service.Folders.Where(f => f.Name == folder.Name).First();
                            if (mappedFolder == null)
                            {
                                mappedFolder = new EmailFolder(folder);
                            }
                            if (FolderNeedsUpdate(folder, mappedFolder, service))
                            {
                                foreach (var summary in await folder.FetchAsync(0, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId))
                                {
                                    Console.WriteLine($"{summary.ModSeq}: {summary.Index}\n{summary.UniqueId}: {summary.NormalizedSubject}");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 
            finally 
            {
                IMAPService.CurrentlyFetching.Remove(service.Id);
            }
        }

        private bool FolderNeedsUpdate(IMailFolder server, EmailFolder client, EmailService service)
        {
            var modSeq = false;
            if (server.SupportsModSeq) {
                modSeq = (int) server.HighestModSeq != client.LastModSequence;
            }
            return modSeq ||
                (server.Count != client.Count) ||
                (server.Unread != client.UnreadCount);
        }

        private Message MapMessage(MimeKit.MimeMessage from) {
            try {
                var ret = new Message();
                ret.Date = from.Date.Date;
                ret.Sender = from.From.Select(s => new EmailAddress(s.ToString(), s.Name)).First();
                ret.Subject = from.Subject;
                ret.Recipients = from.To.Select(r => new EmailAddress(r.ToString(),r.Name)).ToList();
                ret.Copied = from.Cc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList();
                ret.BlindCopied = from.Bcc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList();
                ret.Content = from.HtmlBody != null ? from.HtmlBody : from.TextBody;
                    //todo: Add attachments
                return ret;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}