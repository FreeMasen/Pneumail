import StorageService from './storageService';
import { UpdateType } from '../enums';
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
    private storageService: StorageService;
    private listeners = [];
     /**
      * Create a new DataService
      * @param {string} connectionURI The websocket URI/Path
      * @param {string} name The DB Name
      */
    constructor(
        private connectionURI: string,
        name: string) {
        // console.log('new DataService(', connectionURI, name, ')');
        this.worker = new Worker('/js/worker.js');
        this.worker.postMessage({
            event: 'start',
            uri: this.connectionURI,
            name: name,
        });
        this.worker.addEventListener('message',
            ev => this.messageFromWorker(ev.data))
        this.storageService = new StorageService(name);
    }

    public async listen(listener)  {
        this.listeners.push(listener);
        //send the last state we had
        listener(await this.storageService.getCategories());
    }

    private messageFromWorker(msg: any) {
        // console.log('DataService.messageFromWorker(', msg, ')');
        switch (msg.event) {
            case 'new-message':
                this.newMessage(msg.updateType);
            break;
            case 'ready':
                // console.log('worker ready');
            break;
            case 'not-ready':
                // console.log('worker not ready');
            break;
        }
    }

    private newMessage(updateType: UpdateType) {
        // console.log('DataService.newMessage(', updateType, ')');
        switch (updateType) {
            case UpdateType.Initial:
                // console.log('UpdateType.Initial');
                this.storageService.getCategories()
                    .then(categories => {
                        // console.log('storageService.getCategories()', categories);
                        for (let listener of this.listeners) {
                            listener(categories);
                        }
                    })
        }
    }
}