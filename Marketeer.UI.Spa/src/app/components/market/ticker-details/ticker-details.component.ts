import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ITicker, ITickerHistorySummary } from 'src/app/models/model';
import { DelistEnum } from 'src/app/models/model.enum';
import { TableHeader } from 'src/app/models/view-model';
import { HistoryDataApiService } from 'src/app/services/api/history-data-api.service';
import { TickerApiService } from 'src/app/services/api/ticker-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-ticker-details',
  templateUrl: './ticker-details.component.html',
  styleUrls: ['./ticker-details.component.scss'],
})
export class TickerDetailsComponent implements OnInit {
  tickerDetail: ITicker | null = null;
  tickerHistorySummary: ITickerHistorySummary | null = null;

  constructor(
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly historyDataApi: HistoryDataApiService,
    private readonly tickerApi: TickerApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    const symbol = this.activatedRoute.snapshot.paramMap.get('symbol');
    if (symbol == null || symbol.length == 0) {
      this.router.navigateByUrl('/tickers', {
        skipLocationChange: false,
      });
    } else {
      this.tickerApi.getTickerBySymbol(symbol, (result) => {
        if (result == null) {
          this.router.navigateByUrl('/tickers', {
            skipLocationChange: false,
          });
        } else {
          this.tickerDetail = result!;
        }
      });
    }
  }

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

  fetchInfo(): void {
    this.tickerApi.updateTickerInfoData(this.tickerDetail!.id, (result) => {
      if (result) {
        this.toaster.showSuccess('New Info Data');
        this.tickerApi.getTickerById(
          this.tickerDetail!.id,
          (result) => (this.tickerDetail = result)
        );
      } else {
        this.toaster.showWarning('Info Data up to date');
      }
    });
  }

  fetchHistData(): void {
    this.historyDataApi.updateTickerHistoryData(
      this.tickerDetail!.id,
      (result) => {
        if (result) {
          this.toaster.showSuccess('New History Data');
          this.tickerApi.getTickerById(
            this.tickerDetail!.id,
            (result) => (this.tickerDetail = result)
          );
        } else {
          this.toaster.showWarning('History Data up to date');
        }
      }
    );
  }
}
