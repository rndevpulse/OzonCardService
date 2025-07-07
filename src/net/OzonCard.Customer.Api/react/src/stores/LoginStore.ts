import { makeAutoObservable } from 'mobx';
import AuthService from '../services/AuthService';



export default class LoginStore {
    Roles: string[] = JSON.parse(localStorage.getItem('roles') ?? '[]');
    IsLoading = false;
    IsAuth = !!localStorage.getItem('token');
    public constructor() {
        makeAutoObservable(this);
    }



    setLoading(bool: boolean) {
        this.IsLoading = bool;
    }



    async login(email: string, password: string) {
        try {
            const response = await AuthService.login(email, password);
            localStorage.setItem('token', response.data.access);
            localStorage.setItem('refresh', response.data.refresh);
            localStorage.setItem('roles', JSON.stringify(response.data.roles));
            this.IsAuth = true;
            this.Roles = response.data.roles;
            //console.log(response);
        }
        catch (e) {
            //console.log(e);
        }
    }

    async logout() {
        try {
            await AuthService.logout();
            // localStorage.removeItem('token');
            //console.log(responce);
        }
        finally {
            localStorage.clear();
            this.IsAuth = false;
        }
    }

    async checkAuth() {
        this.IsLoading = true;
        try {
            await AuthService.check()

        }
        catch (e) {
            const response = await AuthService.refresh()
            localStorage.setItem('token', response.data.access);
            localStorage.setItem('refresh', response.data.refresh);
            localStorage.setItem('roles', JSON.stringify(response.data.roles));
        }
        finally {
            this.IsLoading = false;
        }
        
    }

}

