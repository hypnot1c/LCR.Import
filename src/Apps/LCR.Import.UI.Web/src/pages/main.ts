import { autoinject } from "aurelia-framework";
import { Router, RouteConfig, NavigationInstruction, activationStrategy } from "aurelia-router";

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

  uploadHistory: any[];
  isLoadInProggress: boolean;
  paginationData: { currentPageNumber: number, totalPages: number }
  currentRouteConfig: RouteConfig;
  currentRouteParams: any;

  determineActivationStrategy() {
    return activationStrategy.replace;
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
    this.dataService.import.getHistory({ page: params.page, pageSize: 10 })
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

  async uploadFile() {
    this.router.navigateToRoute("upload-file");
  }
}
