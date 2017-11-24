interface DataServiceOptions {
    connectionUri: string;
    name: string;
    collections: CollectionSchema[];
}

interface CollectionSchema {

}

interface Column {
    name: string;
    dataType: 'string'
}