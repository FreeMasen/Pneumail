import {DBWorkerState, UpdateType} from '../enums';
import StorageService from './storageService';
let sock;

addEventListener('message', ev => {
    switch (ev.data.event) {
        case 'start':
            startListening(ev.data.uri, ev.data.name);
        break;
    }
});

function startListening(uri: string, name: string) {
    sock = new DBWorker(uri, name);
}

export class DBWorker {
    sock: WebSocket;
    db: StorageService;
    constructor(uri: string, name: string) {
        this.sock = new WebSocket(uri);
        this.sock.addEventListener('open', ev => this.open(ev));
        this.sock.addEventListener('close', ev => this.close(ev));
        this.sock.addEventListener('message', ev => this.receive(ev));
        this.sock.addEventListener('error', ev => this.error(ev));
        this.db = new StorageService(name);
    }

    send(msg) {
        this.sock.send(msg);
    }

    async receive(event: MessageEvent) {
        try {
            let parsed = JSON.parse(event.data);
            await this.db.storeUpdate(parsed);
            postMessage({
                event: DBWorkerState.NewMessage,
                updateType: UpdateType.Initial
            });
        } catch (e) {
            console.error('error in receive', e);
        }
    }

    error(event: Event) {
        postMessage({
            event: DBWorkerState.Error,
        })
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
    }
}