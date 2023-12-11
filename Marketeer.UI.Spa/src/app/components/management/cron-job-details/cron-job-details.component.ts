import { Component, OnInit } from '@angular/core';
import { ICronJobDetail } from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
import { CronJobApiService } from 'src/app/services/api/cron-job-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-cron-job-details',
  templateUrl: './cron-job-details.component.html',
  styleUrls: ['./cron-job-details.component.scss'],
})
export class CronJobDetailsComponent implements OnInit {
  cronJobDetails = ModelHelper.IPaginateGenericDefault<ICronJobDetail>();
  paginateFilter =
    ModelHelper.IPaginateFilterDefault();

  constructor(
    private readonly cronJobApi: CronJobApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    this.cronJobApi.getCronJob(
      this.paginateFilter,
      (x) => {
      this.cronJobDetails = x;
    });
  }

  fire(item: ICronJobDetail): void {
    this.cronJobApi.fireCronJob(item.name, (x) => {
      if (x) {
        this.toaster.showSuccess('Started');
        this.refresh();
      } else {
        this.toaster.showSuccess('Unable to start');
      }
    });
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader(''),
      new TableHeader('Name', 'Name'),
      new TableHeader('Expression'),
      new TableHeader('Next', "NextOccurrence"),
      new TableHeader('Last', "LastOccurrence"),
      new TableHeader('IsRunning'),
    ];
  }
}
