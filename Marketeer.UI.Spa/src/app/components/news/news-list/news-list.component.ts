import { Component, OnInit } from '@angular/core';
import {
  INewsArticle,
  IPaginateGenericFilter,
  INewsFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { SentimentEnum, SentimentStatusEnum } from 'src/app/models/model.enum';
import { TableHeader } from 'src/app/models/view-model';
import { AiApiService } from 'src/app/services/api/ai-api.service';
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
  sentimentEnum = SentimentEnum;
  sentimentStatusEnum = SentimentStatusEnum;

  constructor(
    private readonly newsApi: NewsApiService,
    private readonly tickerApi: TickerApiService,
    private readonly aiApi: AiApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.paginateFilter.orderBy = 'ArticleDate';
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
      new TableHeader('Date', 'ArticleDate'),
      new TableHeader('Last Sentiment'),
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

  enqueueNewsDefaultSentiment(): void {
    var ids = [] as number[];
    this.newArticles.items.forEach((x) => ids.push(x.id));

    this.aiApi.queueNewsDefaultSentiment(ids, (result) =>
      this.toaster.showSuccess(result.data)
    );
  }

  getLastSentimentResult(item: INewsArticle): SentimentEnum | null {
    if (item.sentimentResults.length > 0) {
      const completed = item.sentimentResults.filter(
        (x) => x.status == SentimentStatusEnum.Completed
      );
      if (completed.length > 0) {
        return completed[completed.length - 1].sentiment;
      }
    }

    return null;
  }

  getSentimentResultColor(value: SentimentEnum | null): string {
    if (value == SentimentEnum.Positive) {
      return 'bg-success text-light';
    } else if (value == SentimentEnum.Neutral) {
      return 'bg-warning text-dark';
    } else if (value == SentimentEnum.Negative) {
      return 'bg-danger text-light';
    } else {
      return '';
    }
  }

  getNewsSentimentTableHeaders(): TableHeader[] {
    return [
      new TableHeader('Model Name'),
      new TableHeader('Date'),
      new TableHeader('Status'),
      new TableHeader('Positive Score'),
      new TableHeader('Neutral Score'),
      new TableHeader('Negitive Score'),
      new TableHeader('Sentiment'),
    ];
  }

  onViewNews(news: INewsArticle): void {
    this.viewNewsArticle = news;
  }

  onViewCloseClose(): void {
    this.viewNewsArticle = null;
  }
}
