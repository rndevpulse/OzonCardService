import { makeAutoObservable } from 'mobx';
import AuthService from '../services/AuthService';
import OrganizationService from "../services/OrganizationServise";



export default class LoginStore {
    Roles: string[] = [];
    IsAuth = false;
    IsLoading = false;
    public constructor() {
        makeAutoObservable(this);
    }
    setIsAuth(bool: boolean) {
        this.IsAuth = bool;
        LoginStore.isAuthenticated = bool;
    }

    static isAuthenticated : boolean = false;

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
            localStorage.setItem('refresh', response.data.refresh);
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
            localStorage.clear();
            // localStorage.removeItem('token');
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
            await AuthService.check()
            this.setRules(this.Roles);

        }
        catch (e) {
            const response = await AuthService.refresh()
            localStorage.setItem('token', response.data.access);
            localStorage.setItem('refresh', response.data.refresh);
            this.setRules(response.data.roles);
        }
        finally {
            this.IsLoading = false;
            this.setIsAuth(true);
        }
        
    }

}

