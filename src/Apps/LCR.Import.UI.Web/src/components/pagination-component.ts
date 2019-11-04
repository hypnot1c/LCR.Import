import { customElement, bindable } from "aurelia-framework";

import { BaseComponent } from "shared/components";

@customElement('pagination-component')
export class PaginantionComponent extends BaseComponent {
    constructor() {
        super("PaginantionComponent");
    }

    @bindable pageData: { currentPageNumber: number, totalPages: number }
    pageMap: any[];

    bind() {
        this.pageMap = [];
        if (this.pageData.totalPages <= 10) {
            for (let i = 1; i <= this.pageData.totalPages; i++) {
                this.pageMap.push(i);
            }
        }
        else {
            let leftBound = this.pageData.currentPageNumber - 3;
            let rightBound = this.pageData.currentPageNumber + 3;

            if (leftBound > 0) {
                this.pageMap.push(1);
                if ((leftBound - 1) > 1) {
                    this.pageMap.push('...');
                }
            }

            leftBound = leftBound <= 0 ? 1 : leftBound;
            for (let i = leftBound; i < this.pageData.currentPageNumber; i++) {
                this.pageMap.push(i);
            }

            if (rightBound < this.pageData.totalPages) {
                for (let i = this.pageData.currentPageNumber; i <= rightBound; i++) {
                    this.pageMap.push(i);
                }
                if ((this.pageData.totalPages - rightBound) > 1) {
                    this.pageMap.push('...');
                }
                this.pageMap.push(this.pageData.totalPages);
            }
            else {
                for (let i = this.pageData.currentPageNumber; i <= this.pageData.totalPages; i++) {
                    this.pageMap.push(i);
                }
            }
        }
        this.Logger.info("", this.pageData);
    }
}
