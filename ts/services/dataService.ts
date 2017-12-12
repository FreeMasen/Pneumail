import StorageService from './storageService';
import { UpdateType } from '../enums';
type Listener = (update: IUpdate) => void;
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
    private listeners: Array<Listener> = [];
     /**
      * Create a new DataService
      * @param {string} connectionURI The websocket URI/Path
      * @param {string} name The DB Name
      */
    constructor(
        private connectionURI: string,
        name: string) {
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

    public async listen(listener: Listener)  {
        this.listeners.push(listener);
        //send the last state we had

    }

    private async sendCategories() {
        let update = {
            event: 'categories',
            data: await this.storageService.getCategories(),
        }
        for (let listener of this.listeners) {
            listener(update);
        }
    }

    private async sendServices() {
        let update = {
            event: 'services',
            data: await this.storageService.getServices(),
        }
        for (let listener of this.listeners) {
            listener(update);
        }
    }

    private async sendRules() {
        let update = {
            event: 'rules',
            data: await this.storageService.getRules(),
        }
        for (let listener of this.listeners) {
            listener(update);
        }
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

    private async newMessage(updateType: UpdateType) {
        // console.log('DataService.newMessage(', updateType, ')');
        switch (updateType) {
            case UpdateType.Initial:
                for (let listener of this.listeners) {
                    this.sendCategories();
                    this.sendServices();
                    this.sendRules();
                }
            break;
        }
    }

    public sendServiceUpdate(service: IEmailService) {
        this.worker.postMessage(
            {
                event: 'update-service',
                service: service
            }
        );
    }
}