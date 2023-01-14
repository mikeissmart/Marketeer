import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbDateAdapter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CommonModule, DatePipe } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Components
import { ForbiddenComponent } from './components/core/forbidden/forbidden.component';
import { HomeComponent } from './components/core/home/home.component';
import { LoginComponent } from './components/core/login/login.component';
import { NavMenuComponent } from './components/core/nav-menu/nav-menu.component';
import { ServerErrorComponent } from './components/core/server-error/server-error.component';
import { ToasterComponent } from './components/core/toaster/toaster.component';
import { UnauthorizedComponent } from './components/core/unauthorized/unauthorized.component';
import { AppLogDetailsComponent } from './components/management/app-log-details/app-log-details.component';
import { CronLogDetailsComponent } from './components/management/cron-log-details/cron-log-details.component';
import { PythonLogDetailsComponent } from './components/management/python-log-details/python-log-details.component';
import { LogDetailsComponent } from './components/management/log-details/log-details.component';
import { PaginateComponent } from './components/common/paginate/paginate.component';
import { EnumToArrayPipe } from './pipes/enum-to-array.pipe';
import { JsonDateInterceptor } from './services/http/http-interceptor/json-date.interceptor';
import { TableComponent } from './components/common/table/table.component';
import { TruncatePipe } from './pipes/truncate.pipe';
import { ModalComponent } from './components/common/modal/modal.component';
import { DatePickerComponent } from './components/common/date-picker/date-picker.component';
import { NgbDatePickerAdapter } from './services/data/ngb-date-picker-adapter';
import { DatePickerRangeComponent } from './components/common/date-picker-range/date-picker-range.component';
import { TickerListComponent } from './components/market/ticker-list/ticker-list.component';
import { AutoCompleteComponent } from './components/common/auto-complete/auto-complete.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    ForbiddenComponent,
    ServerErrorComponent,
    UnauthorizedComponent,
    ToasterComponent,
    AppLogDetailsComponent,
    CronLogDetailsComponent,
    PythonLogDetailsComponent,
    LogDetailsComponent,
    EnumToArrayPipe,
    PaginateComponent,
    TableComponent,
    TruncatePipe,
    ModalComponent,
    DatePickerComponent,
    DatePickerRangeComponent,
    TickerListComponent,
    AutoCompleteComponent,
  ],
  imports: [
    ReactiveFormsModule,
    CommonModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    NgxSpinnerModule,
    NgbModule,
    BrowserAnimationsModule,
  ],
  providers: [
    DatePipe,
    { provide: NgbDateAdapter, useClass: NgbDatePickerAdapter },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JsonDateInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
