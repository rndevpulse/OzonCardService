import { makeAutoObservable } from 'mobx';
import AuthService from '../services/AuthService';



export default class LoginStore {
    Roles: string[] = [];
    IsAuth = false;
    IsLoading = false;
    public constructor() {
        makeAutoObservable(this);
    }

    setIsAuth(bool: boolean) {
        this.IsAuth = bool;
    }

    setLoading(bool: boolean) {
        this.IsLoading = bool;
    }
    setRules(rules: string[]) {
        this.Roles = rules;
    }

    async login(email: string, password: string) {
        try {
            const response = await AuthService.login(email, password);
            localStorage.setItem('token', response.data.access);
            this.setIsAuth(true);
            this.setRules(response.data.roles);
            //console.log(response);
        }
        catch (e) {
            //console.log(e);
        }
    }

    async logout() {
        try {
            await AuthService.logout();
            localStorage.removeItem('token');
            this.setIsAuth(false);
            //console.log(responce);
        }
        catch (e) {
            //console.log(e);
        }
    }

    async checkAuth() {
        this.IsLoading = true;
        try {
            const response = await AuthService.refresh()
            localStorage.setItem('token', response.data.access);
            this.setIsAuth(true);
            this.setRules(response.data.roles);
            //console.log(response);
        }
        catch (e) {
            //console.log(e);
        }
        finally {
            this.IsLoading = false;

        }
        
    }

}

