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

  searchNames(
    search: string,
    limit: number,
    callback: (result: string[]) => void
  ): void {
    this.apiHttp.get(
      this.uri + `SearchNames?search=${search}&limit=${limit}`,
      callback
    );
  }

  searchSymbols(
    search: string,
    limit: number,
    callback: (result: string[]) => void
  ): void {
    this.apiHttp.get(
      this.uri + `SearchSymbols?search=${search}&limit=${limit}`,
      callback
    );
  }

  searchQuoteTypes(
    search: string,
    limit: number,
    callback: (result: string[]) => void
  ): void {
    this.apiHttp.get(
      this.uri + `SearchQuoteTypes?search=${search}&limit=${limit}`,
      callback
    );
  }

  searchSectors(
    search: string,
    limit: number,
    callback: (result: string[]) => void
  ): void {
    this.apiHttp.get(
      this.uri + `SearchSectors?search=${search}&limit=${limit}`,
      callback
    );
  }

  searchIndustries(
    search: string,
    limit: number,
    callback: (result: string[]) => void
  ): void {
    this.apiHttp.get(
      this.uri + `SearchIndustries?search=${search}&limit=${limit}`,
      callback
    );
  }

  getTickerDetails(
    filter: IPaginateGenericFilter<ITickerFilter>,
    callback: (result: IPaginateGeneric<ITicker>) => void
  ): void {
    this.apiHttp.post(this.uri + 'GetTickerDetails', filter, callback);
  }

  getTickerById(tickerId: number, callback: (result: ITicker) => void): void {
    this.apiHttp.get(this.uri + `GetTickerById?tickerId=${tickerId}`, callback);
  }

  getTickerBySymbol(
    symbol: string,
    callback: (result: ITicker | null) => void
  ): void {
    this.apiHttp.get(this.uri + `GetTickerBySymbol?symbol=${symbol}`, callback);
  }

  updateTickerInfoData(
    tickerId: number,
    callback: (result: boolean) => void
  ): void {
    this.apiHttp.get(
      this.uri + `UpdateTickerInfoData?tickerId=${tickerId}`,
      callback
    );
  }
}
