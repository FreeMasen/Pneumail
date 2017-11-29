import {DBWorkerState} from '../enums';
let sock;

addEventListener('message', ev => {
    switch (ev.data.event) {
        case 'start':
            startListening(ev.data.uri);
        break;
    }
});

function startListening(uri) {
    sock = new DBWorker(uri);
}

export class DBWorker {
    sock: WebSocket;
    constructor(uri: string) {
        this.sock = new WebSocket(uri);
        this.sock.addEventListener('open', ev => this.open(ev));
        this.sock.addEventListener('close', ev => this.close(ev));
        this.sock.addEventListener('message', ev => this.receive(ev));
        this.sock.addEventListener('error', ev => this.error(ev));
    }

    send(msg) {
        this.sock.send(msg);
    }

    receive(event: MessageEvent) {
        try {
            let parsed = JSON.parse(event.data);
            postMessage({
                event: DBWorkerState.NewMessage,
                update: parsed
            });
        } catch (e) {
            console.error('error in receive', e);
        }
    }

    error(event: Event) {
        console.error('worker->socket->error', JSON.stringify(event));
        postMessage({
            event: DBWorkerState.Error,
        })
    }

    open(event: Event) {
        console.log('worker->socket->open', JSON.stringify(event))
        postMessage({
            event: DBWorkerState.Ready
        });
    }

    close(event: Event) {
        console.log('worker->socket->close', JSON.stringify(event));
        postMessage({
            event: DBWorkerState.NotReady
        });
    }
}