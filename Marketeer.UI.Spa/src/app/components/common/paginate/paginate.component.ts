import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IPaginate } from 'src/app/models/model';

@Component({
  selector: 'app-paginate',
  templateUrl: './paginate.component.html',
  styleUrls: ['./paginate.component.scss'],
})
export class PaginateComponent implements OnInit {
  _paginate: any | null = null;
  @Input() set paginate(value: IPaginate | null) {
    if (value == null) {
      this.pageIndex = 1;
      this.pageItemCount = 1;
    } else {
      this.pageIndex = value.pageIndex + 1;
      this.pageItemCount = value.pageItemCount;
    }
    this._paginate = value;
  }

  @Output()
  changePage: EventEmitter<number> = new EventEmitter<number>();
  @Output()
  changeItemCount: EventEmitter<number> = new EventEmitter<number>();

  disableChange = false;
  pageIndex = 0;
  pageItemCount = 0;

  constructor() {}

  ngOnInit(): void {}

  onChangePage(): void {
    if (
      this.pageIndex == null ||
      Number.isNaN(this.pageIndex) ||
      this.pageIndex <= 0
    ) {
      this.pageIndex = 0;
    } else if (this.pageIndex >= this._paginate.totalPages) {
      this.pageIndex = this._paginate.totalPages - 1;
    } else {
      this.pageIndex--;
    }

    this.changePage.emit(this.pageIndex);
  }

  onChangeItemCount(): void {
    debugger;
    if (
      this.pageItemCount == null ||
      Number.isNaN(this.pageItemCount) ||
      this.pageItemCount <= 0
    ) {
      this.pageItemCount = 1;
    }

    this.changeItemCount.emit(this.pageItemCount);
  }
}
