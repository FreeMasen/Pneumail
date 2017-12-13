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
        Task<List<Message>> GetMessages(EmailService service);
    }
    public class IMAPService: IIncomingEmailService
    {
        private static SortedSet<Guid> CurrentlyFetching = new SortedSet<Guid>();
        public IMAPService()
        {

        }

        private async Task<ImapClient> Connect(EmailService service) 
        {
            var client = new ImapClient();
            await client.ConnectAsync(service.Address, service.Port, SecureSocketOptions.SslOnConnect);
            return client;
        }

        private async Task Authenticate(EmailService service, ImapClient client)
        {
            await client.AuthenticateAsync(Encoding.UTF8, service.Credentials());
        }

        public async Task<List<Message>> GetMessages(EmailService service)
        {
            List<Message> ret = new List<Message>();
            try {
                if (!IMAPService.CurrentlyFetching.Add(service.Id)) {
                    return ret;
                }
                var client = await Connect(service);
                if (client.IsConnected)
                {
                    await Authenticate(service, client);
                    if (client.IsAuthenticated)
                    {

                        foreach (var folder in await client.GetFoldersAsync(client.PersonalNamespaces.First()))
                        {
                            Console.WriteLine($"Reading: {folder}");
                            if (!client.IsConnected) 
                            {
                                client = await Connect(service);
                            }
                            if (!client.IsAuthenticated) 
                            {
                                await Authenticate(service, client);
                            }
                            try {
                                await folder.OpenAsync(FolderAccess.ReadOnly);

                            } catch (Exception ex) {
                                Console.WriteLine($" Error opening {ex.Message}");
                                continue;
                            }
                            EmailFolder mappedFolder = null;
                            if (service.Folders == null) {
                                service.Folders = new List<EmailFolder>();
                            } else {
                                mappedFolder = service.Folders?.Where(f => f.Name == folder.Name)?.FirstOrDefault();
                            }
                            
                            if (FolderNeedsUpdate(folder, mappedFolder, service))
                            {
                                Console.WriteLine($"Folder does need to be updated: {folder.Name}");
                                if (mappedFolder == null) {
                                    mappedFolder = new EmailFolder();
                                    service.Folders.Add(mappedFolder);
                                    
                                }
                                var summeries = await folder.FetchAsync(0, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
                                var needed = summeries.Where(s => (int)s.ModSeq > mappedFolder.LastModSequence).ToList();
                                
                                foreach (var summary in needed)
                                {
                                    if (summary.NormalizedSubject == "ACM Contact Information Update") {
                                        Console.WriteLine("stop");
                                    }
                                    Console.WriteLine($"summary: {summary.NormalizedSubject}");
                                    ret.Add(new Message {
                                        IsComplete = false,
                                        BlindCopied = summary.Envelope.Bcc.Select(s => new EmailAddress(s.ToString(), s.Name)).ToList(),
                                        Copied = summary.Envelope.Cc.Select(s => new EmailAddress(s.ToString(), s.Name)).ToList(),
                                        Sender = summary.Envelope.From.Select(s => new EmailAddress(s.ToString(), s.Name)).FirstOrDefault(),
                                        Date = summary.Date.Date,
                                        Recipients = summary.Envelope.To.Select(s => new EmailAddress(s.ToString(), s.Name)).ToList(),
                                        IsDelayed = false,
                                        Subject = summary.NormalizedSubject,
                                        Content = summary.TextBody.ToString(),
                                    });
                                }

                            } else {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting messages: {ex.Message}");
            } 
            finally 
            {
                IMAPService.CurrentlyFetching.Remove(service.Id);
            }
            return ret;
        }

        private bool FolderNeedsUpdate(IMailFolder server, EmailFolder client, EmailService service)
        {
            if (client == null) return true;
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