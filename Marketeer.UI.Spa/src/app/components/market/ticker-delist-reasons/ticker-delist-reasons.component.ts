import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ITickerDelistReason } from 'src/app/models/model';
import { DelistEnum } from 'src/app/models/model.enum';
import { TableHeader } from 'src/app/models/view-model';
import { TickerApiService } from 'src/app/services/api/ticker-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-ticker-delist-reasons',
  templateUrl: './ticker-delist-reasons.component.html',
  styleUrls: ['./ticker-delist-reasons.component.scss'],
})
export class TickerDelistReasonsComponent implements OnInit {
  @Input() set tickerId(value: number | null | undefined) {
    if (value == undefined || value == null) {
      this.currentTickerId = null;
    } else if (this.currentTickerId != value) {
      this.currentTickerId = value;
    }
  }
  @Input() set lastInfoUpdate(value: Date | null | undefined) {
    if (value == undefined || value == null) {
      this.currentLastInfoUpdate = null;
    } else if (this.currentLastInfoUpdate != value) {
      this.currentLastInfoUpdate = value;
    }
  }
  @Input() set delistReasons(value: ITickerDelistReason[] | null | undefined) {
    if (value == undefined) {
      this.currentDelistReasons = null;
    } else {
      this.currentDelistReasons = value;
    }
  }
  @Output() onUpdateInfo = new EventEmitter();

  currentTickerId: number | null = null;
  currentLastInfoUpdate: Date | null = null;
  currentDelistReasons: ITickerDelistReason[] | null = null;

  constructor(
    private readonly tickerApi: TickerApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {}

  getDelist(value: DelistEnum): string {
    return DelistEnum[value].replaceAll('_', ' ');
  }

  getTableHeaders(): TableHeader[] {
    return [new TableHeader('Delist Reasons')];
  }

  updateInfo(): void {
    this.tickerApi.updateTickerInfoData(this.currentTickerId!, (result) => {
      if (result) {
        this.toaster.showSuccess('New Info Data');
      } else {
        this.toaster.showWarning('Info Data up to date');
      }
      this.onUpdateInfo.emit();
    });
  }
}
