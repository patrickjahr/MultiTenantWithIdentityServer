import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {

    constructor() {
    }

    setItem(key: string, value: any) {
        const stringifiedValue = JSON.stringify(value);
        localStorage.setItem(key, stringifiedValue);
    }

    getItem<T>(key: string): T {
        const itemString = localStorage.getItem(key);
        return JSON.parse(itemString) as T;
    }

    getSessionStorageItem(key: string) {
        const itemString = sessionStorage.getItem(key);
        return itemString;
    }

    removeItem(key: string) {
        localStorage.removeItem(key);
    }
}
