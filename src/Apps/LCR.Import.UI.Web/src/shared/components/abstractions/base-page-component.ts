import { RouteConfig, NavigationInstruction, NavigationCommand } from 'aurelia-router';
import { Subscription } from 'aurelia-event-aggregator';

import { BaseComponent } from './base-component';

export abstract class BasePageComponent extends BaseComponent {
  constructor(
    loggerName: string
  ) {
    loggerName = !!loggerName ? loggerName : "BasePageComponent";
    super(loggerName);

    this.lifecycleLongSubscriptions = [];
  }

  protected lifecycleLongSubscriptions: Subscription[];

  public canActivate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction)
    : boolean | Promise<boolean> | NavigationCommand {
    return true;
  }


  public activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction)
    : Promise<any> | Promise<void> | void {

  }

  public canDeactivate()
    : boolean | Promise<boolean> | NavigationCommand {
    return true;
  }

  public deactivate() {
  }

  public detached() {
    while (this.lifecycleLongSubscriptions.length > 0) {
      this.lifecycleLongSubscriptions.pop().dispose();
    }
  }
}
