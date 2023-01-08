import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { LoadingService } from '../../loading/loading.service';
import { SecurityApiService } from '../../api/security-api.service';
import { HttpService } from '../http/http.service';
import { ModelStateError, ModelStateErrors } from '../ModelStateErrors';
import { IStringData } from 'src/app/models/model';

@Injectable({
  providedIn: 'root',
})
export class ApiHttpService extends HttpService<ModelStateErrors> {
  public serverErrorMessage = '';

  constructor(
    private readonly router: Router,
    private readonly loadingService: LoadingService,
    private readonly httpClient: HttpClient,
    private readonly securityStore: SecurityApiService
  ) {
    super();

    this.initialize(
      this.loadingService,
      this.httpClient,
      environment.apiUrl,
      this.getHeaderOptions,
      this.handleErrors
    );
  }

  testAuth(callback: (result: string) => void): void {
    this.get('Security/TestAuth', (x: IStringData) => {
      callback(x.data!!);
    });
  }

  private getHeaderOptions(): { headers: HttpHeaders } {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    const token = this.securityStore.getJwtToken();
    if (token !== null) {
      options.headers = options.headers.append(
        'Authorization',
        `Bearer ${token}`
      );
    }
    return options;
  }

  private handleErrors(httpError: HttpErrorResponse): ModelStateErrors | null {
    const appError = httpError.headers.get('Application-Error');
    if (appError) {
      throw appError;
    }
    switch (httpError.status) {
      case 500: // server error
        this.serverErrorMessage = httpError.error;
        this.router.navigateByUrl('/servererror', {
          skipLocationChange: true,
        });
        break;
      case 401: // forbidden
        this.router.navigateByUrl('/forbidden', {
          skipLocationChange: true,
        });
        break;
      case 403: // unauthorized
        this.router.navigateByUrl('/unauthorized', {
          skipLocationChange: true,
        });
        break;
      case 400: // bad request
        const modelErrors = new ModelStateErrors();
        Object.getOwnPropertyNames(httpError.error.errors).forEach((x) => {
          const modelError = new ModelStateError();
          modelError.property = x;
          httpError.error.errors[x].forEach((y: string) => {
            modelError.descriptions.push(y);
          });
          modelErrors.errors.push(modelError);
        });
        return modelErrors;
    }
    return null;
  }
}
