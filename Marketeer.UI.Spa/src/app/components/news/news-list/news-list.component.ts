import { Component, OnInit } from '@angular/core';
import {
  INewsArticle,
  IPaginateGenericFilter,
  INewsFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
import { NewsApiService } from 'src/app/services/api/news-api.service';
import { TickerApiService } from 'src/app/services/api/ticker-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-news-list',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.scss'],
})
export class NewsListComponent implements OnInit {
  searchSymbols = [] as string[];
  newArticles = ModelHelper.IPaginateGenericDefault<INewsArticle>();
  paginateFilter =
    ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<INewsFilter>;
  viewNewsArticle: INewsArticle | null = null;

  constructor(
    private readonly newsApi: NewsApiService,
    private readonly tickerApi: TickerApiService
  ) {}

  ngOnInit(): void {
    this.paginateFilter.orderBy = 'Date';
    this.paginateFilter.isOrderAsc = false;
    this.fetchNewsArticles();
  }

  onSearchSymbol(searchText: string): void {
    this.paginateFilter.filter.symbol = searchText;
    this.tickerApi.searchSymbols(
      searchText,
      8,
      (x) => (this.searchSymbols = x)
    );
  }

  onSelectedSymbol(symbol: string): void {
    this.paginateFilter.filter.symbol = symbol;
  }

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader(''),
      new TableHeader(''),
      new TableHeader('Title', 'Title'),
      new TableHeader('Date', 'Date'),
    ];
  }

  clear(): void {
    this.paginateFilter =
      ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<INewsFilter>;
    this.paginateFilter.orderBy = 'Date';
    this.paginateFilter.isOrderAsc = false;
  }

  fetchNewsArticles(): void {
    this.newsApi.getTickerNews(
      this.paginateFilter,
      (x) => (this.newArticles = x)
    );
  }

  onViewNews(news: INewsArticle): void {
    this.viewNewsArticle = news;
  }

  onViewCloseClose(): void {
    this.viewNewsArticle = null;
  }
}
