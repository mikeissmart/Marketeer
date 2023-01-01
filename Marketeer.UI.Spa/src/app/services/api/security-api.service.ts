import { Injectable } from '@angular/core';
import {
  IClientTokenDto,
  ILoginDto,
  IStringDataDto,
} from 'src/app/models/models';

import { environment } from 'src/environments/environment';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { LoadingService } from '../loading/loading.service';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';

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
  private readonly stateBehSub$: BehaviorSubject<IClientTokenDto | null>;
  private stateObs$: Observable<IClientTokenDto | null>;
  private baseUri = '';

  constructor(
    private readonly httpClient: HttpClient,
    private readonly loadingService: LoadingService
  ) {
    this.stateBehSub$ = new BehaviorSubject<IClientTokenDto | null>(null);
    this.stateObs$ = this.stateBehSub$.asObservable();
    this.baseUri = environment.apiUrl + 'security/';
    this.getClientToken();
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

  getClientToken(): IClientTokenDto | null {
    var clientToken = this.getState();
    if (clientToken === null) {
      clientToken = this.parseJwtToken();
      this.setState(clientToken);
    }

    if (clientToken !== null) {
      if (clientToken.exp!!.getTime() < Date.now()) {
        // Token expired
        this.logout();
      }
    }

    return clientToken;
  }

  getJwtToken(): string | null {
    return this.getClientToken() !== null ? this.jwtToken : null;
  }

  observable(): Observable<IClientTokenDto | null> {
    return this.stateObs$;
  }

  login(
    userName: string,
    password: string,
    callback: (result: IClientTokenDto | null) => void,
    errorCallback?: () => void
  ): void {
    const token = this.getClientToken();
    if (token !== null) {
      callback(token);
    } else {
      const loadingDone = this.loadingService.show();
      this.httpClient
        .post<IStringDataDto>(
          this.baseUri + 'Login',
          { UserName: userName, Password: password } as ILoginDto,
          this.headerOptions
        )
        .subscribe({
          next: (result) => {
            loadingDone();
            localStorage.setItem('userToken', result.data!!);
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

  logout(callback?: () => void): void {
    localStorage.removeItem('userToken');
    this.setState(null);
    this.jwtToken = null;
    if (callback) callback();
  }

  private getState(): IClientTokenDto | null {
    return this.stateBehSub$.getValue();
  }

  protected setState(state: IClientTokenDto | null): void {
    this.stateBehSub$.next(JSON.parse(JSON.stringify(state)));
  }

  private parseJwtToken(): IClientTokenDto | null {
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
    } as IClientTokenDto;

    return clientToken;
  }
}
