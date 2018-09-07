import { Component } from '@angular/core';
import { AuthConfig } from 'angular-oauth2-oidc';
import { AuthenticationService } from '../../../shared/services/authentication.service';

@Component({
    selector: 'gl-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
    registerUrl: string;

    constructor(private readonly authenticationService: AuthenticationService, authConfig: AuthConfig) {
        this.registerUrl = `${authConfig.issuer}/register/register`;
    }

    performLogin() {
        this.authenticationService.login();
    }

    openRegistrationPage() {
        window.open(this.registerUrl);
    }

    performLogout() {
        this.authenticationService.logout();
    }
}
