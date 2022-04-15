import { makeAutoObservable } from 'mobx';
import AuthService from '../services/AuthService';

export interface IAuth {
    access_token: string
    login: string
    pass: string
}

export default class LoginStore {
    auth = {} as IAuth;
    isAuth = false;

    public constructor() {
        makeAutoObservable(this);
    }

    setIsAuth(bool: boolean) {
        this.isAuth = bool;
    }

    setAuth(auth: IAuth) {
        this.auth = auth;
    }

    async login(email: string, password: string) {
        try {
            const response = await AuthService.login(email, password);
            localStorage.setItem('token', response.access_token);
            this.isAuth = true;
            console.log(response);
        }
        catch (e) {
            console.log(e);
        }
    }

}


