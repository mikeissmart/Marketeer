import { Injectable } from '@angular/core';
import {
  IPaginateGenericFilter,
  ITickerFilter,
  IPaginateGeneric,
  ITicker,
} from 'src/app/models/model';
import { ApiHttpService } from '../http/api-http/api-http.service';

@Injectable({
  providedIn: 'root',
})
export class TickerApiService {
  uri = 'ticker/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  searchSymbol(
    search: string,
    limit: number,
    callback: (result: string[]) => void
  ): void {
    this.apiHttp.get(
      this.uri + `SearchSymbol?search=${search}&limit=${limit}`,
      callback
    );
  }

  getTickers(
    filter: IPaginateGenericFilter<ITickerFilter>,
    callback: (result: IPaginateGeneric<ITicker>) => void
  ): void {
    this.apiHttp.post(this.uri + 'GetTickers', filter, callback);
  }
}
