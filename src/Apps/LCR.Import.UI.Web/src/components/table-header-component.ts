import { bindable } from "aurelia-framework";

import { BaseComponent } from "shared/components";
import sortAscIcon from "icons/sort_asc.png";
import sortDescIcon from "icons/sort_desc.png";

export class TableHeaderComponent extends BaseComponent {
  constructor() {
    super("TableHeaderComponent");

    this.showSortIcon = false;
  }

  @bindable sortDirection: "asc" | "desc"
  @bindable fieldName: string;
  @bindable sortFieldName: string;
  protected sortIcon: any;
  showSortIcon: boolean;

  fieldNameChanged() {
    this.bind();
  }

  sortFieldNameChanged() {
    this.bind();
  }

  sortDirectionChanged() {
    this.bind();
  }

  bind() {
    if (this.fieldName == this.sortFieldName) {
      this.sortIcon.src = this.sortDirection == "desc" ? sortDescIcon : sortAscIcon;
      this.showSortIcon = true;
    }
    else {
      this.showSortIcon = false;
    }
  }
}
