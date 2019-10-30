import { autoinject } from "aurelia-framework";
import { Router } from "aurelia-router";

import { BasePageComponent } from "shared/components";
import { DataService } from "services";

@autoinject
export class MainPage extends BasePageComponent {
    constructor(
        private dataService: DataService,
        private router: Router
    ) {
        super("MainPage");
    }

    files: FileList;

    async uploadFile() {
        var formData = new FormData();
        for (var i = 0; i < this.files.length; i++) {
            var file = this.files[i];
            formData.append('file', file, file.name);
        }

        const resp = await this.dataService.file.upload(1, formData, ({ loaded, total }) => {
            this.Logger.info(this.getProgressMessage(loaded, total));
        });

        if (resp.status == "Ok") {
            this.Logger.info("File loaded successfuly");

            this.router.navigateToRoute("proccess-file")
        }
    }

    private getProgressMessage(loaded?, total?) {
        let percentage = (loaded && total && Math.floor((loaded * 100) / total)) || 0;
        return `Loaging file... ${percentage}%`;
    };
}
