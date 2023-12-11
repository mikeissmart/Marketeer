import { Component, Input, OnInit } from '@angular/core';
import { ITickerHistorySummary } from 'src/app/models/model';
import { TableHeader } from 'src/app/models/view-model';

@Component({
  selector: 'app-ticker-history-summary',
  templateUrl: './ticker-history-summary.component.html',
  styleUrls: ['./ticker-history-summary.component.scss'],
})
export class TickerHistorySummaryComponent implements OnInit {
  @Input() set summary(value: ITickerHistorySummary | null | undefined) {
    if (value == undefined) {
      this.historySummary = null;
    } else {
      this.historySummary = value;
    }
  }

  historySummary: ITickerHistorySummary | null = null;

  constructor() {}

  ngOnInit(): void {}

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader('Title'),
      new TableHeader('Date'),
      new TableHeader('Close'),
    ];
  }
}
