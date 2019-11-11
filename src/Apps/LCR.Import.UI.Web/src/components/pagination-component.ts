import { customElement, bindable, autoinject } from "aurelia-framework";
import { RouteConfig, Router } from "aurelia-router";
import cloneDeep from "clone-deep";

import { BaseComponent } from "shared/components";

@autoinject
@customElement('pagination-component')
export class PaginantionComponent extends BaseComponent {
  constructor(
    private router: Router
  ) {
    super("PaginantionComponent");
  }

  @bindable currentPage: number;
  @bindable totalPages: number;
  @bindable routeConfig: RouteConfig;
  @bindable routeParams: any;
  routeName: string;
  prevPageUrl: any;
  nextPageUrl: any;
  pageMap: any[];

  currentPageChanged() {
    this.bind();
  }

  totalPagesChanged() {
    this.bind();
  }

  bind() {
    this.Logger.info("bind");
    if (!!this.routeParams) {
      this.routeName = this.routeConfig.name;
      if (this.currentPage > 1) {
        const prevPageParams = cloneDeep(this.routeParams);
        prevPageParams.page = this.currentPage - 1;
        this.prevPageUrl = this.router.generate(this.routeName, prevPageParams);
      }

      if (this.currentPage < this.totalPages) {
        const nextPageParams = cloneDeep(this.routeParams);
        nextPageParams.page = this.currentPage + 1;
        this.nextPageUrl = this.router.generate(this.routeName, nextPageParams);
      }

      this.pageMap = [];
      if (this.totalPages <= 10) {
        for (let i = 1; i <= this.totalPages; i++) {
          this.pageMap.push({ number: i });
        }
      }
      else {
        let leftBound = this.currentPage - 3;
        let rightBound = this.currentPage + 3;

        if (leftBound > 0) {
          if (leftBound != 1) {
            this.pageMap.push({ number: 1 });
          }
          if ((leftBound - 1) > 1) {
            this.pageMap.push('...');
          }
        }

        leftBound = leftBound <= 0 ? 1 : leftBound;
        for (let i = leftBound; i < this.currentPage; i++) {
          this.pageMap.push({ number: i });
        }

        if (rightBound < this.totalPages) {
          for (let i = this.currentPage; i <= rightBound; i++) {
            this.pageMap.push({ number: i });
          }
          if ((this.totalPages - rightBound) > 1) {
            this.pageMap.push('...');
          }
          if (rightBound != this.totalPages) {
            this.pageMap.push({ number: this.totalPages });
          }
        }
        else {
          for (let i = this.currentPage; i <= this.totalPages; i++) {
            this.pageMap.push({ number: i });
          }
        }
      }
      for (let i = 0; i < this.pageMap.length; i++) {
        if (this.pageMap[i] != "...") {
          let params = cloneDeep(this.routeParams);
          params.page = this.pageMap[i].number;
          this.pageMap[i].params = params
        }
      }
    }
  }
}
