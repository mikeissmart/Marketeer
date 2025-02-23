import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ModelHelper } from 'src/app/models/model-helper';
import {
  IAppLog,
  IAppLogFilter,
  IPaginateGenericFilter,
} from 'src/app/models/model';
import { LogApiService } from 'src/app/services/api/log-api.service';
import { LogLevelEnum } from 'src/app/models/model.enum';
import { TableHeader } from 'src/app/models/view-model';

@Component({
  selector: 'app-app-log-details',
  templateUrl: './app-log-details.component.html',
  styleUrls: ['./app-log-details.component.scss'],
})
export class AppLogDetailsComponent implements OnInit {
  logs = ModelHelper.IPaginateGenericDefault<IAppLog>();
  paginateFilter =
    ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<IAppLogFilter>;
  logLevelEnum = LogLevelEnum;
  eventIds = [] as number[];
  viewLog: IAppLog | null = null;

  constructor(private readonly logApi: LogApiService) {}

  ngOnInit(): void {
    this.logApi.getAppLogEventIds((x) => (this.eventIds = x));
    this.paginateFilter.orderBy = 'CreatedDateTime';
    this.paginateFilter.isOrderAsc = false;
    this.fetchLogs();
  }

  setLogLevel(value: LogLevelEnum | null): void {
    if (value == null) {
      this.paginateFilter.filter.logLevel = undefined;
    } else {
      this.paginateFilter.filter.logLevel = value as LogLevelEnum;
    }
  }

  clear(): void {
    this.paginateFilter.filter = {};
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader(''),
      new TableHeader('Log Level', 'LogLevel'),
      new TableHeader('Event Name', 'EventName'),
      new TableHeader('Source', 'Source'),
      new TableHeader('Date', 'CreatedDateTime'),
    ];
  }

  onViewLog(log: IAppLog): void {
    this.viewLog = log;
  }

  onViewLogClose(): void {
    this.viewLog = null;
  }

  fetchLogs(): void {
    this.logApi.getAppLogs(this.paginateFilter, (x) => (this.logs = x));
  }
}
