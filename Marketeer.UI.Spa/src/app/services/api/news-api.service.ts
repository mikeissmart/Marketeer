import { Injectable } from '@angular/core';
import { ApiHttpService } from '../http/api-http/api-http.service';
import {
  INewsArticle,
  INewsFilter,
  IPaginateGeneric,
  IPaginateGenericFilter,
} from 'src/app/models/model';

@Injectable({
  providedIn: 'root',
})
export class NewsApiService {
  uri = 'news/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  getTickerNews(
    tickerId: number,
    filter: IPaginateGenericFilter<INewsFilter>,
    callback: (result: IPaginateGeneric<INewsArticle>) => void
  ): void {
    this.apiHttp.post(
      this.uri + `GetTickerNews?tickerId=${tickerId}`,
      filter,
      callback
    );
  }

  updateTickerNews(
    tickerId: number,
    callback: (result: boolean) => void
  ): void {
    this.apiHttp.get(
      this.uri + `UpdateTickerNews?tickerId=${tickerId}`,
      callback
    );
  }

  getFinanceNews(
    filter: IPaginateGenericFilter<INewsFilter>,
    callback: (result: IPaginateGeneric<INewsArticle>) => void
  ): void {
    this.apiHttp.post(this.uri + `GetFinanceNews`, filter, callback);
  }

  updateFinanceNews(callback: (result: boolean) => void): void {
    this.apiHttp.get(this.uri + `UpdateFinanceNews`, callback);
  }
}
