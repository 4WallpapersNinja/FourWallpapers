import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';

import { LocalStorageService } from '../local-storage/local-storage.service';
import { IAction } from '../core.interfaces';

import {
    authKey as AUTH_KEY,
    authLogin as AUTH_LOGIN,
    authLogout as AUTH_LOGOUT
    } from './auth.reducer';

@Injectable()
export class AuthEffects {

    constructor(
        private actions$: Actions<IAction>,
        private localStorageService: LocalStorageService
    ) {}

    @Effect({ dispatch: false })
    login(): Observable<IAction> {
        return this.actions$
            .ofType(AUTH_LOGIN)
            .do(
                action => this.localStorageService
                .setItem(AUTH_KEY, { isAuthenticated: true }));
    }

    @Effect({ dispatch: false })
    logout(): Observable<IAction> {
        return this.actions$
            .ofType(AUTH_LOGOUT)
            .do(
                action => this.localStorageService
                .setItem(AUTH_KEY, { isAuthenticated: false }));
    }

}
