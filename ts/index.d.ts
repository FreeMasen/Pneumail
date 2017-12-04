interface Attachment {
    id: string;
    msgId: string;
    name: string;
    path: string;
}

interface Category {
    id: string;
    name: string;
    messages?: Array<Message>;
}


interface Message {
    id: string;
    catId: string;
    sender: string;
    recipients: Array<string>;
    copied?: Array<string>;
    blindCopied?: Array<string>;
    subject: string;
    content: string;
    previousId?: string;
    isReply: boolean;
    attachments?: Array<Attachment>;
}