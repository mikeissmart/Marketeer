import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// components
import { ForbiddenComponent } from './components/core/forbidden/forbidden.component';
import { HomeComponent } from './components/core/home/home.component';
import { LoginComponent } from './components/core/login/login.component';
import { ServerErrorComponent } from './components/core/server-error/server-error.component';
import { UnauthorizedComponent } from './components/core/unauthorized/unauthorized.component';
import { LogDetailsComponent } from './components/management/log-details/log-details.component';
import { TickerListComponent } from './components/market/ticker-list/ticker-list.component';
import { AuthGuard } from './guards/auth/auth.guard';

const routes: Routes = [
  // management
  {
    path: 'management/logs',
    component: LogDetailsComponent,
    canActivate: [AuthGuard],
  },
  // market
  {
    path: 'market/tickers',
    component: TickerListComponent,
    canActivate: [AuthGuard],
  },
  // core
  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'servererror', component: ServerErrorComponent },
  { path: 'login', component: LoginComponent },
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: false,
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
