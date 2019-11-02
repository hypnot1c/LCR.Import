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
    uploadStatus: string;

    async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
        this.switchList = await this.dataService.switch.getList();
    }

    async uploadFile() {
        this.uploadStatus = "Загрузка файла..."
        var formData = new FormData();
        for (var i = 0; i < this.files.length; i++) {
            var file = this.files[i];
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
        }
    }

    private getProgressMessage(loaded?, total?) {
        let percentage = (loaded && total && Math.floor((loaded * 100) / total)) || 0;
        return `Loaging file... ${percentage}%`;
    };
}
