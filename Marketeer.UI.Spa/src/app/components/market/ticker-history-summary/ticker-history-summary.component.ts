import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ITickerHistorySummary } from 'src/app/models/model';
import { TableHeader } from 'src/app/models/view-model';
import { HistoryDataApiService } from 'src/app/services/api/history-data-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-ticker-history-summary',
  templateUrl: './ticker-history-summary.component.html',
  styleUrls: ['./ticker-history-summary.component.scss'],
})
export class TickerHistorySummaryComponent implements OnInit {
  @Input() set tickerId(value: number | null | undefined) {
    if (value == undefined || value == null) {
      this.tickerId = null;
      this.historySummary = null;
    } else if (this.currentTickerId != value) {
      this.currentTickerId = value;
    }
  }
  @Input() set lastHistoryUpdate(value: Date | null | undefined) {
    if (value == undefined || value == null) {
      this.currentLastHistoryUpdate = null;
    } else if (this.currentLastHistoryUpdate != value) {
      this.currentLastHistoryUpdate = value;
    }
  }
  @Output() onUpdateHistoryData = new EventEmitter();

  currentTickerId: number | null = null;
  currentLastHistoryUpdate: Date | null = null;
  historySummary: ITickerHistorySummary | null = null;

  constructor(
    private readonly historyDataApi: HistoryDataApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    if (this.currentTickerId != null) {
      this.fetchHistData();
    }
  }

  fetchHistData(): void {
    this.historyDataApi.getTickerHistorySummary(
      this.currentTickerId!,
      (x) => (this.historySummary = x)
    );
  }

  updateHistData(): void {
    this.historyDataApi.updateTickerHistoryData(
      this.currentTickerId!,
      (result) => {
        if (result) {
          this.toaster.showSuccess('New History Data');
        } else {
          this.toaster.showWarning('History Data up to date');
        }
        this.fetchHistData();
        this.onUpdateHistoryData.emit();
      }
    );
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader('Title'),
      new TableHeader('Date'),
      new TableHeader('Value'),
      new TableHeader('Diff'),
    ];
  }

  getDifferenceColor(diff: number | undefined): string {
    if (diff === undefined) {
      return '';
    } else if (diff > 0) {
      return 'bg-success text-light';
    } else if (diff < 0) {
      return 'bg-danger text-light';
    } else if (diff == 0) {
      return 'bg-warning text-dark';
    } else {
      return '';
    }
  }
}
