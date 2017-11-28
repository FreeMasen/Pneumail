import {DBWorkerState} from '../enums';
let sock;
console.log('hello from worker');

addEventListener('message', ev => {
    console.log('message', ev);
    switch (ev.data.event) {
        case 'start':
            startListening(ev.data.uri);
        break;
    }
});

function startListening(uri) {
    console.log('startListening');
    sock = new DBWorker(uri);
}

export class DBWorker {
    sock: WebSocket;
    constructor(uri: string) {
        console.log('new DBWorker', uri);
        this.sock = new WebSocket(uri);
        this.sock.addEventListener('open', ev => this.open(ev));
        this.sock.addEventListener('close', ev => this.close(ev));
        this.sock.addEventListener('message', ev => this.receive(ev));
        this.sock.addEventListener('error', ev => this.error(ev));
        setInterval(() => this.send('ping'), 1000);
    }

    send(msg) {
        this.sock.send(msg);
    }

    receive(event: MessageEvent) {
        console.log('worker->socket->receive', JSON.stringify(event.data));
        try {
            let parsed = JSON.parse(event.data);
            this.send(JSON.stringify(parsed));
        } catch (e) {
            console.error('error in receive', e);
        }
    }

    error(event: Event) {
        console.log('worker->socket->error', JSON.stringify(event));
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