
import Dexie from 'dexie';
import { UpdateType } from '../enums';

export default class StorageService extends Dexie {
    private categories: Dexie.Table<Category, string>;
    private messages: Dexie.Table<Message, string>;
    private attachments: Dexie.Table<Attachment, string>;
    constructor(
        public name: string,
    ) {
        super(name, {autoOpen: true});
        this.version(1).stores({
            categories: 'id, name',
            messages: 'id, catId, sender, recipients, coppied, blindCopied, subject, content, previousId, isReply',
            attachments: 'id, msgId, name, path'
        });
    }

    public async storeUpdate(update): Promise<any> {
        console.log('StorageService.storeUpdate(', update, ')');
        if (update.updateType == 0) return;
        if ((update.updateType | UpdateType.None ) > 0) {

        }
        if ((update.updateType | UpdateType.Initial) > 0) {
            console.log('StorageService->initial update', update);
            return await this.storeCategories(...update.categories)
        }
        if ((update.updateType | UpdateType.Insert) > 0) {

        }
        if ((update.updateType | UpdateType.Delete) > 0) {

        }
        if ((update.updateType | UpdateType.Modify) > 0) {

        }
    }

    public async storeCategories(...categories: Array<Category>){
        console.log('StorageService.storeCategories(', categories, ')');
        for (let cat of categories) {
            await this.storeCategory(cat);
        }
    }

    private async storeCategory(category: Category) {
        let msgs = category.messages.splice(0);
        let attachments = msgs.reduce((acc, msg) => {
            return acc.concat(msg.attachments.splice(0))
        }, [])
        await this.categories.put({id: category.id, name: category.name});
        await this.messages.bulkPut(msgs);
        await this.attachments.bulkPut(attachments);
    }

    public async getCategories(): Promise<Array<Category>> {
        let cats = await this.categories.toArray();
        for (let cat of cats) {
            cat.messages = await this.messages.where('catId').equals(cat.id).toArray();
            for (let msg of cat.messages) {
                msg.attachments = await this.attachments.where('msgId').equals(msg.id).toArray();
            }
        }
        return cats;
    }
}