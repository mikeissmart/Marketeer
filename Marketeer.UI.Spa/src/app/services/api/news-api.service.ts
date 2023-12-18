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
    filter: IPaginateGenericFilter<INewsFilter>,
    callback: (result: IPaginateGeneric<INewsArticle>) => void
  ): void {
    this.apiHttp.post(this.uri + `GetNews`, filter, callback);
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
}
