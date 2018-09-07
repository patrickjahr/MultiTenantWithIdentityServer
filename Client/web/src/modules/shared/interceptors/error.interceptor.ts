import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private readonly _router: Router, private readonly _authenticationService: AuthenticationService) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                tap(
                    void 0,
                    e => {
                        if (e instanceof HttpErrorResponse && (e.status !== 401 || e.status !== 403)) {
                            this._router.navigate(['/error']);
                        }
                    },
                ),
            );
    }
}
