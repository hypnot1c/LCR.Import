import { Router, RouteConfig, NavigationInstruction } from "aurelia-router";
import { Store } from "aurelia-store";
import cloneDeep from "clone-deep";

import { IAppState } from "config/state/abstractions";
import { BasePageComponent } from "shared/components";
import { DataService } from "services";

export class ProcessFilePage extends BasePageComponent {
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

  statusCheckInterval: number;
  importStep: number;
  importStatus: string;

  async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
    this.stateSubscriptions.push(
      this.store.state.subscribe((newState) => {
        this.state = cloneDeep(newState);
      })
    );

    this.importStep = 0;
    this.importStatus = "Форматный контроль...";
    this.paginationData = { currentPageNumber: 1, totalPages: undefined };
    this.currentRouteConfig = routeConfig;
    this.currentRouteParams = params || {};

    params.page = params.page || 1;
    this.paginationData.currentPageNumber = parseInt(params.page);

    this.statusCheckInterval = window.setInterval(() => this.statusCheck(this.state.import.currentHistoryId), 2000);
  }

  private async statusCheck(historyId: number) {
    const res = await this.dataService.import.checkStatus(historyId, 1);
    if (res.historyStatus == 1) {
      window.clearInterval(this.statusCheckInterval);
      this.statusCheckInterval = null;

      this.importStatus = "Файл обработан.";

      this.isLoadInProggress = true;

      const resp = await this.dataService.import.getResult(
        historyId,
        1,
        { page: this.paginationData.currentPageNumber, pageSize: 10 }
      );

      this.uploadResultData = resp.result.data;
      this.paginationData.totalPages = resp.result.totalPages;
      this.isLoadInProggress = false;
    }
  }
}
