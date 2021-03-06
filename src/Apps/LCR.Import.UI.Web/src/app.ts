import { autoinject } from "aurelia-framework";
import { Router, RouterConfiguration } from "aurelia-router";
import { HttpClient as FetchClient } from "aurelia-fetch-client";
import { HttpClient } from "aurelia-http-client";
import { Store, logMiddleware, MiddlewarePlacement } from "aurelia-store";

import { BasePageComponent } from "shared/components";
import { Environment } from "config/environment";
import appConfig from "config/app-config";
import routeConfig from "./config/app-route-config";
import { IAppState } from "./config/state/abstractions";


@autoinject
export class App extends BasePageComponent {
  constructor(
    protected router: Router,
    protected fetchClient: FetchClient,
    protected httpClient: HttpClient,
    protected store: Store<IAppState>
  ) {
    super("App");
    this.state = {} as IAppState;

    this.configureHttpClient();
    this.configureState();
    this.configureEvents();
  }

  state: IAppState;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "LCR Import";
    config.options.pushState = true;
    config.map(routeConfig.routes);
    config.options.root = appConfig.root;

    this.router = router;
  }

  private configureHttpClient() {
    this.fetchClient.configure(c => {
      let defaults: any = {
        credentials: "same-origin",
        headers: {
          Accept: "application/json",
          "X-Requested-With": "Fetch",
          "Cache-Control": "no-cache",
          "Pragma": "no-cache"
        }
      };

      c.withBaseUrl(`${appConfig.ApiHost}/`);
      c.withDefaults(defaults);
    });

    this.httpClient.configure(c => {
      c.withBaseUrl(`${appConfig.ApiHost}/`);
      c.withHeader("Accept", "application/json");
      c.withHeader("Cache-Control", "no-cache");
      c.withHeader("Pragma", "no-cache");
    });
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
