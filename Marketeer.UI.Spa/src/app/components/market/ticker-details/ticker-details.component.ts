import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ITicker } from 'src/app/models/model';
import { HistoryDataApiService } from 'src/app/services/api/history-data-api.service';
import { NewsApiService } from 'src/app/services/api/news-api.service';
import { TickerApiService } from 'src/app/services/api/ticker-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-ticker-details',
  templateUrl: './ticker-details.component.html',
  styleUrls: ['./ticker-details.component.scss'],
})
export class TickerDetailsComponent implements OnInit {
  tickerDetail: ITicker | null = null;

  constructor(
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly tickerApi: TickerApiService
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

  getTickerDetails(): void {
    this.tickerApi.getTickerBySymbol(this.tickerDetail!.symbol, (result) => {
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
