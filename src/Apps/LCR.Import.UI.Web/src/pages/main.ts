import { autoinject } from "aurelia-framework";
import { Router, RouteConfig, NavigationInstruction, activationStrategy } from "aurelia-router";
import Pikaday from "pikaday";
import ruI18n from "config/date-picker-config";

import { BasePageComponent } from "shared/components";
import { DataService } from "services";

@autoinject
export class MainPage extends BasePageComponent {
  constructor(
    private dataService: DataService,
    private router: Router
  ) {
    super("MainPage");
    this.uploadHistory = [];
  }

  switchList: any[];
  selectedSwitch: number;
  selectedDateFrom: string;
  selectedDateTo: string;

  dateFrom: any;
  dateTo: any;
  private params: any;

  uploadHistory: any[];
  isLoadInProggress: boolean;
  paginationData: { currentPageNumber: number, totalPages: number }
  currentRouteConfig: RouteConfig;
  currentRouteParams: any;

  determineActivationStrategy() {
    return activationStrategy.invokeLifecycle;
  }

  async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction)
    : Promise<any> {
    this.Logger.info("activate");
    this.paginationData = { currentPageNumber: 1, totalPages: undefined };
    this.isLoadInProggress = true;
    this.currentRouteConfig = routeConfig;
    this.currentRouteParams = params || {};

    params.page = params.page || 1;
    this.paginationData.currentPageNumber = parseInt(params.page);

    this.params = params;

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
    const switchPromise = this.dataService.switch.getList();

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

    this.switchList = await switchPromise;
  }

  clearInput(picker: any) {
    picker.setDate(null);
  }

  filter() {
    const params: any = { page: this.paginationData.currentPageNumber, pageSize: 10 };

    const dateFromMoment = this.dateFrom.getMoment();
    const dateToMoment = this.dateTo.getMoment();

    params.switchId = this.selectedSwitch;
    params.dateFrom = dateFromMoment._isValid ? dateFromMoment.format("YYYY-MM-DDTHH:mm:ss") : null;
    params.dateTo = dateToMoment._isValid ? dateToMoment.format("YYYY-MM-DDTHH:mm:ss") : null;

    this.router.navigateToRoute("root", params);
  }

  async uploadFile() {
    this.router.navigateToRoute("upload-file");
  }
}
