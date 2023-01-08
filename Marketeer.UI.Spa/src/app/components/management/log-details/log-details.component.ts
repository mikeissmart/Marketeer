import { Component, OnInit } from '@angular/core';
import { LogTypeEnum } from 'src/app/models/enum';
import { ModelHelper } from 'src/app/models/model-helper';
import { IAppLog, ICronLog, IPythonLog } from 'src/app/models/model';
import { LogApiService } from 'src/app/services/api/log-api.service';

@Component({
  selector: 'app-log-details',
  templateUrl: './log-details.component.html',
  styleUrls: ['./log-details.component.scss'],
})
export class LogDetailsComponent implements OnInit {
  logTypeEnum = LogTypeEnum;
  currentLogType = 0;

  appPaginateFilter = ModelHelper.IPaginateFilterDefault();
  cronPaginateFilter = ModelHelper.IPaginateFilterDefault();
  pythonPaginateFilter = ModelHelper.IPaginateFilterDefault();
  appLogs = ModelHelper.IPaginateGenericDefault<IAppLog>();
  cronLogs = ModelHelper.IPaginateGenericDefault<ICronLog>();
  pythonLogs = ModelHelper.IPaginateGenericDefault<IPythonLog>();

  constructor(private readonly logApi: LogApiService) {}

  ngOnInit(): void {}

  logTypeChange(value: number): void {
    if (this.currentLogType == value) return;
    this.currentLogType = value as LogTypeEnum;
  }
}
