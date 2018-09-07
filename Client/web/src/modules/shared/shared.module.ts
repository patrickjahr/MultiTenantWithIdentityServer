import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { MatButtonModule, MatDialogModule, MatProgressSpinnerModule } from '@angular/material';
import { JwksValidationHandler, OAuthModule, ValidationHandler } from 'angular-oauth2-oidc';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { IsAuthenticatedGuard } from './guards/is-authenticated.guard';
import { CustomDatePipe } from './pipes/custom-date.pipe';
import { AuthenticationService } from './services/authentication.service';
import { StorageService } from './services/storage.service';
import { TenantService } from './services/tenant.service';
import { TokenService } from './services/token.service';
import { WINDOW, windowFactory } from './services/window';

const SHARED_COMPONENTS = [
    SpinnerComponent,
    CustomDatePipe,
    ConfirmDialogComponent,
];

@NgModule({
    imports: [
        CommonModule,
        MatProgressSpinnerModule,
        MatDialogModule,
        MatButtonModule,
        OAuthModule,
    ],
    declarations: SHARED_COMPONENTS,
    exports: SHARED_COMPONENTS,
    entryComponents: [ConfirmDialogComponent],
})
export class SharedModule {

    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                StorageService,
                AuthenticationService,
                TokenService,
                IsAuthenticatedGuard,
                {
                    provide: ValidationHandler,
                    useClass: JwksValidationHandler,
                },

                {
                    provide: WINDOW,
                    useFactory: windowFactory,
                },
                TenantService,
            ],
        };
    }
}
