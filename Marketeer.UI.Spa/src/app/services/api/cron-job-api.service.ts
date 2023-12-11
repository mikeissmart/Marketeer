import { Injectable } from '@angular/core';
import { ApiHttpService } from '../http/api-http/api-http.service';
import { ICronJobDetail, IPaginateFilter, IPaginateGeneric } from 'src/app/models/model';

@Injectable({
  providedIn: 'root',
})
export class CronJobApiService {
  uri = 'CronJob/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  getCronJob(
    filter: IPaginateFilter,
    callback: (result: IPaginateGeneric<ICronJobDetail>) => void): void {
    this.apiHttp.post(this.uri + `GetCronJob`, filter, callback);
  }

  fireCronJob(cronJobName: string, callback: (result: boolean) => void): void {
    this.apiHttp.get(
      this.uri + `FireCronJob?cronJobName=${cronJobName}`,
      callback
    );
  }
}
