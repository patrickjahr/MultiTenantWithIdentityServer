import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsAuthenticatedGuard } from '../shared/guards/is-authenticated.guard';
import { CallbackComponent } from './components/callback/callback.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'callback',
        pathMatch: 'full',
    },
    {
        path: 'callback',
        component: CallbackComponent,
    },
    {
        path: 'login',
        loadChildren: '../login/login.module#LoginModule',
    },
    {
        path: 'games',
        loadChildren: '../games/games.module#GamesModule',
        canActivate: [IsAuthenticatedGuard],
    },
    {
        path: '**',
        redirectTo: 'games',
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {
}
