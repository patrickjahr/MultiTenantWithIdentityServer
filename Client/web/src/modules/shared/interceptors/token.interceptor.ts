import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenService } from '../services/token.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(private readonly _tokenService: TokenService) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this._tokenService.token) {
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${this._tokenService.token}`,
                },
            });
        }

        return next.handle(req);
    }

}
