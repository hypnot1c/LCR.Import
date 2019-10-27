import { Subscription } from 'aurelia-event-aggregator';
import { Subscription as StateSubscription } from "rxjs";

import { BaseObject } from "shared/abstractions";

export abstract class BaseComponent extends BaseObject {
    constructor(loggerName: string) {
        super(loggerName ? loggerName : "BaseComponent");
        this.subscriptions = [];
        this.stateSubscriptions = [];
    }

    protected subscriptions: Subscription[];
    protected stateSubscriptions: StateSubscription[];

    public created() { }

    public bind() { }

    public attached(): Promise<any> | Promise<void> | void { }

    public detached() { }

    public unbind() {
        while (this.subscriptions.length > 0) {
            this.subscriptions.pop().dispose();
        }
        while (this.stateSubscriptions.length > 0) {
            this.stateSubscriptions.pop().unsubscribe();
        }
    }
}
