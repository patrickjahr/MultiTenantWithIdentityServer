import { Injectable } from '@angular/core';
import { StorageService } from './storage.service';

const TOKEN_KEY = 'access_token';

@Injectable()
export class TokenService {

    constructor(private readonly _storageService: StorageService) {
    }

    private _token: string;

    get token(): string {
        if (!this._token) {
            this._token = this._storageService.getSessionStorageItem(TOKEN_KEY);
        }
        return this._token;
    }

    set token(token: string) {
        this._storageService.setItem(TOKEN_KEY, token);
        this._token = token;
    }
}
