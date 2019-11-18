import { autoinject } from "aurelia-framework";
import { RouteConfig, NavigationInstruction, Router } from "aurelia-router";
import { ValidationRules, ValidationController, validateTrigger } from 'aurelia-validation';
import { Store } from "aurelia-store";
import cloneDeep from "clone-deep";

import { IAppState } from "config/state/abstractions";
import { BasePageComponent } from "shared/components";
import { DataService } from "services";
import * as importStateActions from "config/state/actions/import-state-actions";

@autoinject
export class UploadFilePage extends BasePageComponent {
  constructor(
    private dataService: DataService,
    private router: Router,
    private validationController: ValidationController,
    protected store: Store<IAppState>
  ) {
    super("UploadFilePage");

    this.selectedSwitch = null;
    this.isValid = false;

    ValidationRules
      .ensure((m: UploadFilePage) => m.selectedSwitch).required()
      .ensure((m: UploadFilePage) => m.files).satisfies((val: FileList) => val.length === 1)
      .on(this);

    this.validationController.validateTrigger = validateTrigger.manual;
  }

  state: IAppState;

  files: FileList;

  switchList: any[];
  selectedSwitch: number;
  selectedFileName: string;

  uploadStatus: string;

  fUpload: HTMLInputElement;

  isValid: boolean;

  async activate(params: any, routeConfig: RouteConfig, navigationInstruction: NavigationInstruction) {
    this.stateSubscriptions.push(
      this.store.state.subscribe((newState) => {
        this.state = cloneDeep(newState);
      })
    );

    const resp = await this.dataService.switch.getList();
    this.switchList = resp.result;
  }

  showFileName() {
    this.files = this.files || this.fUpload.files;
    for (var i = 0; i < this.files.length; i++) {
      var file = this.files[i];
      this.selectedFileName = file.name;
    }
  }

  clearFile() {
    this.files = null;
    this.fUpload.files = null;
    this.selectedFileName = null;
  }

  async uploadFile() {
    this.files = this.files || this.fUpload.files;

    const errors = await this.validationController.validate();
    this.isValid = errors.valid;
    if (!this.isValid) {
      return;
    }

    this.uploadStatus = "Загрузка файла..."
    var formData = new FormData();
    for (var i = 0; i < this.files.length; i++) {
      var file = this.files[i];
      formData.append('file', file, file.name);
    }

    formData.append("switchId", `${this.selectedSwitch}`);
    formData.append("userId", `${this.state.userId}`);
    const resp = await this.dataService.file.upload(formData, ({ loaded, total }) => {
      const percent = this.getLoadingPercentage(loaded, total);
      this.Logger.info(`Loading file...${percent}%`);
      this.uploadStatus = `Загрузка файла...${percent}%`;
    });

    if (resp.status == "Ok") {
      this.Logger.info("File loaded successfuly");
      this.uploadStatus = "Файл загружен";

      await this.store.dispatch(importStateActions.setCurrentHistoryId, resp.historyId);
      this.router.navigateToRoute("process-file");
    }
  }

  private getLoadingPercentage(loaded?, total?) {
    let percentage = (loaded && total && Math.floor((loaded * 100) / total)) || 0;
    return percentage;
  };
}
