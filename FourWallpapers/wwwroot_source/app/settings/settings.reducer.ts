import { IAction } from '@app/core';

export const initialState = {
    theme: 'DEFAULT-THEME'
};

export const settingsKey = 'SETTINGS';
export const settingsChangeTheme = 'SETTINGS_CHANGE_THEME';

export const actionChangeTheme = (theme: string) =>
    ({ type: settingsChangeTheme, payload: theme });

export const selectorSettings = state => state.settings || { theme: '' };

export function settingsReducer(state = initialState, action: IAction) {
    switch (action.type) {
        case settingsChangeTheme:
            return { theme: action.payload };

        default:
            return state;
    }
}