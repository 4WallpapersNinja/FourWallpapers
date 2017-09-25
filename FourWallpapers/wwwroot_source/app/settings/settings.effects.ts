import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';

import { LocalStorageService, IAction } from '@app/core';

import {
    settingsKey as SETTINGS_KEY,
    settingsChangeTheme as SETTINGS_CHANGE_THEME,
    } from './settings.reducer';

@Injectable()
export class SettingsEffects {

    constructor(
        private actions$: Actions<IAction>,
        private localStorageService: LocalStorageService
    ) {}

    @Effect({ dispatch: false })
    persistThemeSettings(): Observable<IAction> {
        return this.actions$
            .ofType(SETTINGS_CHANGE_THEME)
            .do(
                action => this.localStorageService
                .setItem(SETTINGS_KEY, { theme: action.payload }));
    }

}