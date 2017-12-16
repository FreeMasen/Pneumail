using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using Pneumail.Models;
using Pneumail.Data;

namespace Pneumail.Services 
{
    public interface IIncomingEmailService
    {
        Task GetMessages(string userId);
    }
    public class IMAPService: IIncomingEmailService
    {
        private static SortedSet<Guid> CurrentlyFetching = new SortedSet<Guid>();
        private readonly ApplicationDbContext _data;
        public IMAPService(ApplicationDbContext data)
        {
            this._data = data;
        }

        private async Task<ImapClient> Connect(EmailService service) 
        {
            var client = new ImapClient();
            await client.ConnectAsync(service.InboundAddress,
                                        service.InboundPort,
                                        SecureSocketOptions.SslOnConnect);
            return client;
        }

        public async Task GetMessages(string userId)
        {
            try
            {
                var user = _data.GetUser(userId);
                foreach (var service in user.Services)
                {
                    await GetMessages(service, user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting message with userId {ex.Message}");
            }
        }

        private async Task Authenticate(EmailService service, ImapClient client)
        {
            await client.AuthenticateAsync(Encoding.UTF8, service.Credentials());
        }

        public async Task GetMessages(EmailService service, User user)
        {
            try {
                var incomplete = user.Categories
                                    .Where(c => c.Name.ToLower() == "incomplete")
                                    .FirstOrDefault();
                if (!IMAPService.CurrentlyFetching.Add(service.Id)) {
                    return;
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
                                    Console.WriteLine($"summary: {summary.NormalizedSubject}");
                                    try
                                    {
                                        var msg = MapMessage(summary);
                                        incomplete.Messages.Add(msg);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error parsing msg: {ex.Message}");
                                        continue;

                                    }
                                }
                                Console.WriteLine("Saving folder stats");
                                mappedFolder.LastModSequence = (int)folder.HighestModSeq;
                                mappedFolder.Count = folder.Count;
                                mappedFolder.UnreadCount = folder.Unread;
                                Console.WriteLine("SavingChanges");
                                _data.SaveChanges();
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

        private Message MapMessage(IMessageSummary from) {
            try {
                var ret = new Message();
                ret.SourceId = from.GMailMessageId != null ? (int) from.GMailMessageId : (int)from.UniqueId.Id;
                ret.Date = from.Date.Date;
                ret.Sender = from.Envelope.From.Select(s => new EmailAddress(s.ToString(), s.Name)).First();
                ret.Subject = from.NormalizedSubject;
                ret.Recipients = from.Envelope.To.Select(r => new EmailAddress(r.ToString(),r.Name)).ToList();
                ret.Copied = from.Envelope.Cc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList();
                ret.BlindCopied = from.Envelope.Bcc.Select(r => new EmailAddress(r.ToString(), r.Name)).ToList();
                ret.Content = from.HtmlBody != null ? from.HtmlBody.ToString() : from.TextBody.ToString();
                    //todo: Add attachments
                return ret;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}