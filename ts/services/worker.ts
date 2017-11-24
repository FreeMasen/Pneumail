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
        this.sock.addEventListener('open', ev => console.log('worker->socket->open', JSON.stringify(ev)));
        this.sock.addEventListener('close', ev => console.log('worker->socket->close', JSON.stringify(ev)));
        this.sock.addEventListener('message', ev => console.log(ev.data));
        this.sock.addEventListener('error', ev => console.log('worker->socket->error', JSON.stringify(ev)));
        setInterval(() => this.send('things'), 5000)
    }

    send(msg) {
        this.sock.send(msg);
    }

    receive(event: Event) {

    }

    error(event: Event) {

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