import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { LoadingService } from '../../loading/loading.service';

export abstract class HttpService<TErrorResponse> {
  private _loadingService: LoadingService = {} as LoadingService;
  private _httpClient: HttpClient = {} as HttpClient;
  private _url: string = '';
  private _headersCallback: () => { headers: HttpHeaders } = {} as () => {
    headers: HttpHeaders;
  };
  private _handleErrorCallback: (
    httpError: HttpErrorResponse,
    errorCallback: (error: TErrorResponse | null) => void
  ) => void = {} as (
    httpError: HttpErrorResponse,
    errorCallback: (error: TErrorResponse | null) => void
  ) => void;

  protected initialize(
    loadingService: LoadingService,
    httpClient: HttpClient,
    url: string,
    headersCallback: () => { headers: HttpHeaders },
    handleErrorCallback: (
      httpError: HttpErrorResponse,
      errorCallback: (error: TErrorResponse | null) => void
    ) => void
  ): void {
    this._loadingService = loadingService;
    this._httpClient = httpClient;
    this._url = url;
    this._headersCallback = headersCallback;
    this._handleErrorCallback = handleErrorCallback;
  }

  /**
   * Read - resources.
   */
  get<TResult>(
    api: string,
    callback: (result: TResult) => void,
    errorCallback?: (error: TErrorResponse) => void
  ): void {
    const loadingDone = this._loadingService.show();
    this._httpClient
      .get<TResult>(this._url + api, this._headersCallback!!())
      .subscribe({
        next: (result) => {
          loadingDone();
          callback(result);
        },
        error: (httpError: HttpErrorResponse) => {
          loadingDone();
          this._handleErrorCallback!!(httpError, (error) => {
            if (errorCallback && error !== null) errorCallback(error);
          });
        },
      });
  }

  /**
   * Create - new resources.
   */
  post<TResult>(
    api: string,
    payload: any,
    callback: (result: TResult) => void,
    errorCallback?: (error: TErrorResponse) => void
  ): void {
    const loadingDone = this._loadingService.show();
    this._httpClient
      .post<TResult>(this._url + api, payload, this._headersCallback!!())
      .subscribe({
        next: (result) => {
          loadingDone();
          callback(result);
        },
        error: (httpError: HttpErrorResponse) => {
          loadingDone();
          this._handleErrorCallback!!(httpError, (error) => {
            if (errorCallback && error !== null) errorCallback(error);
          });
        },
      });
  }

  /**
   * Update - newly-updated representation of the original resource.
   */
  put<TResult>(
    api: string,
    payload: any,
    callback: (result: TResult) => void,
    errorCallback?: (error: TErrorResponse) => void
  ): void {
    const loadingDone = this._loadingService.show();
    this._httpClient
      .put<TResult>(this._url + api, payload, this._headersCallback!!())
      .subscribe({
        next: (result) => {
          loadingDone();
          callback(result);
        },
        error: (httpError: HttpErrorResponse) => {
          loadingDone();
          this._handleErrorCallback!!(httpError, (error) => {
            if (errorCallback && error !== null) errorCallback(error);
          });
        },
      });
  }

  /**
   * Modify - contain the changes to the resource, not the complete resource.
   */
  patch<TResult>(
    api: string,
    payload: any,
    callback: (result: TResult) => void,
    errorCallback?: (error: TErrorResponse) => void
  ): void {
    const loadingDone = this._loadingService.show();
    this._httpClient
      .patch<TResult>(this._url + api, payload, this._headersCallback!!())
      .subscribe({
        next: (result) => {
          loadingDone();
          callback(result);
        },
        error: (httpError: HttpErrorResponse) => {
          loadingDone();
          this._handleErrorCallback!!(httpError, (error) => {
            if (errorCallback && error !== null) errorCallback(error);
          });
        },
      });
  }

  /**
   * Delete - delete resource.
   */
  delete<TResult>(
    api: string,
    callback: (result: TResult) => void,
    errorCallback?: (error: TErrorResponse) => void
  ): void {
    const loadingDone = this._loadingService.show();
    this._httpClient
      .delete<TResult>(this._url + api, this._headersCallback!!())
      .subscribe({
        next: (result) => {
          loadingDone();
          callback(result);
        },
        error: (httpError: HttpErrorResponse) => {
          loadingDone();
          this._handleErrorCallback!!(httpError, (error) => {
            if (errorCallback && error !== null) errorCallback(error);
          });
        },
      });
  }
}
