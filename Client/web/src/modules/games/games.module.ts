import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';
import { SharedModule } from '../shared/shared.module';
import { DetailComponent } from './components/detail/detail.component';
import { ListComponent } from './components/list/list.component';
import { NetworkRoutingModule } from './games-routing.modules';
import { GamesService } from './services/games.service';
import { EditorComponent } from './components/editor/editor.component';

@NgModule({
    imports: [
        CommonModule,
        NetworkRoutingModule,
        SharedModule,
        MaterialModule,
        ReactiveFormsModule,
    ],
    declarations: [ListComponent, DetailComponent, EditorComponent],
    providers: [GamesService],
})
export class GamesModule {
}
