import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthConfig } from 'angular-oauth2-oidc';
import { TokenService } from '../../../shared/services/token.service';

@Component({
    selector: 'gl-welcome',
    templateUrl: './callback.component.html',
    styleUrls: ['./callback.component.scss'],
})
export class CallbackComponent implements OnInit {

    constructor(authConfig: AuthConfig, private _tokenService: TokenService, private _router: Router) {
    }

    ngOnInit(): void {
        setTimeout(() => {
            if (this._tokenService.token !== '') {
                this._router.navigate(['games']);
            } else {
                this._router.navigate(['login']);
            }
        }, 1000);
    }
}
