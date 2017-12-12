
interface IAttachment {
    id: string;
    msgId: string;
    name: string;
    path: string;
}

interface ICategory {
    id: string;
    name: string;
    messages?: Array<IMessage>;
}

interface IMessage {
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
    attachments?: Array<IAttachment>;
}

interface IEmailService {
    id?: string;
    address: string;
    port: number;
    username: string;
    password?: string;
    confirmPassword?: string;
}

interface IRule {
    id?: string;
    searchTerm: string;
    location: number;
}

interface IUpdate {
    event: string;
    data: any;
}