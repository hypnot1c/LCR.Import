import { LogManager } from 'aurelia-framework';
import { Logger } from 'aurelia-logging';

export abstract class BaseObject {
    constructor(
        loggerName: string
    ) {
        this.Logger = LogManager.getLogger(loggerName ? loggerName : "BaseObject");
    }

    protected Logger: Logger;
}
