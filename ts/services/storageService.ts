
import Dexie from 'dexie';
import { debug } from 'util';
import { UpdateType } from '../enums';

export default class StorageService extends Dexie {
    private categories: Dexie.Table<Category, string>;
    constructor(
        public name: string,
    ) {
        super(name);
        this.version(1).stores({categories: 'id, name'})
    }

    public storeUpdate(update): Promise<any> {
        console.log('StorageService.storeUpdate(', update, ')');
        if (update.updateType == 0) return;
        if ((update.updateType | UpdateType.None ) > 0) {

        }
        if ((update.updateType | UpdateType.Initial) > 0) {
            console.log('StorageService->initial update', update);
            return this.storeCategories(...update.initial)
        }
        if ((update.updateType | UpdateType.Insert) > 0) {

        }
        if ((update.updateType | UpdateType.Delete) > 0) {

        }
        if ((update.updateType | UpdateType.Modify) > 0) {

        }
    }

    public storeCategories(...categories: Array<Category>): Promise<Array<string>> {
        console.log('StorageService.storeCategories(', categories, ')');
        return new Promise<Array<string>>((resolve, reject) => {
            var results = [];
            for (let cat of categories) {
                this.categories.put(cat, cat.id)
                    .then(res => {
                        results.push(res);
                    })
                .catch(e => {
                    return reject(e);
                });
            }
            return resolve(results);
        })
    }

    /*
    (executor:
        (resolve: (value?: string | PromiseLike<string>) => void
        , reject: (reason?: any) => void) => void) => Promise<string>
    */

    public getCategories(): Promise<Array<Category>> {
        return this.categories.toArray()
    }
}