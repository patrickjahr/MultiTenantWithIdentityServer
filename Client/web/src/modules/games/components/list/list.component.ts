import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Game } from '../../models/game';
import { GamesService } from '../../services/games.service';

@Component({
    selector: 'gl-list',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss'],
})
export class ListComponent implements OnInit {

    gamesList$: Observable<Game[]>;
    displayedColumns = ['name', 'console', 'type', 'edit', 'delete'];

    constructor(private readonly _gamesService: GamesService, private readonly _dialog: MatDialog) {
    }

    ngOnInit() {
        this.gamesList$ = this._gamesService.getGamesList();
    }

    deleteGame(id: number) {
        const dialogRef = this._dialog.open<ConfirmDialogComponent, ConfirmDialogData>(ConfirmDialogComponent, {
            data: {
                title: `Delete Game: ${id}`,
                subject: `Do you really want to delete this Game from your list?`,
                confirmValueText: 'Yes',
            },
        });

        dialogRef.afterClosed()
            .pipe(
                filter(result => result),
                switchMap(() => this._gamesService.deleteGameById(id)),
            )
            .subscribe(() => {
                this.gamesList$ = this._gamesService.getGamesList();
            });
    }
}
