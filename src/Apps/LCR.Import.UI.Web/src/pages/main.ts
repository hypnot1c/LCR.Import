import { autoinject } from "aurelia-framework";
import { Router, RouteConfig, NavigationInstruction, activationStrategy } from "aurelia-router";
import { Store } from "aurelia-store";
import cloneDeep from "clone-deep";
import Pikaday from "pikaday";
import ruI18n from "config/date-picker-config";

import { IAppState } from "config/state/abstractions";
import { BasePageComponent } from "shared/components";
import { DataService } from "services";
import * as importStateActions from "config/state/actions/import-state-actions";


@autoinject
export class MainPage extends BasePageComponent {
  constructor(
    private dataService: DataService,
    protected store: Store<IAppState>,
    private router: Router
  ) {
    super("MainPage");
    this.uploadHistory = [];
  }

  state: IAppState;

  switchList: any[];
  selectedSwitch: number;
  userList: any[];
  selectedUser: number;

  selectedDateFrom: string;
  selectedDateTo: string;

  dateFrom: any;
  dateTo: any;
  private params: any;

  uploadHistory: any[];
  isLoadInProggress: boolean;
  paginationData: { currentPageNumber: number, pageSize: number, totalPages: number }
  currentRouteConfig: RouteConfig;
  currentRouteParams: any;

  selectedSortFieldName: string;
  sortDirection: 'asc' | 'desc';

  determineActivationStrategy() {
    return activationStrategy.invokeLifecycle;
  }

  async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction)
    : Promise<any> {
    this.stateSubscriptions.push(
      this.store.state.subscribe((newState) => {
        this.state = cloneDeep(newState);
      })
    );

    if (params.userId) {
      const userId = parseInt(params.userId);
      await this.store.dispatch(importStateActions.setCurrentUserId, userId);
    }

    this.paginationData = { currentPageNumber: 1, pageSize: 10, totalPages: undefined };
    this.isLoadInProggress = true;
    this.currentRouteConfig = routeConfig;
    this.currentRouteParams = params || {};
    params.page = params.page || 1;
    params.pageSize = params.pageSize || 10;
    params.sortField = params.sortField || "dateUpload";
    params.sortDirection = params.sortDirection || "desc";

    this.paginationData.pageSize = parseInt(params.pageSize);
    this.paginationData.currentPageNumber = parseInt(params.page);

    this.params = params;

    this.selectedSortFieldName = this.params.sortField;
    this.sortDirection = this.params.sortDirection;

    this.dataService.import.getHistory(params)
      .then(resp => {
        if (resp.status != "Ok") {
          this.Logger.error("Error fetching upload history");
          return;
        }
        this.uploadHistory = resp.result.data;
        this.paginationData.totalPages = resp.result.totalPages;
        this.isLoadInProggress = false;
      })
      ;
  }

  async attached() {
    await this.initFilters();
  }

  async initFilters() {
    const promises = Promise.all([
      this.dataService.switch.getList(),
      this.dataService.users.getList()
    ]);

    this.dateFrom = new Pikaday({
      field: document.querySelector('#dateFrom'),
      firstDay: 1,
      i18n: ruI18n,
      format: "L"
    });

    this.dateTo = new Pikaday({
      field: document.querySelector('#dateTo'),
      firstDay: 1,
      i18n: ruI18n,
      format: "L"
    });

    const dateFrom = this.params.dateFrom || null;
    const dateTo = this.params.dateTo || null;

    this.dateFrom.setDate(dateFrom);
    this.dateTo.setDate(dateTo);
    this.selectedSwitch = parseInt(this.params.switchId) || null;
    this.selectedUser = parseInt(this.params.filterUserId) || null;

    const [switches, users] = await promises;
    this.switchList = switches.result;
    this.userList = users.result;
  }

  clearInput(picker: any) {
    picker.setDate(null);
  }

  filter() {
    const params: any = { page: this.paginationData.currentPageNumber, pageSize: this.paginationData.pageSize };

    const dateFromMoment = this.dateFrom.getMoment();
    const dateToMoment = this.dateTo.getMoment();

    params.switchId = this.selectedSwitch;
    params.filterUserId = this.selectedUser;
    params.dateFrom = dateFromMoment._isValid ? dateFromMoment.format("YYYY-MM-DDTHH:mm:ss") : null;
    params.dateTo = dateToMoment._isValid ? dateToMoment.format("YYYY-MM-DDTHH:mm:ss") : null;

    params.sortField = this.selectedSortFieldName;
    params.sortDirection = this.sortDirection;

    this.router.navigateToRoute("root", params);
  }

  async uploadFile() {
    this.router.navigateToRoute("upload-file");
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

      this.filter();
    }
  }
}
