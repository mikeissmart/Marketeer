import { Injectable } from '@angular/core';
import { ApiHttpService } from '../http/api-http/api-http.service';
import { IHuggingFaceModel, IStringData } from 'src/app/models/model';
import { SentimentResultTypeEnum } from 'src/app/models/model.enum';

@Injectable({
  providedIn: 'root',
})
export class AiApiService {
  uri = 'AI/';

  constructor(private readonly apiHttp: ApiHttpService) {}

  getHuggingFaceModels(callback: (result: IHuggingFaceModel[]) => void): void {
    this.apiHttp.get(this.uri + `GetHuggingFaceModels`, callback);
  }

  queueTickerNewsDefaultSentiment(
    symbol: string,
    callback: (result: IStringData) => void
  ): void {
    this.apiHttp.get(
      this.uri + `QueueTickerNewsDefaultSentiment?symbol=${symbol}`,
      callback
    );
  }

  queueNewsDefaultSentiment(
    ids: number[],
    callback: (result: IStringData) => void
  ): void {
    this.apiHttp.post(this.uri + `QueueNewsDefaultSentiment`, ids, callback);
  }
}
