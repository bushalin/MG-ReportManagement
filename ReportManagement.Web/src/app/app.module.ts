import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// import ngx-translate and http loader
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReportModule } from './report/report.module';
import { CommonService } from 'src/services/common.services';
import { PageNotFoundComponent } from './common/page-not-found/page-not-found.component';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
import { UserProfileCreateComponent } from './user/user-profile-create/user-profile-create.component';
import { UserProfileEditComponent } from './user/user-profile-edit/user-profile-edit.component';
import { AuthenticationService } from 'src/services/authentication.service';
import { LoginComponent } from './common/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpInterceptorService } from 'src/services/http-interceptor.service';
import { ErrorInterceptorService } from 'src/services/error-interceptor.service';
import { LandingPageComponent } from './common/landing-page/landing-page.component';
import { UserRegistrationComponent } from './user/user-registration/user-registration.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { AdminHomeComponent } from './admin/admin-home/admin-home.component';
import { RoleCreationComponent } from './admin/role/role-creation/role-creation.component';
import { RoleEditComponent } from './admin/role/role-edit/role-edit.component';
import { RoleDeleteComponent } from './admin/role/role-delete/role-delete.component';
import { RoleAssignComponent } from './admin/role/role-assign/role-assign.component';
import { SetDefaultPasswordComponent } from './admin/set-default-password/set-default-password.component';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    UserProfileComponent,
    UserProfileCreateComponent,
    UserProfileEditComponent,
    LoginComponent,
    LandingPageComponent,
    UserRegistrationComponent,
    UserListComponent,
    AdminHomeComponent,
    RoleCreationComponent,
    RoleEditComponent,
    RoleDeleteComponent,
    RoleAssignComponent,
    SetDefaultPasswordComponent,
  ],
  imports: [
    ReportModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    
    // configure translate module
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [CommonService, AuthenticationService, {provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true},
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }

// required for AOT compilation
export function HttpLoaderFactory(http: HttpClient){
  return new TranslateHttpLoader(http);
}
