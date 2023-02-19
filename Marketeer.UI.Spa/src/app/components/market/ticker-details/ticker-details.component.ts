import { Component, Input, OnInit } from '@angular/core';
import { ITicker, ITickerHistorySummary } from 'src/app/models/model';
import { DelistEnum } from 'src/app/models/model.enum';
import { TableHeader } from 'src/app/models/view-model';
import { HistoryDataApiService } from 'src/app/services/api/history-data-api.service';

@Component({
  selector: 'app-ticker-details',
  templateUrl: './ticker-details.component.html',
  styleUrls: ['./ticker-details.component.scss'],
})
export class TickerDetailsComponent implements OnInit {
  @Input() set ticker(value: ITicker | null | undefined) {
    if (value == undefined) {
      this.tickerDetail = null;
    } else {
      this.tickerDetail = value;
    }
    this.getTickerHistorySummary();
  }

  tickerDetail: ITicker | null = null;
  tickerHistorySummary: ITickerHistorySummary | null = null;

  constructor(private historyDataApi: HistoryDataApiService) {}

  ngOnInit(): void {}

  getTickerHistorySummary(): void {
    if (this.tickerDetail == null) {
      this.tickerHistorySummary = null;
    } else {
      this.historyDataApi.getTickerHistorySummary(
        this.tickerDetail.id,
        (x) => (this.tickerHistorySummary = x)
      );
    }
  }

  getDelist(value: DelistEnum): string {
    return DelistEnum[value].replaceAll('_', ' ');
  }

  getTableHeaders(): TableHeader[] {
    return [new TableHeader('Delist Reasons')];
  }
}
