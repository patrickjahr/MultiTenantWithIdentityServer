import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TenantService } from '../../shared/services/tenant.service';
import { Game } from '../models/game';

@Injectable()
export class GamesService {

    constructor(private readonly _httpClient: HttpClient, private readonly tenantService: TenantService) {
    }

    getGamesList(): Observable<Game[]> {
        return this._httpClient.get<Game[]>(`${this.tenantService.getApiUrl()}/api/games`);
    }

    getGameItemById(id: string): Observable<Game> {
        return this._httpClient.get<Game>(`${this.tenantService.getApiUrl()}/api/games/${id}`);
    }

    createNewGameItem(game: Game): Observable<number> {
        return this._httpClient.post<number>(`${this.tenantService.getApiUrl()}/api/games`, JSON.stringify(game, (key, value) => {
            if (value !== null) {
                return value;
            }
        }), { headers: new HttpHeaders().set('Content-Type', 'application/json') });
    }

    updateNewGameItem(game: Game): Observable<void> {
        return this._httpClient.put<void>(`${this.tenantService.getApiUrl()}/api/games/${game.id}`, game);
    }

    deleteGameById(id: number) {
        return this._httpClient.delete<void>(`${this.tenantService.getApiUrl()}/api/games/${id}`);
    }
}
