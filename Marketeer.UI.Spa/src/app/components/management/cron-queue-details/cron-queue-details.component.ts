import { Component, OnInit } from '@angular/core';
import { ICronQueueDetail } from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
import { CronApiService } from 'src/app/services/api/cron-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-cron-queue-details',
  templateUrl: './cron-queue-details.component.html',
  styleUrls: ['./cron-queue-details.component.scss'],
})
export class CronQueueDetailsComponent implements OnInit {
  cronQueueDetails = ModelHelper.IPaginateGenericDefault<ICronQueueDetail>();
  paginateFilter = ModelHelper.IPaginateFilterDefault();

  constructor(
    private readonly cronApi: CronApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    this.cronApi.getCronQueues(this.paginateFilter, (x) => {
      this.cronQueueDetails = x;
    });
  }

  startOrStop(item: ICronQueueDetail): void {
    if (item.isRunning) {
      this.cronApi.stopCronQueue(item.name, (x) => {
        if (x) {
          this.toaster.showSuccess('Stopped');
          this.refresh();
        } else {
          this.toaster.showSuccess('Unable to stop');
        }
      });
    } else {
      this.cronApi.startCronQueue(item.name, (x) => {
        if (x) {
          this.toaster.showSuccess('Started');
          this.refresh();
        } else {
          this.toaster.showSuccess('Unable to start');
        }
      });
    }
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader('Start / Stop'),
      new TableHeader('Name', 'Name'),
      new TableHeader('Is Running'),
      new TableHeader('Is Pending Stop'),
    ];
  }
}
