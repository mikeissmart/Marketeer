import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IPaginate, IPaginateFilter } from 'src/app/models/model';
import { TableHeader } from 'src/app/models/view-model';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent implements OnInit {
  @Input()
  paginateFilter: IPaginateFilter | null = null;
  @Input()
  paginate: IPaginate | null = null;
  @Input()
  tableHeaders = [] as TableHeader[];

  @Output()
  refetchData = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  sortIcon(header: TableHeader): string {
    if (this.paginateFilter == null) {
      return '';
    }

    if (this.paginateFilter.orderBy == header.orderBy) {
      return this.paginateFilter.isOrderAsc
        ? 'fa-solid fa-sort-up'
        : 'fa-solid fa-sort-down';
    }

    return '';
  }

  onOrderBy(header: TableHeader): void {
    if (this.paginateFilter == null || header.orderBy == undefined) {
      return;
    }

    if (this.paginateFilter.orderBy == header.orderBy) {
      this.paginateFilter.isOrderAsc = !this.paginateFilter.isOrderAsc;
    } else {
      this.paginateFilter.isOrderAsc = true;
    }
    this.paginateFilter.orderBy = header.orderBy;

    this.refetchData.emit();
  }

  changePage(page: number): void {
    if (this.paginateFilter != null) {
      this.paginateFilter.pageIndex = page;
      this.refetchData.emit();
    }
  }

  changeItemCount(itemCount: number): void {
    if (this.paginateFilter != null) {
      this.paginateFilter.pageItemCount = itemCount;
      this.refetchData.emit();
    }
  }
}
