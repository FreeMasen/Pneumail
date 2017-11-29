
/**
 * Main data service provider. This class
 * will deal with the result of the webworker's
 * socket connection updates and persist the data
 * on the client in the IndexDB
 * ```
 *
 * ```
 */
export default class DataService {
    private worker: Worker;
    private db: IDBDatabase;
    private listeners = [];
     /**
      * Create a new DataService
      * @param {string} connectionURI The websocket URI/Path
      * @param {string} name The DB Name
      */
    constructor(
        private connectionURI: string,
        public name: string) {
        this.worker = new Worker('/js/worker.js');
        this.worker.postMessage({
            event: 'start',
            uri: this.connectionURI
        });
        this.worker.addEventListener('message', ev => this.messageFromWorker(ev.data))
        this.connect();
    }

    public listen(listener) {
        this.listeners.push(listener);
    }

    private connect() {
        let dbOpenReq = indexedDB.open(this.name);
        dbOpenReq.addEventListener('success', ev => this.openSuccessCallback(ev));
        dbOpenReq.addEventListener('error', ev => this.openErrorCB(ev));
    }

    private openSuccessCallback(event) {
        let req = event.currentTarget as IDBOpenDBRequest;
        this.db = req.result;
    }

    private openErrorCB(event) {
        let req = event.currentTarget as IDBOpenDBRequest;
        throw req.error
    }

    private openUpgradeNeededCB(event) {
        let req = event.currentTarget as IDBOpenDBRequest;
        if (!this.db) {
            this.db = req.result;
        }
        let messages = this.db.createObjectStore('messages');
        messages.createIndex('id', 'id', {unique: true});
        messages.createIndex('sender', 'sender');
        messages.createIndex('subject', 'subject');
        messages.createIndex('content', 'content');
    }

    private messageFromWorker(msg: any) {
        switch (msg.event) {
            case 'new-message':
                this.storeWorkerUpdate(msg.update)
            break;
            case 'ready':
                console.log('worker ready');
            break;
            case 'not-ready':
                console.log('worker not ready');
            break;
        }
    }

    private storeWorkerUpdate(updated: any) {
        console.log('storeWorkerUpdate', updated);
        if (updated.updateType == 1) {
            for (let listener of this.listeners) {
                listener(updated.initial[0].categories)
            }
        }
    }


}