import { Injectable } from '@angular/core';

const appPrefix = 'fourwallpapers-';

@Injectable()
export class LocalStorageService {

    constructor() {}

    setItem(key: string, value: any) {
        localStorage.setItem(`${appPrefix}${key}`, JSON.stringify(value));
    }

    getItem(key: string) {
        return JSON.parse(localStorage.getItem(`${appPrefix}${key}`));
    }

    static loadInitialState() {
        return Object.keys(localStorage)
            .reduce(
                (state: any, storageKey) => {
                    if (storageKey.includes(appPrefix)) {
                        state = state || {};
                        const stateKey = storageKey.replace(appPrefix, '')
                            .toLowerCase()
                            .split('.');
                        let currentStateRef = state;
                        stateKey.forEach(
                            (key, index) => {
                                if (index === stateKey.length - 1) {
                                    currentStateRef[key] = JSON
                                        .parse(
                                            localStorage.getItem(storageKey));
                                    return;
                                }
                                currentStateRef[key] =
                                    currentStateRef[key] || {};
                                currentStateRef = currentStateRef[key];
                            });
                    }
                    return state;
                },
                undefined);
    }

}
