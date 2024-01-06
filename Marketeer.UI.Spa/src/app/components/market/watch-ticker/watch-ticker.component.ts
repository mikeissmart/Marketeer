import { Component, Input, OnInit } from '@angular/core';
import { IWatchTickerChange, IWatchUserStatus } from 'src/app/models/model';
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
  watchTicker: IWatchTickerChange | null = null;
  watchStatus = {} as IWatchUserStatus;

  constructor(
    private readonly watchApi: WatchApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.getWatcher();
    this.getWatchStatus();
  }

  getWatcher(): void {
    this.watchApi.getWatchTickerUpdateDaily(this.currentTickerId!, (result) => {
      this.watchTicker = {} as IWatchTickerChange;
      this.watchTicker.tickerIds = [];
      this.watchTicker.tickerIds.push(this.currentTickerId!);
      this.watchTicker.updateHistoryData = result!.updateHistoryData;
      this.watchTicker.updateNewsArticles = result!.updateNewsArticles;
    });
  }

  getWatchStatus(): void {
    this.watchApi.getWatcherUserStatus(
      (result) => (this.watchStatus = result!)
    );
  }

  updateWatcher(): void {
    this.watchApi.updateWatchTickerUpdateDaily(this.watchTicker!, (result) => {
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
      this.getWatchStatus();
    });
  }

  stopWatching(): void {
    this.watchTicker!.updateHistoryData = false;
    this.watchTicker!.updateNewsArticles = false;
    this.updateWatcher();
  }
}
