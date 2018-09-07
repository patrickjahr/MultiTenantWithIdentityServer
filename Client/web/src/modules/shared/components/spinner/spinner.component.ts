import { Component, Input } from '@angular/core';

@Component({
    selector: 'gl-spinner',
    templateUrl: './spinner.component.html',
    styleUrls: ['./spinner.component.scss'],
})
export class SpinnerComponent {

    @Input()
    active: boolean;

}
