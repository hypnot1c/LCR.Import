import { autoinject } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
import { Router, RouteConfig, NavigationInstruction, activationStrategy } from "aurelia-router";
import { Store } from "aurelia-store";
import cloneDeep from "clone-deep";
import UIkit from "uikit";
import Pikaday from "pikaday";
import ruI18n from "config/date-picker-config";

import { IAppState } from "config/state/abstractions";
import { BasePageComponent } from "shared/components";
import { DataService } from "services";
import * as importStateActions from "config/state/actions/import-state-actions";

@autoinject
export class ProcessFilePage extends BasePageComponent {
  constructor(
    private ea: EventAggregator,
    private router: Router,
    private dataService: DataService,
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

  statusCheckInterval: number;
  importStep: number;
  importStatus: string;
  summary: any;
  allRowsAreApproved: boolean;
  operatorList: any[];
  selectedOperatorId: number;
  selectedFilter: string;
  selectedRowErrorFlags: number;
  showErrorDialog: boolean;

  selectedSortFieldName: string;
  sortDirection: 'asc' | 'desc';

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

    this.operatorList = [];
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
    this.selectedFilter = params.rowFilter;

    if (this.importStep != 2) {
      this.importStep = 0;
      this.importStatus = "Форматный контроль...";

      this.statusCheckInterval = window.setInterval(() => this.statusCheck(this.state.import.currentHistoryId), 2000);
    }
    else {
      this.isLoadInProggress = true;

      params.rowFilter = this.selectedFilter == "null" ? null : this.selectedFilter;
      const resp = await this.dataService.import.getResult(
        this.state.import.currentHistoryId,
        this.state.userId,
        params
      );

      this.uploadResultData = resp.result.data;
      this.paginationData.totalPages = resp.result.totalPages;
      this.isLoadInProggress = false;
    }
  }

  deactivate() {
    window.clearInterval(this.statusCheckInterval);
    this.statusCheckInterval = null;
  }

  flagFunc(method, update, value, flag) {
    value = flag && ((value & flag) != 0) ? 'diff' : '';
    update(value);
  }

  async approveRow(row: any) {
    const resp = await this.dataService.import.setRowApproved(this.state.import.currentHistoryId, row.id, true);
    if (resp.status == "Ok") {
      row.approved = true;
      row.excluded = false;

      const index = this.uploadResultData.indexOf(row);
      this.uploadResultData.splice(index, 1, resp.result);

      const resp2 = await this.dataService.import.isAllApproved(this.state.import.currentHistoryId);
      if (resp2.status == "Ok") {
        this.allRowsAreApproved = resp2.result;
      }
    }
  }

  async disapproveRow(row: any) {
    const resp = await this.dataService.import.setRowApproved(this.state.import.currentHistoryId, row.id, false);
    if (resp.status == "Ok") {
      row.approved = false;

      const resp2 = await this.dataService.import.isAllApproved(this.state.import.currentHistoryId);
      if (resp2.status == "Ok") {
        this.allRowsAreApproved = resp2.result;
      }
    }
  }

  async excludeRow(row: any) {
    const resp = await this.dataService.import.setRowExcluded(this.state.import.currentHistoryId, row.id, true);
    if (resp.status == "Ok") {
      row.excluded = true;
      row.approved = false;

      const resp2 = await this.dataService.import.isAllApproved(this.state.import.currentHistoryId);
      if (resp2.status == "Ok") {
        this.allRowsAreApproved = resp2.result;
      }
    }
  }

