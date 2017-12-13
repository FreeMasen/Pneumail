using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pneumail.Data;
using Pneumail.Models;
using Pneumail.Services;

namespace Pneumail.Controllers
{
    /// <summary>
    /// The controller that will be used to maintin and interact with the WebSocket
    /// connection to the browser
    /// </summary>
    public class WebSocketController : Controller
    {
        private WebSocket WebSocket;
        private ApplicationDbContext _data;
        private UserManager<User> _userManager;
        private IMAPService _mailService;
        public WebSocketController(ApplicationDbContext data,
                                    UserManager<User> userManager,
                                    IIncomingEmailService _mailService)
        {
            this._data = data;
            this._userManager = userManager;
            this._mailService = (IMAPService)_mailService;
        }

        /// <summary>
        /// The socket route
        /// </summary>
        public async Task Sock()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                WebSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Accepted Websocket");
                var userId = _userManager.GetUserId(User);
                Console.WriteLine($"Got UserId: {userId}");
                var user = await _data.Users.Where(u => u.Id == userId)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Recipients)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.BlindCopied)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Copied)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Sender)
                                            .Include(u => u.Categories)
                                                .ThenInclude(c => c.Messages)
                                                    .ThenInclude(m => m.Attachments)
                                            .Include(u => u.Rules)
                                            .Include(u => u.Services)
                                            .FirstOrDefaultAsync();
                GetMessages(user.Services);
                try {
                    var rawBuf = new byte[1024 * 4];
                    var buffer = new ArraySegment<byte>(rawBuf);
                    var update = new UpdateMessage {
                                        Categories = user.Categories,
                                        UpdateType = UpdateType.Initial,
                                        Rules = user.Rules,
                                        Services = user.Services
                                        };
                    await SendUpdate(update);
                    var result = await WebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    Console.WriteLine($"Recieved initial message");
                    while (!result.CloseStatus.HasValue)
                    {
                        if (result.Count > 0)
                        {
                            var msgText = System.Text.Encoding.ASCII.GetString(buffer.Array.Take(result.Count).ToArray());
                            Console.WriteLine($"Websocket Message\n----------\n{msgText}");
                            await RespondToUpdate(msgText);

                        }
                        result = await WebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    }
                    await WebSocket.CloseAsync(result.CloseStatus.Value,
                                                result.CloseStatusDescription, CancellationToken.None);
                } catch (Exception e)
                {
                    Console.WriteLine($"error in websocket: {e.Message}");
                }
            }
        }

        public async void GetMessages(List<EmailService> services) {
            var id = _userManager.GetUserId(User);
            var user = _data.CompleteUserQuery(id).First();
            var incomplete = user.Categories.Where(c => c.Name.ToLower() == "incomplete").FirstOrDefault();

            foreach (var service in services) 
            {
                var messages = await _mailService.GetMessages(service);
                incomplete.Messages.AddRange(messages);
            }
            await _data.SaveChangesAsync();
        }

        public async Task RespondToUpdate(string originalMsg)
        {
            var firstComma = originalMsg.IndexOf(',');
            var UpdateTypeStr = originalMsg.Substring(14, firstComma - 14).Replace("\"", "");
            var userId = _userManager.GetUserId(User);
            var userQuery = _data.Users.Where(u => u.Id == userId).AsQueryable();
            User user;
            UpdateMessage fromServer;
            switch (UpdateTypeStr) {
                case ClientUpdateType.UpdateService:
                    ServiceUpdate update = JsonConvert.DeserializeObject<ServiceUpdate>(originalMsg);
                    user = await userQuery
                                    .Include(u => u.Services)
                                    .FirstAsync();
                    if (update.Service.Id != Guid.Empty) {
                        if (update.Delete) {
                            _data.EmailServices.Remove(update.Service);
                        } else {
                            _data.Update(update.Service);
                        }

                    } else {
                        update.Service.Folders = new List<EmailFolder>();
                        user.Services.Add(update.Service);
                    }
                    await _data.SaveChangesAsync();
                    fromServer = new UpdateMessage() {
                        UpdateType = UpdateType.ServiceUpdateConfirmation,
                        Services = user.Services
                    };
                    await SendUpdate(fromServer);
                    await _mailService.GetMessages(update.Service);
                break;
                case ClientUpdateType.UpdateRule:
                    var ruleUpdate = JsonConvert.DeserializeObject<RuleUpdate>(originalMsg);
                    if (ruleUpdate.Rule.Id != Guid.Empty) {
                        _data.Update(ruleUpdate.Rule);
                    } else {
                        _data.Add(ruleUpdate.Rule);
                    }
                    await _data.SaveChangesAsync();
                    user = await userQuery.Include(u => u.Rules)
                                                .FirstAsync();
                    var rulesReply = new UpdateMessage() {
                        UpdateType = UpdateType.RuleUpdateConfirmation,
                        Rules = user.Rules,
                    };
                    await SendUpdate(rulesReply);
                break;
                case ClientUpdateType.MarkMessageComplete:
                    var completeUpdate = JsonConvert.DeserializeObject<MessageCompleteUpdate>(originalMsg);
                    var msg = await _data.Messages.Where(m => m.Id == completeUpdate.Id).FirstAsync();
                    msg.IsComplete = !msg.IsComplete;
                    _data.Update(msg);
                    await _data.SaveChangesAsync();
                    var updatedCat = await _data.Categories
                                            .Where(c => c.Id == msg.CategoryId)
                                            .FirstAsync();
                    fromServer = new UpdateMessage() {
                        Categories = new List<Category>() {
                            updatedCat
                        },
                        UpdateType = UpdateType.Modify,
                    };
                    await SendUpdate(fromServer);
                break;
                case ClientUpdateType.MarkMessageForLater:
                    var delayUpdate = JsonConvert.DeserializeObject<MessageDelayUpdate>(originalMsg);
                    var msgToDelay = await _data.Messages.Where(m => m.Id == delayUpdate.Id)
                                        .FirstAsync();
                    msgToDelay.IsDelayed = true;
                    msgToDelay.Redelivery = delayUpdate.Time;
                    _data.Update(msgToDelay);
                    await _data.SaveChangesAsync();
                    var delayedCat = await _data.Categories
                                                .Where(c => c.Id == msgToDelay.CategoryId)
                                                .FirstAsync();
                    fromServer = new UpdateMessage() {
                        Categories = new List<Category>() {
                            delayedCat,
                        },
                        UpdateType = UpdateType.Modify
                    };
                    await SendUpdate(fromServer);
                break;
                case ClientUpdateType.MoveMessageToCategory:
                    var moveUpdate = JsonConvert.DeserializeObject<MoveMessageUpdate>(originalMsg);
                    var msgToMove = await _data.Messages.Where(m => m.Id == moveUpdate.Id)
                                    .FirstAsync();
                    var oldCatId = msgToMove.CategoryId;
                    msgToMove.CategoryId = moveUpdate.NewCategory;
                    _data.Update(msgToMove);
                    await _data.SaveChangesAsync();
                    var cats = await _data.Categories.Where(c =>
                            c.Id == oldCatId || c.Id == moveUpdate.NewCategory
                        ).ToListAsync();
                    fromServer = new UpdateMessage() {
                        Categories = cats,
                        UpdateType = UpdateType.Modify,
                    };
                    await SendUpdate(fromServer);
                break;
                case ClientUpdateType.SendNewMessage:

                break;
            }
        }

        public async Task SendUpdate(UpdateMessage updates) {
            try {

                var mappedCategories = updates.Categories != null ? updates.Categories.Select(c =>new {
                    Id = c.Id,
                    Name = c.Name,
                    Messages = c.Messages.Select(m => new {
                        Id = m.Id,
                        CatId = c.Id,
                        Sender = m.Sender.ToString(),
                        Recipients = m.Recipients.Select(r => r.ToString()).ToArray(),
                        Copied = m.Copied.Select(r => r.ToString()).ToArray(),
                        BlindCopied = m.BlindCopied.Select(b => b.ToString()).ToArray(),
                        Subject = m.Subject,
                        Content = m.Content,
                        Attachments = m.Attachments.Select(a => new {Id = a.Id, name = a.Name, Path = a.Path, MsgId = m.Id}),
                    })
                }) : null;
                var mapped = JsonConvert.SerializeObject(
                                                new {
                                                    UpdateType = updates.UpdateType,
                                                    Categories = mappedCategories,
                                                    Rules = updates.Rules,
                                                    Services = updates.Services
                                                    },
                                                Formatting.None,
                                                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                var msg = System.Text.Encoding.ASCII.GetBytes(mapped);

                var bytes = new ArraySegment<byte>(msg, 0, msg.Count());

                await WebSocket.SendAsync(bytes,
                                            WebSocketMessageType.Text,
                                            true, CancellationToken.None);
                Console.WriteLine("Sent");
            }
            catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private IQueryable<User> CompleteUserQuery(IQueryable<User> userQuery)
        {
            return userQuery
                    .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Recipients)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.BlindCopied)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Copied)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Sender)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Attachments)
                        .AsQueryable();
        }
    }
}