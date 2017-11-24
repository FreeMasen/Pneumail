import { Event } from "_debugger";


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
        this.worker.addEventListener('message', ev => this.storeWorkerUpdate(ev.data))
        this.connect();
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

    private storeWorkerUpdate(updated: any[]) {

    }


}