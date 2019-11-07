import { customAttribute, autoinject, dynamicOptions, bindable } from 'aurelia-framework';
import * as Pikaday from "pikaday";

import { BaseObject } from 'shared/abstractions';

@customAttribute('datepicker')
@autoinject
export class DatePickerCustomAttribute extends BaseObject {
  constructor(private element: Element) {
    super("DatePickerCustomAttribute");
  }
  private pikaday: any;

  @bindable format: string;

  attached() {
    this.pikaday = new Pikaday({
      field: this.element,
      format: this.format
    });
  }

  detached() {
    // remove it in here if possible!
    //(this.element as any).value = null;
    this.pikaday.destroy();
    this.Logger.info('Detached', (this.element as any).value);
  }
}
