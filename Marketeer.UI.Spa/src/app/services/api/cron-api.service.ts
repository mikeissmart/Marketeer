import { Injectable } from '@angular/core';
import { ApiHttpService } from '../http/api-http/api-http.service';
import {
  ICronJobDetail,
  ICronQueueDetail,
  IPaginateFilter,
  IPaginateGeneric,
} from 'src/app/models/model';

@Injectable({
  providedIn: 'root',
})
export class CronApiService {
  uri = 'Cron/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  getCronJobs(
    filter: IPaginateFilter,
    callback: (result: IPaginateGeneric<ICronJobDetail>) => void
  ): void {
    this.apiHttp.post(this.uri + `GetCronJobs`, filter, callback);
  }

  fireCronJob(name: string, callback: (result: boolean) => void): void {
    this.apiHttp.get(this.uri + `FireCronJob?name=${name}`, callback);
  }

  getCronQueues(
    filter: IPaginateFilter,
    callback: (result: IPaginateGeneric<ICronQueueDetail>) => void
  ): void {
    this.apiHttp.post(this.uri + `GetCronQueues`, filter, callback);
  }

  startCronQueue(name: string, callback: (result: boolean) => void): void {
    this.apiHttp.get(this.uri + `StartCronQueue?name=${name}`, callback);
  }

  stopCronQueue(name: string, callback: (result: boolean) => void): void {
    this.apiHttp.get(this.uri + `StopCronQueue?name=${name}`, callback);
  }
}
