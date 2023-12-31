import { Component, OnInit } from '@angular/core';
import {
  ICronLog,
  ICronLogFilter,
  IPaginateGenericFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { CronLogTypeEnum } from 'src/app/models/model.enum';
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
  cronLogTypeEnum = CronLogTypeEnum;

  constructor(private readonly logApi: LogApiService) {}

  ngOnInit(): void {
    this.logApi.getCronNames((result) => (this.cronNames = result));
    this.paginateFilter.orderBy = 'StartDateTime';
    this.paginateFilter.isOrderAsc = false;
    this.fetchLogs();
  }

  clear(): void {
    this.paginateFilter.filter = {};
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader(''),
      new TableHeader('Cron Type'),
      new TableHeader('Name', 'Name'),
      new TableHeader('Has Ouput'),
      new TableHeader('Is Canceled'),
      new TableHeader('Date', 'StartDateTime'),
      new TableHeader('Run Time'),
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
    const a = typeof log.endDateTime;
    const milliSeconds =
      log.endDateTime.getTime() - log.startDateTime.getTime();

    const hours = Math.floor(milliSeconds / 1000 / 60 / 60);
    const minutes = Math.floor(milliSeconds / 1000 / 60 - hours * 60);
    const seconds = Math.floor(
      milliSeconds / 1000 - minutes * 60 - hours * 60 * 60
    );

    return `${hours.toString().padStart(2, '0')}:${minutes
      .toString()
      .padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
  }
}
