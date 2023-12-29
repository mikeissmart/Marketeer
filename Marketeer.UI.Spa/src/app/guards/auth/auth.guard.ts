import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable, map } from 'rxjs';
import { SecurityApiService } from '../../services/api/security-api.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private readonly router: Router,
    private readonly securityStore: SecurityApiService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (!this.securityStore.checkClientToken()) {
      return this.securityStore.refreshToken().pipe(
        map((result) => {
          if (result != null) {
            return this.securityStore.checkClientToken();
          } else {
            this.router.navigateByUrl('/', {
              skipLocationChange: false,
            });
            return false;
          }
        })
      );
    }

    return true;
  }
}
