import { Subscription } from "aurelia-event-aggregator";

import { BaseObject } from 'shared/abstractions';

export class BaseCustomAttribute extends BaseObject {
  constructor(
    loggerName: string,
    protected element: Element
  ) {
    super(loggerName ? loggerName : "BaseCustomAttribute");
    this.subscriptions = [];
  }

  protected subscriptions: Subscription[];

  attached(): void | Promise<void> {
  }

  detached(): void | Promise<void> {
    while (this.subscriptions.length > 0) {
      this.subscriptions.pop().dispose();
    }
  }
}
