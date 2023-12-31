import { Component, OnInit } from '@angular/core';
import {
  IPaginateGenericFilter,
  ITicker,
  ITickerFilter,
  IWatchTickerDetailsChange,
  IWatchUserStatus,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
import { TickerApiService } from 'src/app/services/api/ticker-api.service';
import { WatchApiService } from 'src/app/services/api/watch-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-ticker-list',
  templateUrl: './ticker-list.component.html',
  styleUrls: ['./ticker-list.component.scss'],
})
export class TickerListComponent implements OnInit {
  searchNames = [] as string[];
  searchSymbols = [] as string[];
  searchQuotes = [] as string[];
  searchSectors = [] as string[];
  searchIndustries = [] as string[];
  tickers = ModelHelper.IPaginateGenericDefault<ITicker>();
  paginateFilter =
    ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<ITickerFilter>;
  viewTicker: ITicker | null = null;
  watchStatus = {} as IWatchUserStatus;
  isOpenWatchModal = false;
  watchTickerChange = {
    updateHistoryData: false,
    updateNewsArticles: false,
  } as IWatchTickerDetailsChange;

  constructor(
    private readonly tickerApi: TickerApiService,
    private readonly watchApi: WatchApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.clear();
    this.fetchTickers();
    this.fetchWatchStatus();
  }

  onSearchName(searchText: string): void {
    this.paginateFilter.filter.name = searchText;
    this.tickerApi.searchNames(searchText, 8, (x) => (this.searchNames = x));
  }

  onSelectedName(name: string | undefined): void {
    this.paginateFilter.filter.name = name;
  }

  onSearchSymbol(searchText: string): void {
    this.paginateFilter.filter.symbol = searchText;
    this.tickerApi.searchSymbols(
      searchText,
      8,
      (x) => (this.searchSymbols = x)
    );
  }

  onSelectedSymbol(symbol: string | undefined): void {
    this.paginateFilter.filter.symbol = symbol;
  }

  onSearchQuoteType(searchText: string): void {
    this.paginateFilter.filter.quoteType = searchText;
    this.tickerApi.searchQuoteTypes(
      searchText,
      8,
      (x) => (this.searchQuotes = x)
    );
  }

  onSelectedQuote(quote: string | undefined): void {
    this.paginateFilter.filter.quoteType = quote;
  }

  onSearchSector(searchText: string): void {
    this.paginateFilter.filter.sector = searchText;
    this.tickerApi.searchSectors(
      searchText,
      8,
      (x) => (this.searchSectors = x)
    );
  }

  onSelectedSector(sector: string | undefined): void {
    this.paginateFilter.filter.sector = sector;
  }

  onSearchIndustry(searchText: string): void {
    this.paginateFilter.filter.industry = searchText;
    this.tickerApi.searchIndustries(
      searchText,
      8,
      (x) => (this.searchIndustries = x)
    );
  }

  onSelectedIndustry(industry: string | undefined): void {
    this.paginateFilter.filter.industry = industry;
  }

  clear(): void {
    this.paginateFilter.filter = {};
    this.searchSymbols = [];
    this.searchQuotes = [];
    this.searchSectors = [];
    this.searchIndustries = [];

    this.onSearchName('');
    this.onSearchSymbol('');
    this.onSearchQuoteType('');
    this.onSearchSector('');
    this.onSearchIndustry('');
  }

  fetchTickers(): void {
    this.tickerApi.getTickerDetails(
      this.paginateFilter,
      (x) => (this.tickers = x)
    );
  }

  fetchWatchStatus(): void {
    this.watchApi.getWatcherUserStatus(
      (result) => (this.watchStatus = result!)
    );
  }

  openWatchModal(): void {
    this.watchTickerChange = {
      updateHistoryData: false,
      updateNewsArticles: false,
    } as IWatchTickerDetailsChange;
    this.isOpenWatchModal = true;
  }

  updateWatchTickers(): void {
    this.watchTickerChange.filter = this.paginateFilter;
    this.watchApi.appendWatchTickerDetails(this.watchTickerChange, (result) => {
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
      this.isOpenWatchModal = false;
      this.fetchWatchStatus();
    });
  }

  onViewTicker(ticker: ITicker): void {
    this.viewTicker = ticker;
  }

  onViewTickerClose(): void {
    this.viewTicker = null;
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader(''),
      new TableHeader('Name', 'Name'),
      new TableHeader('Symbol', 'Symbol'),
      new TableHeader('Quote Type', 'QuoteType'),
      new TableHeader('Sector', 'Sector'),
      new TableHeader('Industry', 'Industry'),
      new TableHeader('Is Listed'),
      new TableHeader('Is Watching'),
    ];
  }
}
