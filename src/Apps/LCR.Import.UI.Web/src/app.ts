import { autoinject } from "aurelia-framework";
import { Router, RouterConfiguration, NavigationInstruction, RouteConfig } from "aurelia-router";
import { HttpClient } from "aurelia-fetch-client";
import { Store, logMiddleware, MiddlewarePlacement } from "aurelia-store";
import { pluck } from "rxjs/operators";
import cloneDeep from "clone-deep";

import { Environment } from "config/environment";
import appConfig from "config/app-config";
import routeConfig from "./config/app-route-config";

import { IAppState } from "./config/state/abstractions";

import { BasePageComponent } from "shared/components";

@autoinject
export class App extends BasePageComponent {
    constructor(
        protected router: Router,
        protected http: HttpClient,
        protected store: Store<IAppState>
    ) {
        super("App");
        this.state = {} as IAppState;

        this.configureHttpClient();
        this.configureState();
        this.configureEvents();
    }

    state: IAppState;

    async activate(params, route: RouteConfig, navigationInstruction: NavigationInstruction) {
    }

    attached() {
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = "LCR Import";
        config.options.pushState = true;
        config.map(routeConfig.routes);

        this.router = router;
    }

    private configureHttpClient() {
    }

    private async configureState() {
        if (appConfig.Environment < Environment.PROD) {
            this.store.registerMiddleware(logMiddleware, MiddlewarePlacement.Before, {
                logType: "debug"
            });
        }
    }

    private configureEvents(): void {
    }
}
