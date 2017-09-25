import { IAction } from '../core.interfaces';

export const initialState = {
    isAuthenticated: false
};

export const authKey = 'AUTH';
export const authLogin = 'AUTH_LOGIN';
export const authLogout = 'AUTH_LOGOUT';

export const login = () => ({ type: authLogin });
export const logout = () => ({ type: authLogout });

export const selectorAuth = state => state.auth;

export function authReducer(state = initialState, action: IAction) {
    switch (action.type) {
        case authLogin:
            return Object.assign(
                {},
                state,
                {
                    isAuthenticated: true
                });

        case authLogout:
            return Object.assign(
                {},
                state,
                {
                    isAuthenticated: false
                });

        default:
            return state;
    }
}
