import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../shared/services/authentication.service';

@Component({
    selector: 'gl-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
    constructor(private readonly authenticationService: AuthenticationService) {
    }

    ngOnInit(): void {
        this.authenticationService.activateSession();
    }


}
