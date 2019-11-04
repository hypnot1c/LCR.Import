import { autoinject } from "aurelia-framework";
import { RouteConfig, NavigationInstruction, Router } from "aurelia-router";

import { BasePageComponent } from "shared/components";
import { DataService } from "services";

@autoinject
export class UploadFilePage extends BasePageComponent {
    constructor(
        private dataService: DataService
    ) {
        super("UploadFilePage");
    }

    files: FileList;

    switchList: any[];
    selectedSwitch: any;

    statusCheckInterval: number;
    uploadStep: number;
    uploadStatus: string;
    uploadResultData: any[];
    isLoadInProggress: boolean;
    paginationData: { currentPageNumber: number, totalPages: number }

    fUpload: HTMLInputElement;

    async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
        this.switchList = await this.dataService.switch.getList();
    }

    async uploadFile() {
        const files = this.files || this.fUpload.files;
        this.uploadStatus = "Загрузка файла..."
        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            formData.append('file', file, file.name);
        }

        formData.append("userId", "1");
        const resp = await this.dataService.file.upload(formData, ({ loaded, total }) => {
            this.Logger.info(this.getProgressMessage(loaded, total));
        });

        if (resp.status == "Ok") {
            this.Logger.info("File loaded successfuly");
            this.uploadStatus = "Файл загружен. Обработка";
            this.statusCheckInterval = window.setInterval(() => this.statusCheck(resp.historyId), 2000);
        }
    }

    private async statusCheck(historyId: number) {
        const res = await this.dataService.import.checkStatus(historyId, 1);
        if (res.historyStatus == 1) {
            window.clearInterval(this.statusCheckInterval);
            this.statusCheckInterval = null;

            this.uploadStatus = "Файл обработан.";
            this.uploadStep = res.historyStatus;
            this.isLoadInProggress = true;
            const resp = await this.dataService.import.getResult(historyId, 1, { page: 1, pageSize: 10 });

            this.paginationData = { currentPageNumber: 1, totalPages: resp.result.totalPages };
            this.uploadResultData = resp.result.data;
            this.isLoadInProggress = false;
        }
    }

    private getProgressMessage(loaded?, total?) {
        let percentage = (loaded && total && Math.floor((loaded * 100) / total)) || 0;
        return `Loaging file... ${percentage}%`;
    };
}
