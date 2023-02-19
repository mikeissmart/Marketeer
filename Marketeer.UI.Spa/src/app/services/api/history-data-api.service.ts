import { Injectable } from '@angular/core';
import { ITickerHistorySummary } from 'src/app/models/model';
import { ApiHttpService } from '../http/api-http/api-http.service';

@Injectable({
  providedIn: 'root',
})
export class HistoryDataApiService {
  uri = 'HistoryData/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  getTickerHistorySummary(
    tickerId: number,
    callback: (result: ITickerHistorySummary) => void
  ): void {
    this.apiHttp.get(
      this.uri + `GetTickerHistorySummary?tickerId=${tickerId}`,
      callback
    );
  }
}
