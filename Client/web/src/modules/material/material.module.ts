import { NgModule } from '@angular/core';
import {
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDialogModule,
    MatIconModule,
    MatInputModule,
    MatMenuModule,
    MatSelectModule,
    MatSidenavModule,
    MatTableModule,
    MatToolbarModule,
} from '@angular/material';

const SHARED_MODULES = [
    MatToolbarModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatSelectModule,
    MatDialogModule,
    MatInputModule,
    MatTableModule,
    MatCheckboxModule,
    MatMenuModule,
];

@NgModule({
    imports: [
        SHARED_MODULES,
    ],
    exports: [
        SHARED_MODULES,
    ],
})
export class MaterialModule {
}
