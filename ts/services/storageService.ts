
import Dexie from 'dexie';
import { UpdateType } from '../enums';

export default class StorageService extends Dexie {
    private categories: Dexie.Table<ICategory, string>;
    private messages: Dexie.Table<IMessage, string>;
    private attachments: Dexie.Table<IAttachment, string>;
    private rules: Dexie.Table<IRule, string>;
    private services: Dexie.Table<IEmailService, string>;

    constructor(
        public name: string,
    ) {
        super(name, {autoOpen: true});
        this.version(1).stores({
            categories: 'id, name',
            messages: 'id, catId, sender, recipients, coppied, blindCopied, subject, content, previousId, isReply',
            attachments: 'id, msgId, name, path',
            rules: 'id, searchTerm, location',
            services: 'id, address, port, username'
        });
    }

    public async storeUpdate(update): Promise<any> {
        if (update.updateType == 0) return;
        if ((update.updateType & UpdateType.None ) > 0) {

        }
        if ((update.updateType & UpdateType.Initial) > 0) {
            await this.storeCategories(...update.categories)
            await this.cullCategories(...update.categories);
            await this.storeServices(...update.services);
            await this.cullServices(update.services);
            await this.storeRules(...update.rules);
            return await this.cullRules(update.rules);
        }
        if ((update.updateType & UpdateType.Insert) > 0) {

        }
        if ((update.updateType & UpdateType.Delete) > 0) {

        }
        if ((update.updateType & UpdateType.Modify) > 0) {

        }
        if ((update.updateType & UpdateType.ServiceUpdateConfirmation) > 0) {

        }
        if ((update.updateType & UpdateType.RuleUpdateConfirmation) > 0) {

        }
    }

    public async storeCategories(...categories: Array<ICategory>) {
        for (let cat of categories) {
            await this.storeCategory(cat);
        }
    }

    public async cullCategories(...categories: Array<ICategory>) {
        for (let cat of categories) {
            await this.cullMessages(...cat.messages);
        }
        let ids = categories.map(c => c.id);
        let toBeDeleted = await this.categories.filter(c => ids.indexOf(c.id) <= -1);
        let toBeDelIds = await toBeDeleted.keys();
    }

    private async cullMessages(...messages: Array<IMessage>) {
        let ids = messages.map(m => m.id);
        let toBeDeleted = await this.messages.filter(m => ids.indexOf(m.id) <= -1);
        let toBeDelIds = await toBeDeleted.keys();
        await this.messages.bulkDelete(toBeDelIds);
    }

    private async storeCategory(category: ICategory) {
        let msgs = category.messages.splice(0);
        let attachments = msgs.reduce((acc, msg) => {
            return acc.concat(msg.attachments.splice(0))
        }, [])
        await this.categories.put({id: category.id, name: category.name});
        await this.messages.bulkPut(msgs);
        await this.attachments.bulkPut(attachments);
    }

    public async getCategories(): Promise<Array<ICategory>> {
        let cats = await this.categories.toArray();
        for (let cat of cats) {
            cat.messages = await this.messages.where('catId').equals(cat.id).toArray();
            for (let msg of cat.messages) {
                msg.attachments = await this.attachments.where('msgId').equals(msg.id).toArray();
            }
        }
        return cats;
    }

    public async storeRules(...rules: Array<IRule>) {
        this.rules.bulkPut(rules);
    }

    public async cullRules(rules: Array<IRule>) {
        let ids = rules.map(r => r.id);
        let toBeDeleted = await this.rules.filter(r => ids.indexOf(r.id) <= -1);
        let toBeDelIds = await toBeDeleted.keys();
        this.rules.bulkDelete(toBeDelIds);
    }

    public async getRules() {
        return await this.rules.toArray()
    }

    public async storeServices(...services: Array<IEmailService>) {

        await this.services.bulkPut(services.map(s => {
            return {
                id: s.id,
                address: s.address,
                port: s.port,
                username: s.username
            };
        }));
    }

    public async cullServices(services: Array<IEmailService>) {
        if (services.length == 0) {
            return await this.services.clear()
        }
        let ids = services.map(s => s.id);
        let toBeDeleted = await this.rules.filter(s => ids.indexOf(s.id) <= -1);
        let toBeDelIds = await toBeDeleted.keys();
        await this.services.bulkDelete(toBeDelIds);
    }

    public async getServices() {
        return this.services.toArray();
    }
}