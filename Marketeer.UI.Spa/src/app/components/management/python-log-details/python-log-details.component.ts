import { Component, OnInit } from '@angular/core';
import {
  IPaginateGenericFilter,
  IPythonLog,
  IPythonLogFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
import { LogApiService } from 'src/app/services/api/log-api.service';

@Component({
  selector: 'app-python-log-details',
  templateUrl: './python-log-details.component.html',
  styleUrls: ['./python-log-details.component.scss'],
})
export class PythonLogDetailsComponent implements OnInit {
  logs = ModelHelper.IPaginateGenericDefault<IPythonLog>();
  paginateFilter =
    ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<IPythonLogFilter>;
  files = [] as string[];
  viewLog: IPythonLog | null = null;

  constructor(private readonly logApi: LogApiService) {}

  ngOnInit(): void {
    this.logApi.getPythonFiles((x) => (this.files = x));
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
      new TableHeader('Has Output'),
      new TableHeader('Has Error'),
      new TableHeader('Date', 'StartDate'),
      new TableHeader('Run Time'),
      new TableHeader(''),
    ];
  }

  onViewLog(log: IPythonLog): void {
    this.viewLog = log;
  }

  onViewLogClose(): void {
    this.viewLog = null;
  }

  fetchLogs(): void {
    this.logApi.getPythonLogs(this.paginateFilter, (x) => (this.logs = x));
  }

  calculateRunTime(log: IPythonLog): string {
    const milliSeconds = log.endDate.getTime() - log.startDate.getTime();

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
