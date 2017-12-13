import {DBWorkerState, UpdateType} from '../enums';
import StorageService from './storageService';
let sock;

addEventListener('message', ev => {
    switch (ev.data.event) {
        case 'start':
            startListening(ev.data.uri, ev.data.name);
        break;
        case 'update-service':
            if (sock) {
                let forServer = {
                    updateType: ev.data.event,
                    service: ev.data.service,
                    delete: false
                }
                sock.send(JSON.stringify(forServer));
            }
        break;
        case 'update-rule':
            if (sock) {
                let forServer = {
                    updateType: ev.data.event,
                    rule: ev.data.rule,
                    delete: false,
                };
                sock.send(JSON.stringify(forServer));
            }
        break;
    }
});

function startListening(uri: string, name: string) {
    sock = new DBWorker(uri, name);
}

export class DBWorker {
    uri: string;
    name: string;
    sock: WebSocket;
    db: StorageService;

    constructor(uri: string, name: string) {
        this.uri = uri;
        this.name = name;
        this.startListening();
        this.db = new StorageService(name);
    }

    startListening() {
        this.sock = new WebSocket(this.uri);
        this.sock.addEventListener('open', ev => this.open(ev));
        this.sock.addEventListener('close', ev => this.close(ev));
        this.sock.addEventListener('message', ev => this.receive(ev));
        this.sock.addEventListener('error', ev => this.error(ev));
    }

    send(msg: any) {
        this.sock.send(msg);
    }

    async receive(event: MessageEvent) {
        try {
            let parsed = JSON.parse(event.data);
            console.log(parsed);
            await this.db.storeUpdate(parsed);
            postMessage({
                event: DBWorkerState.NewMessage,
                updateType: parsed.updateType
            });
        } catch (e) {
            console.error('error in receive', e);
        }
    }

    async fromServer(message) {

    }

    error(event: Event) {
        postMessage({
            event: DBWorkerState.Error,
        });
        if (this.sock.readyState == WebSocket.CLOSED) {
            this.startListening();
        }
    }

    open(event: Event) {
        postMessage({
            event: DBWorkerState.Ready
        });
    }

    close(event: Event) {
        postMessage({
            event: DBWorkerState.NotReady
        });
        this.startListening();
    }
}