import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IClientTokenDto } from 'src/app/models/models';
import { SecurityApiService } from '../../services/api/security-api.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit, OnDestroy {
  private clientTokenSub: Subscription | null = null;

  isCollapsed = true;
  clientToken: IClientTokenDto | null = null;

  constructor(
    private readonly securityStore: SecurityApiService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.clientTokenSub = this.securityStore
      .observable()
      .subscribe((x) => (this.clientToken = x));
    /*this.securityStore.subscribe(this, (clientToken) => {
      this.clientToken = clientToken;
    });*/
  }
  ngOnDestroy(): void {
    this.clientTokenSub!.unsubscribe();
    this.clientTokenSub = null;
  }

  logout(): void {
    this.securityStore.logout(() => {
      if (this.router.url === '/') {
        window.location.reload();
      } else {
        this.router.navigateByUrl('/');
      }
    });
  }
}
