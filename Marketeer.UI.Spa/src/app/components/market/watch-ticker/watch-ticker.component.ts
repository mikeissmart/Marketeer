import { Component, Input, OnInit } from '@angular/core';
import { IWatchTickerChange } from 'src/app/models/model';
import { WatchApiService } from 'src/app/services/api/watch-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-watch-ticker',
  templateUrl: './watch-ticker.component.html',
  styleUrls: ['./watch-ticker.component.scss'],
})
export class WatchTickerComponent implements OnInit {
  @Input() set tickerId(value: number | null | undefined) {
    if (value == undefined || value == null) {
      this.currentTickerId = null;
    } else if (this.currentTickerId != value) {
      this.currentTickerId = value;
    }
  }

  currentTickerId: number | null = null;
  watcher: IWatchTickerChange | null = null;

  constructor(
    private readonly watcherApi: WatchApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.getWatcher();
  }

  getWatcher(): void {
    this.watcherApi.getWatchTickerUpdateDaily(
      this.currentTickerId!,
      (result) => {
        this.watcher = {} as IWatchTickerChange;
        this.watcher.tickerIds = [];
        this.watcher.tickerIds.push(this.currentTickerId!);
        this.watcher.updateHistoryData = result!.updateHistoryData;
        this.watcher.updateNewsArticles = result!.updateNewsArticles;
      }
    );
  }

  updateWatcher(): void {
    this.watcherApi.updateWatchTickerUpdateDaily(this.watcher!, (result) => {
      if (
        result.addedCount > 0 ||
        result.updatedCount > 0 ||
        result.removedCount > 0
      ) {
        this.toaster.showSuccess('Update Success');
      } else if (result.currentCount == result.maxCount) {
        this.toaster.showError(
          `Unable to add any more, ${result.currentCount} of ${result.maxCount}`
        );
      } else {
        this.toaster.showWarning('No changes made');
      }
      this.getWatcher();
    });
  }
}
