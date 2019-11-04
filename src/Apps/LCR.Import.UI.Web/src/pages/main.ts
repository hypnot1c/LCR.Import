import { autoinject } from "aurelia-framework";
import { Router, RouteConfig, NavigationInstruction } from "aurelia-router";

import { BasePageComponent } from "shared/components";
import { DataService } from "services";

@autoinject
export class MainPage extends BasePageComponent {
    constructor(
        private dataService: DataService,
        private router: Router
    ) {
        super("MainPage");
        this.isLoadInProggress = true;
        this.uploadHistory = [];
    }

    uploadHistory: any[];
    isLoadInProggress: boolean;
    paginationData: { currentPageNumber: number, totalPages: number }

    async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction)
        : Promise<any> {
        const resp = await this.dataService.import.getHistory({ page: 1, pageSize: 10 });
        if (resp.status != "Ok") {
            this.Logger.error("Error fetching upload history");
            return;
        }
        this.uploadHistory = resp.result.data;
        this.paginationData = { currentPageNumber: 1, totalPages: resp.result.totalPages };
        this.isLoadInProggress = false;
    }

    async uploadFile() {
        this.router.navigateToRoute("upload-file");
    }
}
