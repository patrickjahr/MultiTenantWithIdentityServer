import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';

export interface ConfirmDialogData {
    title: string;
    subject: string;
    confirmValueText: string;
}

@Component({
    selector: 'gl-confirm-dialog',
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.scss'],
})
export class ConfirmDialogComponent {
    constructor(@Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData) {
    }
}
