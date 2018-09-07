import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DetailComponent } from './components/detail/detail.component';
import { EditorComponent } from './components/editor/editor.component';
import { ListComponent } from './components/list/list.component';

const routes: Routes = [
    {
        path: '',
        component: ListComponent,
        pathMatch: 'full',
        /*canActivate: [IsAuthenticatedGuard],*/
    },
    {
        path: ':id',
        /*        canActivateChild: [IsAuthenticatedGuard],*/
        children: [
            {
                path: '',
                component: DetailComponent,
            },
            {
                path: 'editor',
                component: EditorComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class NetworkRoutingModule {

}
