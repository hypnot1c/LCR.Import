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

    async uploadFile() {
        this.router.navigateToRoute("upload-file");
    }
}
