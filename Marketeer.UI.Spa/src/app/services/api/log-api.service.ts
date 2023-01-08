import { Injectable } from '@angular/core';
import {
  IPaginateFilter,
  IAppLog,
  ICronLog,
  IPythonLog,
  IPaginateGeneric,
  IAppLogFilter,
  ICronLogFilter,
  IPaginateGenericFilter,
} from 'src/app/models/model';
import { ApiHttpService } from '../http/api-http/api-http.service';

@Injectable({
  providedIn: 'root',
})
export class LogApiService {
  uri = 'logs/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  getAppLogEventIds(callback: (result: number[]) => void): void {
    this.apiHttp.get(this.uri + 'GetAppLogEventIds', callback);
  }

  getAppLogs(
    filter: IPaginateGenericFilter<IAppLogFilter>,
    callback: (result: IPaginateGeneric<IAppLog>) => void
  ): void {
    this.apiHttp.post(this.uri + 'GetAppLogs', filter, callback);
  }

  getCronNames(callback: (result: string[]) => void): void {
    this.apiHttp.get(this.uri + 'GetCronNames', callback);
  }

  getCronLogs(
    filter: IPaginateGenericFilter<ICronLogFilter>,
    callback: (result: IPaginateGeneric<ICronLog>) => void
  ): void {
    this.apiHttp.post(this.uri + 'GetCronLogs', filter, callback);
  }

  getPythonFiles(callback: (result: string[]) => void): void {
    this.apiHttp.get(this.uri + 'GetPythonFiles', callback);
  }

  getPythonLogs(
    filter: IPaginateFilter,
    callback: (result: IPaginateGeneric<IPythonLog>) => void
  ): void {
    this.apiHttp.post(this.uri + 'GetPythonLogs', filter, callback);
  }
}
