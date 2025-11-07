import { ErrorInterceptor } from './helpers/error.interceptor';
import { JwtInterceptor } from './helpers/jwt.interceptor';
import { ListNotificationsComponent } from './components/notifications/list-notifications/list-notifications.component';
import { TestComponent } from './components/test-component/test-component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ProfileComponent } from './components/profile/profile.component';
import { ErrorComponent } from './components/error/error.component';
import { ForgotComponent } from './components/forgot/forgot.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { NgModule } from '@angular/core';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';

// Import routing module
import { AppRoutingModule } from './app-routing.module';

// Import app component
import { AppComponent } from './app.component';

// Import containers
import {
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
} from './containers';

// Import per la localizzazione
import { LOCALE_ID } from '@angular/core';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { registerLocaleData } from '@angular/common';
import localeIt from '@angular/common/locales/it';
import { AvatarModule } from 'ngx-avatars';
import { ToastrModule } from 'ngx-toastr';
import { PendingChangesGuard } from './helpers/componentDeactivate.guard';
registerLocaleData(localeIt, 'it-IT');

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { Injector } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { EqpUiModule } from '@eqproject/eqp-ui';
import { ChangeForgotPasswordComponent } from './components/change-forgot-password/change-forgot-password.component';
import { TenantInterceptor } from './interceptors/tenant.interceptor';
import { SharedModule } from '../modules/shared.module';
import { RegistryModule } from './components/registry/registry.module';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MessageBarComponent } from './elements/message-bar/message-bar-component';
import {TopbarComponent}  from './components/topbar-component/topbar.component'
//TODO: usare npm i ngx-scrollbar con suppressScrollX come per il DEFAULT_PERFECT_SCROLLBAR_CONFIG (deprecato)
// const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
//   suppressScrollX: true,
// };

const APP_CONTAINERS = [
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
];

@NgModule({
  declarations: [AppComponent,
    LoginComponent,
    RegisterComponent,
    SpinnerComponent,
    ForgotComponent,
    ErrorComponent,
    ProfileComponent,
    ChangePasswordComponent,
    TestComponent,
    MessageBarComponent,
    ListNotificationsComponent, ...APP_CONTAINERS, ChangeForgotPasswordComponent,TopbarComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AvatarModule,
    ReactiveFormsModule,
    SharedModule.forRoot(),
    FontAwesomeModule,
    ToastrModule.forRoot(),
    EqpUiModule,
    RegistryModule,
    MatSnackBarModule
  ],
  providers: [
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: TenantInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'it-IT' },
    { provide: LOCALE_ID, useValue: 'it-IT' },
    PendingChangesGuard,
    Title
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(private injector: Injector) {
    InjectorInstance = this.injector;
  }
}

export let InjectorInstance: Injector;
