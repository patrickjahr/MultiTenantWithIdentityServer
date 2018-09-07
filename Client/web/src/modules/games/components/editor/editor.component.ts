import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, switchMap, filter, tap, finalize } from 'rxjs/operators';
import { Game } from '../../models/game';
import { GamesService } from '../../services/games.service';

@Component({
  selector: 'gl-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss']
})
export class EditorComponent implements OnInit {

    formGroup: FormGroup;
    isLoading: boolean;

    constructor(
        private readonly _activatedRoute: ActivatedRoute,
        private readonly _gameService: GamesService,
        private readonly _formBuilder: FormBuilder,
        private readonly _router: Router,
    ) {
        this.createForm();
    }

    ngOnInit() {
        this._activatedRoute.params.pipe(
            map(({ id }: Params) => id),
            filter(id => !!id),
            switchMap(id => this.load(id)),
        ).subscribe();
    }

    submit() {
        if (!this.formGroup.value.id) {
            this._gameService.createNewGameItem(this.formGroup.value)
                .subscribe(created => this.navigateToDetail(created));
        }

        this._gameService.updateNewGameItem(this.formGroup.value)
            .subscribe(() => this.navigateToDetail(this.formGroup.value.id));
    }

    private navigateToDetail(id: number) {
        this._router.navigate(['../..', id], { relativeTo: this._activatedRoute });
    }

    private load(id: string): Observable<any> {
        this.isLoading = true;
        return this._gameService.getGameItemById(id)
            .pipe(
                tap((game: Game) => {
                    this.formGroup.setValue({
                        id: game.id,
                        name: game.name,
                        console: game.console,
                        type: game.type
                    });
                }),
                finalize(() => this.isLoading = false),
            );
    }

    private createForm() {
        this.formGroup = this._formBuilder.group({
            id: undefined,
            name: ['', Validators.required],
            console: '',
            type: ''
        });
    }

}
