export default class Option {
    constructor(
        public text: string,
        public value: string
    ){}
}

export class SideBarOption extends Option {
    constructor(
        public text: string,
        public value: string,
        public icon: JSX.Element = null
    ) {
        super(text, value);
    }
}