import { Injectable } from '@angular/core';
import { ApiHttpService } from '../http/api-http/api-http.service';
import {
  IWatchTicker,
  IWatchTickerChange,
  IWatchTickerResult,
} from 'src/app/models/model';

@Injectable({
  providedIn: 'root',
})
export class WatchApiService {
  uri = 'watch/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  getWatchTickerUpdateDaily(
    tickerId: number,
    callback: (result: IWatchTicker | null) => void
  ): void {
    this.apiHttp.get(
      this.uri + `GetWatchTickerUpdateDaily?tickerId=${tickerId}`,
      callback
    );
  }

  updateWatchTickerUpdateDaily(
    watcher: IWatchTickerChange,
    callback: (result: IWatchTickerResult) => void
  ): void {
    this.apiHttp.post(
      this.uri + `UpdateWatchTickerUpdateDaily`,
      watcher,
      callback
    );
  }
}
