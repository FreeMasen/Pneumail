using System;
using System.Collections.Generic;

namespace Pneumail.Models
{
    public class UpdateMessage
    {
        public UpdateType UpdateType { get; set; }
        public List<Category> Categories = new List<Category>();
        public List<Rule> Rules;
        public List<EmailService> Services;
    }

    [Flags]
    public enum UpdateType
    {
        None = 0,
        Initial = 1,
        Insert = 2,
        Delete = 4,
        Modify = 8,
        ServiceUpdateConfirmation = 16,
        RuleUpdateConfirmation = 32,
    }

    public abstract class UpdateFromClient
    {
        public string UpdateType { get; set; }
    }

    public class ServiceUpdate: UpdateFromClient
    {
        public EmailService Service { get; set; }
        public bool Delete { get; set; }
    }

    public class RuleUpdate: UpdateFromClient
    {
        public Rule Rule { get; set; }
        public bool Delete { get; set; }
    }

    public class MessageCompleteUpdate: UpdateFromClient
    {
        public Guid Id { get; set; }

    }

    public class MessageDelayUpdate: UpdateFromClient
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
    }

    public class MoveMessageUpdate: UpdateFromClient
    {
        public Guid Id { get; set; }
        public Guid NewCategory { get; set; }
    }

    public class SendMessageUpdate: UpdateFromClient
    {
        public Message Message { get; set; }
    }

    public static class ClientUpdateType
    {
        public const string UpdateService = "update-service";
        public const string UpdateRule = "update-rule";
        public const string MarkMessageComplete = "message-complete";
        public const string MarkMessageForLater = "message-delay";
        public const string MoveMessageToCategory = "message-move";
        public const string SendNewMessage = "new-message";

    }
}