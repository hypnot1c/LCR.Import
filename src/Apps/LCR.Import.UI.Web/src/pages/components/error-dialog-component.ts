import { autoinject, bindable } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
import { BaseComponent } from "shared/components";
import UIkit from "uikit";

@autoinject
export class ErrorDialogComponent extends BaseComponent {
  constructor(private ea: EventAggregator) {
    super("ErrorDialogComponent");
  }

  @bindable formatFlags: number;
  private isAttached: boolean;

  attached() {
    const el = document.getElementById('error-dialog');

    if (!this.isAttached) {
      this.isAttached = true;
      UIkit.util.on("#error-dialog", "hidden", () => {
        this.ea.publish("dialog:error:finish");
      });
    }

    UIkit.modal(el).show();
  }

  private onFinish() {
    const el = document.getElementById('error-dialog');
    UIkit.modal(el).hide();
  }
}
