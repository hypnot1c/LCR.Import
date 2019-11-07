import { autoinject } from "aurelia-framework";
import { Router, RouteConfig, NavigationInstruction, activationStrategy } from "aurelia-router";
import { Store } from "aurelia-store";
import cloneDeep from "clone-deep";

import { IAppState } from "config/state/abstractions";
import { BasePageComponent } from "shared/components";
import { DataService } from "services";

@autoinject
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

  editModes: any;

  determineActivationStrategy() {
    return activationStrategy.invokeLifecycle;
  }

  async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
    this.stateSubscriptions.push(
      this.store.state.subscribe((newState) => {
        this.state = cloneDeep(newState);
      })
    );

    if (!this.state.import.currentHistoryId) {
      this.router.navigateToRoute("upload-file");
    }

    this.editModes = {};

    this.paginationData = { currentPageNumber: 1, totalPages: undefined };
    this.currentRouteConfig = routeConfig;
    this.currentRouteParams = params || {};

    params.page = params.page || 1;
    this.paginationData.currentPageNumber = parseInt(params.page);
    if (this.importStep != 2) {
      this.importStep = 0;
      this.importStatus = "Форматный контроль...";

      this.statusCheckInterval = window.setInterval(() => this.statusCheck(this.state.import.currentHistoryId), 2000);
    }
    else {
      this.isLoadInProggress = true;

      const resp = await this.dataService.import.getResult(
        this.state.import.currentHistoryId,
        params.page,
        { page: this.paginationData.currentPageNumber, pageSize: 10 }
      );

      this.uploadResultData = resp.result.data;
      this.paginationData.totalPages = resp.result.totalPages;
      this.isLoadInProggress = false;
    }
  }

  flagFunc(method, update, value, flag) {
    value = (value & flag) != 0 ? 'diff' : '';
    update(value);
  }

  deactivate() {
    window.clearInterval(this.statusCheckInterval);
    this.statusCheckInterval = null;
  }

  private async statusCheck(historyId: number) {
    const res = await this.dataService.import.checkStatus(historyId, 1);
    switch (res.historyStatus) {
      case 0:
        this.importStep = 0;
        this.importStatus = "Форматный контроль...";
        break;
      case 1:
        this.importStep = 1;
        this.importStatus = "Сопоставление данных";
        break;
      case 2: {
        this.importStep = 2;
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
        break;
      }
      default:
        this.importStep = -1;
        this.Logger.warn("Unknown response");
        window.clearInterval(this.statusCheckInterval);
        this.statusCheckInterval = null;
        break;
    }
  }
}
