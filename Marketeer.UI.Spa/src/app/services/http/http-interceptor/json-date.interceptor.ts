import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import {
  BehaviorSubject,
  catchError,
  filter,
  map,
  Observable,
  switchMap,
  take,
  throwError,
} from 'rxjs';
import { SecurityApiService } from '../../api/security-api.service';
import { IToken } from 'src/app/models/model';

@Injectable()
export class JsonDateInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );
  private isoDateFormat = /\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-9]\d\.\d+/;

  constructor(private securityService: SecurityApiService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      map((val: HttpEvent<any>) => {
        if (val instanceof HttpResponse) {
          const body = val.body;
          this.convert(body);
        }
        return val;
      }),
      catchError((error) => {
        if (
          error instanceof HttpErrorResponse &&
          !request.url.includes('security/login') &&
          error.status == 401
        ) {
          return this.handle401Error(request, next).pipe(
            map((val: HttpEvent<any>) => {
              if (val instanceof HttpResponse) {
                const body = val.body;
                this.convert(body);
              }
              return val;
            })
          );
        }
        return throwError(() => error);
      })
    );
  }

  private isIsoDateString(value: any): boolean {
    if (value === null || value === undefined) {
      return false;
    }
    if (typeof value === 'string') {
      return this.isoDateFormat.test(value);
    }
    return false;
  }

  private convert(body: any) {
    if (body === null || body === undefined) {
      return body;
    }
    if (typeof body !== 'object') {
      return body;
    }
    for (const key of Object.keys(body)) {
      const value = body[key];
      if (this.isIsoDateString(value)) {
        body[key] = new Date(value);
      } else if (typeof value === 'object') {
        this.convert(value);
      }
    }
  }

  private handle401Error(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.securityService.refreshToken().pipe(
        switchMap((token) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(token);
          return next.handle(this.addToken(request, token!));
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter((token) => token != null),
        take(1),
        switchMap((token) => {
          return next.handle(this.addToken(request, token!));
        })
      );
    }
  }

  private addToken(request: HttpRequest<any>, token: IToken) {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token.accessToken}`,
      },
    });
  }
}