  async includeRow(row: any) {
    const resp = await this.dataService.import.setRowExcluded(this.state.import.currentHistoryId, row.id, false);
    if (resp.status == "Ok") {
      row.excluded = false;
      row.approved = false;

      const resp2 = await this.dataService.import.isAllApproved(this.state.import.currentHistoryId);
      if (resp2.status == "Ok") {
        this.allRowsAreApproved = resp2.result;
      }
    }
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
        window.clearInterval(this.statusCheckInterval);
        this.statusCheckInterval = null;

        this.importStatus = "Файл обработан.";

        const summaryResp = await this.dataService.import.getImportSummary(this.state.import.currentHistoryId);
        this.summary = summaryResp.result;

        this.importStep = 2;

        this.isLoadInProggress = true;

        const rowFilter = this.selectedFilter;
        const resp = await this.dataService.import.getResult(
          historyId,
          this.state.userId,
          { page: this.paginationData.currentPageNumber, pageSize: this.paginationData.pageSize, rowFilter: rowFilter }
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

  async edit(entry: any) {
    const dialogPromise = new Promise(async (res, rej) => {
      const el = document.getElementById('edit-dialog');
      const btnSave: HTMLButtonElement = el.querySelector("button.uk-button-primary");

      let resolved = false;

      const resp = await this.dataService.operator.getList();
      if (resp.status == "Ok") {
        this.operatorList = resp.result;
      }

      const op = this.operatorList.find(o => o.id == entry.lcrOperatorId);
      if (op) {
        this.selectedOperatorId = op.id;
      }

      var picker = new Pikaday({
        field: el.querySelector('#validUntil'),
        firstDay: 1,
        i18n: ruI18n,
        format: "L"
      });
      picker.setDate(entry.lcrDateClose);

      const onEditFinish = () => {
        btnSave.removeEventListener("click", saveHandler);
        el.removeEventListener("hidden", cancelhandler);
        picker.destroy();
        this.selectedOperatorId = undefined;
        (el.querySelector('#validUntil') as any).value = null;
      };

      const saveHandler = async () => {
        resolved = true;
        UIkit.modal(el).hide();
        var moment = picker.getMoment();
        const op = this.operatorList.find(o => o.id == this.selectedOperatorId);
        const dateClose = moment._isValid ? moment.format("YYYY-MM-DDTHH:mm:ss") : null;

        const resp = await this.dataService.import.saveRow(
          this.state.import.currentHistoryId,
          entry.id,
          { lcrOperatorId: op.id, lcrDateClose: dateClose }
        );

        if (resp.status == "Ok") {
          const index = this.uploadResultData.indexOf(entry);
          this.uploadResultData.splice(index, 1, resp.result);
        }

        onEditFinish();
        res();
      };

      const cancelhandler = () => {
        if (!resolved) {
          onEditFinish();
          rej();
        }
      };

      btnSave.addEventListener("click", saveHandler);
      el.addEventListener("hidden", cancelhandler);
      UIkit.modal(el).show();
    });

    dialogPromise
      .then(() => { this.Logger.info("SAVED!") })
      .catch(() => { this.Logger.info("CANCELED!") })
      ;
  }

  showFormatErrors(entry: any) {
    this.selectedRowErrorFlags = entry.formatFlags;
    this.showErrorDialog = true;
    this.ea.subscribeOnce("dialog:error:finish", () => {
      this.showErrorDialog = false;
      this.selectedRowErrorFlags = undefined;
    })
  }

  onFilterChanged() {
    const routeParams: any = {
      page: 1
    };

    if (this.selectedFilter) {
      routeParams.rowFilter = this.selectedFilter;
    }

    routeParams.sortField = this.selectedSortFieldName;
    routeParams.sortDirection = this.sortDirection;
    routeParams.id = this.currentRouteParams.id;

    this.router.navigateToRoute("process-file", routeParams);
  }

  onSortChanged() {
    const params: any = { page: this.paginationData.currentPageNumber, pageSize: this.paginationData.pageSize };

    if (this.selectedFilter) {
      params.rowFilter = this.selectedFilter;
    }

    params.sortField = this.selectedSortFieldName;
    params.sortDirection = this.sortDirection;
    params.id = this.currentRouteParams.id;

    this.router.navigateToRoute("process-file", params);
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

  async lcrSave() {
    if (this.allRowsAreApproved) {
      this.importStatus = "Сохранение в LCR";
      this.importStep = 3;
      const resp = await this.dataService.import.lcrSave(this.state.import.currentHistoryId);
      if (resp.status == "Ok") {
        this.importStep = 4;
        this.importStatus = "Сохранено в LCR";

        window.setTimeout(async () => {
          await this.store.dispatch(importStateActions.setCurrentHistoryId, undefined);
          this.router.navigateToRoute("root");
        }, 500);
      }
    }
  }
}
