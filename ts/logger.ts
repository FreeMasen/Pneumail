export default class Logger {
    static Debug(componentName: string, ...args: any[]) {
        console.log('debug->'+componentName, ...args);
    }
}