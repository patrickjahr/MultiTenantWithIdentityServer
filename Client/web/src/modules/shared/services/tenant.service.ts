import { Inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { WINDOW } from './window';

@Injectable()
export class TenantService {
    constructor(@Inject(WINDOW) private readonly window: Window) {
    }

    getTenantName(): string {
        const host = this.window.location.hostname;
        const splitHost = host.split('.');
        if (splitHost.length > 1) {
            return splitHost[0];
        }
        return '';
    }

    getApiUrl(): string {
        return environment.baseApiUrl.replace('{tenant}', this.getTenantName());
    }

    getIdSrvUrl(): string {
        const tenantName = this.getTenantName();
        return environment.identityApiUrl.replace('{tenant}', tenantName);
    }
}
