import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AuthenticationService } from '../../../shared/services/authentication.service';

@Component({
    selector: 'gl-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {

    @Input()
    title: string;

    @Output()
    toggleSideNav = new EventEmitter();

    constructor(private readonly _authenticationService: AuthenticationService) {
    }

    performLogout() {
        this._authenticationService.logout();
    }
}
