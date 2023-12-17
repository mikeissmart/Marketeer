import { Component, OnInit } from '@angular/core';
import {
  INewsArticle,
  IPaginateGenericFilter,
  INewsFilter,
} from 'src/app/models/model';
import { ModelHelper } from 'src/app/models/model-helper';
import { TableHeader } from 'src/app/models/view-model';
import { NewsApiService } from 'src/app/services/api/news-api.service';
import { ToasterService } from 'src/app/services/toaster/toaster.service';

@Component({
  selector: 'app-news-list',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.scss'],
})
export class NewsListComponent implements OnInit {
  newArticles = ModelHelper.IPaginateGenericDefault<INewsArticle>();
  paginateFilter =
    ModelHelper.IPaginateGenericFilterDefault() as IPaginateGenericFilter<INewsFilter>;
  viewNewsArticle: INewsArticle | null = null;

  constructor(
    private readonly newsApi: NewsApiService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.paginateFilter.orderBy = 'Date';
    this.paginateFilter.isOrderAsc = false;
    this.fetchNewsArticles();
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
    this.newsApi.getFinanceNews(
      this.paginateFilter,
      (x) => (this.newArticles = x)
    );
  }

  updateNewsData(): void {
    this.newsApi.updateFinanceNews((result) => {
      if (result) {
        this.toaster.showSuccess('New News');
      } else {
        this.toaster.showWarning('News up to date');
      }
      this.fetchNewsArticles();
    });
  }

  onViewNews(news: INewsArticle): void {
    this.viewNewsArticle = news;
  }

  onViewCloseClose(): void {
    this.viewNewsArticle = null;
  }
}
