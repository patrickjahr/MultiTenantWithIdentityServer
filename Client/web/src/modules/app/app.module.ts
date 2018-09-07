import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogConfig } from '@angular/material';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthConfig, OAuthModule, OAuthModuleConfig } from 'angular-oauth2-oidc';
import { environment } from '../../environments/environment';
import { MaterialModule } from '../material/material.module';
import { ForbiddenInterceptor } from '../shared/interceptors/forbidden.interceptor';
import { TenantService } from '../shared/services/tenant.service';
import { SharedModule } from '../shared/shared.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './components/app/app.component';
import { HeaderComponent } from './components/header/header.component';
import { CallbackComponent } from './components/callback/callback.component';

const oauthModuleConfig: OAuthModuleConfig = {
    resourceServer: {
        sendAccessToken: true,
    },
};

export function AuthConfigFactory(tenantService: TenantService): AuthConfig {
    const config: AuthConfig = {
        issuer: tenantService.getIdSrvUrl(),
        redirectUri: `${window.location.origin}/`,
        postLogoutRedirectUri: `${window.location.origin}/`, // TODO: Shoud may be set to global chainconfig web?
        clientId: '941b8aa0-0085-47ad-84da-73340390d946',
        scope: 'openid games-api',
        requireHttps: environment.production,
    };
    return config;
}

@NgModule({
    declarations: [
        AppComponent,
        HeaderComponent,
        CallbackComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule,
        SharedModule.forRoot(),
        MaterialModule,
        OAuthModule.forRoot(oauthModuleConfig),
    ],
    providers: [
        {
            provide: AuthConfig,
            useFactory: AuthConfigFactory,
            deps: [TenantService]
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ForbiddenInterceptor,
            multi: true,
        },
        {
            provide: MAT_DIALOG_DEFAULT_OPTIONS,
            useValue: {
                disableClose: true,
                hasBackdrop: true,
                autoFocus: false,
            } as MatDialogConfig<any>,
        },
    ],
    bootstrap: [AppComponent],
})
export class AppModule {
}
