import { Component, OnInit } from '@angular/core';
import {
  ICronLog,
  ICronLogFilter,
  IPaginateGenericFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
import { LogApiService } from 'src/app/services/api/log-api.service';

@Component({
  selector: 'app-cron-log-details',
  templateUrl: './cron-log-details.component.html',
  styleUrls: ['./cron-log-details.component.scss'],
})
export class CronLogDetailsComponent implements OnInit {
  logs = ModelHelper.IPaginateGenericDefault<ICronLog>();
  paginateFilter =
    ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<ICronLogFilter>;
  cronNames = [] as string[];
  viewLog: ICronLog | null = null;

  constructor(private readonly logApi: LogApiService) {}

  ngOnInit(): void {
    this.logApi.getCronNames((result) => (this.cronNames = result));
    this.paginateFilter.orderBy = 'StartDate';
    this.paginateFilter.isOrderAsc = false;
    this.fetchLogs();
  }

  clear(): void {
    this.paginateFilter.filter = {};
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader('Name', 'Name'),
      new TableHeader('Has Ouput'),
      new TableHeader('Has Error'),
      new TableHeader('Date', 'StartDate'),
      new TableHeader(''),
    ];
  }

  onViewLog(log: ICronLog): void {
    this.viewLog = log;
  }

  onViewLogClose(): void {
    this.viewLog = null;
  }

  fetchLogs(): void {
    this.logApi.getCronLogs(this.paginateFilter, (x) => (this.logs = x));
  }

  calculateRunTime(log: ICronLog): string {
    const milliSeconds = log.endDate.getTime() - log.startDate.getTime();

    const hours = Math.floor(milliSeconds / 1000 / 60 / 60);
    const minutes = Math.floor(milliSeconds / 1000 / 60 - hours * 60);
    const seconds = Math.floor(
      milliSeconds / 1000 - minutes * 60 - hours * 60 * 60
    );

    return `${hours}:${minutes}:${seconds}`;
  }
}
