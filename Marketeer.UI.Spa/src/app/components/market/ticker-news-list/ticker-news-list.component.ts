import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  INewsArticle,
  INewsFilter,
  IPaginateGenericFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
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

  constructor(
    private readonly newsApi: NewsApiService,
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

  getTableHeaders(): TableHeader[] {
    return [
      new TableHeader(''),
      new TableHeader(''),
      new TableHeader('Title', 'Title'),
      new TableHeader('Date', 'Date'),
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

  onViewNews(news: INewsArticle): void {
    this.viewNewsArticle = news;
  }

  onViewCloseClose(): void {
    this.viewNewsArticle = null;
  }
}
