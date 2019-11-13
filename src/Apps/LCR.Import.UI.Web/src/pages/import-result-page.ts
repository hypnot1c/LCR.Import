import { autoinject } from "aurelia-framework";
import { Router, activationStrategy, RouteConfig, NavigationInstruction } from "aurelia-router";
import { Store } from "aurelia-store";
import cloneDeep from "clone-deep";

import { IAppState } from "config/state/abstractions";
import { BasePageComponent } from "shared/components";
import { DataService } from "services";
import * as importStateActions from "config/state/actions/import-state-actions";

@autoinject
export class ImportResultPage extends BasePageComponent {
  constructor(
    private dataService: DataService,
    private router: Router,
    protected store: Store<IAppState>
  ) {
    super("ProcessFilePage");
  }

  state: IAppState;

  uploadResultData: any[];
  isLoadInProggress: boolean;
  paginationData: { currentPageNumber: number, totalPages: number }
  currentRouteConfig: RouteConfig;
  currentRouteParams: any;

  summary: any;

  determineActivationStrategy() {
    return activationStrategy.invokeLifecycle;
  }

  async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
    this.stateSubscriptions.push(
      this.store.state.subscribe((newState) => {
        this.state = cloneDeep(newState);
      })
    );

    await this.store.dispatch(importStateActions.setCurrentHistoryId, params.id);

    this.uploadResultData = [];
    this.paginationData = this.paginationData || { currentPageNumber: 1, totalPages: undefined };
    this.currentRouteConfig = routeConfig;
    this.currentRouteParams = params || {};

    params.page = params.page || 1;
    this.paginationData.currentPageNumber = parseInt(params.page);
    this.isLoadInProggress = true;


    const [summaryResp, resp] = await Promise.all([
      this.dataService.import.getImportSummary(this.state.import.currentHistoryId),
      this.dataService.import.getResult(
        this.state.import.currentHistoryId,
        params.page,
        { page: this.paginationData.currentPageNumber, pageSize: 50 }
      )
    ]);

    this.summary = summaryResp.result;

    this.uploadResultData = resp.result.data;
    this.paginationData.totalPages = resp.result.totalPages;
    this.isLoadInProggress = false;
  }
}
