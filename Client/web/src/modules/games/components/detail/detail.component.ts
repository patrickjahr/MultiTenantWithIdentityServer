import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { filter, map, switchMap, tap } from 'rxjs/operators';
import { Game} from '../../models/game';
import { GamesService } from '../../services/games.service';

@Component({
    selector: 'gl-detail',
    templateUrl: './detail.component.html',
    styleUrls: ['./detail.component.scss'],
})
export class DetailComponent implements OnInit {

    game: Game;
    displayedColumns = ['name', 'creationDate', 'updateDate', 'configMode', 'edit'];

    constructor(private readonly _activatedRoute: ActivatedRoute, private readonly _gamesService: GamesService) {
    }

    ngOnInit() {
        this._activatedRoute.params.pipe(
            map(params => params['id']),
            filter(id => !!id),
            switchMap(id => this.load(id)),
        ).subscribe();
    }

    private load(id: string): Observable<any> {
        return this._gamesService.getGameItemById(id)
            .pipe(
                tap((game: Game) => {
                    this.game = game;
                }),
            );
    }
}
