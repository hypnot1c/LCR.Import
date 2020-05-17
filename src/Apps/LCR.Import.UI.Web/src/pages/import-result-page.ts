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
  paginationData: { currentPageNumber: number, pageSize: number, totalPages: number }
  currentRouteConfig: RouteConfig;
  currentRouteParams: any;

  selectedSortFieldName: string;
  sortDirection: 'asc' | 'desc';

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
    this.paginationData = this.paginationData || { currentPageNumber: 1, pageSize: 10, totalPages: undefined };
    this.currentRouteConfig = routeConfig;
    this.currentRouteParams = params || {};

    params.page = params.page || 1;
    params.pageSize = params.pageSize || 10;
    params.sortField = params.sortField || "dataRowId";
    params.sortDirection = params.sortDirection || "asc";

    this.selectedSortFieldName = params.sortField;
    this.sortDirection = params.sortDirection;

    this.paginationData.pageSize = parseInt(params.pageSize);
    this.paginationData.currentPageNumber = parseInt(params.page);
    this.isLoadInProggress = true;

    const [summaryResp, resp] = await Promise.all([
      this.dataService.import.getImportSummary(this.state.import.currentHistoryId),
      this.dataService.import.getResult(
        this.state.import.currentHistoryId,
        this.state.userId,
        params
      )
    ]);

    this.summary = summaryResp.result;

    this.uploadResultData = resp.result.data;
    this.paginationData.totalPages = resp.result.totalPages;
    this.isLoadInProggress = false;
  }

  onSortChanged() {
    const params: any = { page: this.paginationData.currentPageNumber, pageSize: this.paginationData.pageSize };

    params.sortField = this.selectedSortFieldName;
    params.sortDirection = this.sortDirection;
    params.id = this.currentRouteParams.id;

    this.router.navigateToRoute("import-result", params);
  }

  onTableHeaderClick(event: Event) {
    const el: HTMLElement = (event.target || event.srcElement) as HTMLElement;
    const fieldName = el.dataset["fieldName"];
    if (fieldName) {
      if (this.selectedSortFieldName == fieldName) {
        switch (this.sortDirection) {
          case "asc":
            this.sortDirection = "desc";
            break;
          case "desc":
          default:
            this.sortDirection = "asc";
            break;
        }
      }
      else {
        this.sortDirection = 'asc';
        this.selectedSortFieldName = fieldName;
      }

      this.onSortChanged();
    }
  }
}
