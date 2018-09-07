import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { TokenService } from '../services/token.service';

@Injectable()
export class IsAuthenticatedGuard implements CanActivate, CanActivateChild {
    constructor(private readonly _tokenService: TokenService, private readonly _router: Router) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        return this.isAuthenticated();
    }

    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        return this.isAuthenticated();
    }

    private isAuthenticated() {
        if (!this._tokenService.token) {
            this._router.navigate(['/login']);
            return false;
        }

        return true;
    }
}
