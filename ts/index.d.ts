interface Attachment {
    id: string;
    name: string;
    path: string;
}

interface Category {
    id: string;
    name: string;
    messages: Array<Message>
}

interface EmailAddress {
    id: string;
    username: string;
    host: string;
    domain: string;
}

interface Message {
    id: string;
    sender: EmailAddress;
    recipients: Array<EmailAddress>;
    copied?: Array<EmailAddress>;
    blindCopied?: Array<EmailAddress>;
    subject: string;
    content: string;
    attachments: Array<Attachment>;
    previousId?: string;
    isReply: boolean;
}