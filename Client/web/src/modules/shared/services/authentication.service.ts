import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthConfig, OAuthService, ReceivedTokens } from 'angular-oauth2-oidc';
import { TenantService } from './tenant.service';

@Injectable()
export class AuthenticationService {

    constructor(
        private readonly oauthService: OAuthService,
        private readonly authConfig: AuthConfig,
        private readonly tenantService: TenantService,
        private readonly router: Router,
    ) {
        this.oauthService.configure(this.authConfig);
    }

    activateSession(): Promise<void> {
        return this.oauthService.loadDiscoveryDocumentAndTryLogin({
            onTokenReceived: this.onTokenReceived.bind(this),
        });
    }

    login() {
        this.oauthService.initImplicitFlow(this.getAfterLoginRedirectUrl(), {
            acr_values: `tenant:${this.tenantService.getTenantName()}`,
        });
    }

    logout(): void {
        this.oauthService.logOut();
    }

    private onTokenReceived(receivedTokens: ReceivedTokens) {
        if (receivedTokens.state) {
            this.router.navigateByUrl(receivedTokens.state);
        }
    }

    private getAfterLoginRedirectUrl(): string {
        let url = this.router.url;

        if (url === '/login') {
            url = '/games';
        }

        return url;
    }
}
