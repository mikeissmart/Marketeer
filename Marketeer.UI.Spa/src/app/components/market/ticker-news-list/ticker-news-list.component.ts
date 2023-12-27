import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  INewsArticle,
  INewsFilter,
  IPaginateGenericFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { SentimentEnum, SentimentStatusEnum } from 'src/app/models/model.enum';
import { TableHeader } from 'src/app/models/view-model';
import { AiApiService } from 'src/app/services/api/ai-api.service';
import { NewsApiService } from 'src/app/services/api/news-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-ticker-news-list',
  templateUrl: './ticker-news-list.component.html',
  styleUrls: ['./ticker-news-list.component.scss'],
})
export class TickerNewsListComponent implements OnInit {
  @Input() set tickerId(value: number | null | undefined) {
    if (value == undefined || value == null) {
      this.currentSymbol = null;
    } else if (this.currentTickerId != value) {
      this.currentTickerId = value;
    }
  }
  @Input() set symbol(value: string | null | undefined) {
    if (value == undefined || value == null) {
      this.currentSymbol = null;
    } else if (this.currentSymbol != value) {
      this.currentSymbol = value;
    }
  }
  @Input() set lastNewsUpdate(value: Date | null | undefined) {
    if (value == undefined || value == null) {
      this.currentLastNewsUpdate = null;
      this.newArticles = ModelHelper.IPaginateGenericDefault<INewsArticle>();
    } else if (this.currentLastNewsUpdate != value) {
      this.currentLastNewsUpdate = value;
    }
  }
  @Output() onUpdateNewsArticles = new EventEmitter();

  currentTickerId: number | null = null;
  currentSymbol: string | null = null;
  currentLastNewsUpdate: Date | null = null;
  newArticles = ModelHelper.IPaginateGenericDefault<INewsArticle>();
  paginateFilter =
    ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<INewsFilter>;
  viewNewsArticle: INewsArticle | null = null;
  sentimentStatusEnum = SentimentStatusEnum;
  sentimentEnum = SentimentEnum;

  constructor(
    private readonly newsApi: NewsApiService,
    private readonly aiApi: AiApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.paginateFilter.filter.symbol = this.currentSymbol!;
    this.paginateFilter.orderBy = 'Date';
    this.paginateFilter.isOrderAsc = false;
    if (this.currentLastNewsUpdate != null) {
      this.fetchNewsArticles();
    }
  }

  getSentimentTableHeaders(): TableHeader[] {
    return [
      new TableHeader(''),
      new TableHeader(''),
      new TableHeader('Title', 'Title'),
      new TableHeader('Date', 'Date'),
      new TableHeader('Last Sentiment'),
    ];
  }

  fetchNewsArticles(): void {
    if (this.currentSymbol != null) {
      this.newsApi.getTickerNews(
        this.paginateFilter,
        (x) => (this.newArticles = x)
      );
    }
  }

  updateNewsData(): void {
    if (this.currentSymbol != null) {
      this.newsApi.updateTickerNews(this.currentTickerId!, (result) => {
        if (result) {
          this.toaster.showSuccess('New News');
        } else {
          this.toaster.showWarning('News up to date');
        }
        this.fetchNewsArticles();
        this.onUpdateNewsArticles.emit();
      });
    }
  }

  enqueueNewsDefaultSentiment(): void {
    this.aiApi.queueTickerNewsDefaultSentiment(this.currentSymbol!, (result) =>
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
