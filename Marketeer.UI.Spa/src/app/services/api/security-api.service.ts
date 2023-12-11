import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { LoadingService } from '../loading/loading.service';
import { BehaviorSubject, Observable, catchError, map, of } from 'rxjs';
import { IClientToken, ILogin, IToken } from 'src/app/models/model';

@Injectable({
  providedIn: 'root',
})
export class SecurityApiService {
  private headerOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  private jwtToken: string | null = null;
  private readonly stateBehSub$: BehaviorSubject<IClientToken | null>;
  private stateObs$: Observable<IClientToken | null>;
  private baseUri = '';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly loadingService: LoadingService
  ) {
    this.stateBehSub$ = new BehaviorSubject<IClientToken | null>(null);
    this.stateObs$ = this.stateBehSub$.asObservable();
    this.baseUri = environment.apiUrl + 'security/';
    const token = this.getClientToken();
    if (token !== null) {
      this.setState(token);
    }
  }

  hasRole(role: string): boolean {
    const state = this.getClientToken();
    if (state == null) {
      return false;
    }
    return state.roles!!.includes(role);
  }

  hasAnyRoles(roles: string[]): boolean {
    const state = this.getClientToken();
    if (state == null) {
      return false;
    }

    const found = state.roles!!.some((x: string) => roles.includes(x));
    return found;
  }

  hasAllRoles(roles: string[]): boolean {
    let has = 0;
    roles.forEach((x) => {
      has += this.hasRole(x) ? 1 : 0;
    });
    return roles.length === has;
  }

  getClientToken(): IClientToken | null {
    var clientToken = this.getState();
    if (clientToken === null) {
      clientToken = this.parseJwtToken();
    }

    return clientToken;
  }

  checkClientToken(): boolean {
    const clientToken = this.getClientToken();

    if (clientToken !== null) {
      if (clientToken.exp!!.getTime() < Date.now()) {
        return false;
      }
    }

    return true;
  }

  getJwtToken(): string | null {
    return this.getClientToken() !== null ? this.jwtToken : null;
  }

  observable(): Observable<IClientToken | null> {
    return this.stateObs$;
  }

  login(
    userName: string,
    password: string,
    callback: (result: IClientToken | null) => void,
    errorCallback?: () => void
  ): void {
    const token = this.getClientToken();
    if (token !== null) {
      this.setState(token);
      callback(token);
    } else {
      const loadingDone = this.loadingService.show();
      this.httpClient
        .post<IToken>(
          this.baseUri + 'Login',
          { userName: userName, password: password } as ILogin,
          this.headerOptions
        )
        .subscribe({
          next: (result) => {
            loadingDone();
            localStorage.setItem('userToken', result.accessToken!!);
            localStorage.setItem('refreshToken', result.refreshToken!!);
            const clientToken = this.parseJwtToken();
            this.setState(clientToken);
            if (callback) callback(clientToken);
          },
          error: (httpError: HttpErrorResponse) => {
            loadingDone();
            if (errorCallback) {
              this.logout();
              errorCallback();
            }
          },
          complete: () => {},
        });
    }
  }

  refreshToken(): Observable<IToken | null> {
    const loadingDone = this.loadingService.show();
    return this.httpClient
      .post<IToken>(
        this.baseUri + 'Refresh',
        {
          accessToken: localStorage.getItem('userToken'),
          refreshToken: localStorage.getItem('refreshToken'),
        } as IToken,
        this.headerOptions
      )
      .pipe(
        map((result) => {
          loadingDone();
          localStorage.setItem('userToken', result.accessToken!!);
          localStorage.setItem('refreshToken', result.refreshToken!!);
          const clientToken = this.parseJwtToken();
          this.setState(clientToken);
          return result;
        }),
        catchError((error: HttpErrorResponse) => {
          loadingDone();
          this.logout();
          return of(null);
        })
      );
  }

  logout(callback?: () => void): void {
    localStorage.removeItem('userToken');
    localStorage.removeItem('refreshToken');
    this.setState(null);
    this.jwtToken = null;
    if (callback) callback();
  }

  private getState(): IClientToken | null {
    return this.stateBehSub$.getValue();
  }

  protected setState(state: IClientToken | null): void {
    this.stateBehSub$.next(state);
  }

  private parseJwtToken(): IClientToken | null {
    this.jwtToken = localStorage.getItem('userToken');
    if (this.jwtToken === null) return null;

    var base64Url = this.jwtToken!!.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );

    const jToken = JSON.parse(jsonPayload);
    const clientToken = {
      userId: Number.parseInt(jToken['UserId']),
      userName: jToken['UserName'],
      roles: jToken['Roles'].split(','),
      exp: new Date(Number.parseInt(jToken['exp']) * 1000),
      iat: new Date(Number.parseInt(jToken['iat']) * 1000),
      nbf: new Date(Number.parseInt(jToken['nbf']) * 1000),
    } as IClientToken;

    return clientToken;
  }
}
