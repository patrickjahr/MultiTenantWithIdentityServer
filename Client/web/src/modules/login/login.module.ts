import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './components/login/login.component';
import { LoginRoutingModule } from './login-routing.module';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        LoginRoutingModule,
        SharedModule,
        MaterialModule,
    ],
    declarations: [
        LoginComponent,
    ],
})
export class LoginModule {
}
