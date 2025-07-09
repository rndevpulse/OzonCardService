import { makeAutoObservable } from 'mobx';
import AuthService from '../services/AuthService';


export default class LoginStore {
    Roles: string[] = JSON.parse(localStorage.getItem('roles') ?? '[]');
    IsLoading = false;
    public constructor() {
        makeAutoObservable(this);
    }
    isTokenExpired = (): boolean => {
        return localStorage.getItem('token') === null;
        // try {
        //     const now = Date.now();
        //     const value = localStorage.getItem('expired');
        //     const expired = value ? Date.parse(value) : now;
        //     console.log('expired ', expired);
        //     console.log('now ', now);
        //     return now >= expired;
        //     // if (now >= expired){
        //     //     try{
        //     //         AuthService.refresh().then(response=>{
        //     //             localStorage.setItem('token', response.data.access);
        //     //             localStorage.setItem('refresh', response.data.refresh);
        //     //             localStorage.setItem('expired', response.data.expired.toString());
        //     //         })
        //     //         return false;
        //     //     }
        //     //     catch (error) {
        //     //         return true;
        //     //     }
        //     // }
        //     // return true;
        // } catch {
        //     return true;
        // }
    };


    setLoading(bool: boolean) {
        this.IsLoading = bool;
    }


    async login(email: string, password: string) {
        try {
            this.setLoading(true);
            const response = await AuthService.login(email, password);
            localStorage.setItem('token', response.data.access);
            localStorage.setItem('refresh', response.data.refresh);
            localStorage.setItem('roles', JSON.stringify(response.data.roles));
            this.Roles = response.data.roles;
            //console.log(response);
        }
        catch (e) {
            //console.log(e);
        }
        this.setLoading(false);
    }

    async logout() {
        try {
            await AuthService.logout();
            // localStorage.removeItem('token');
            //console.log(responce);
        }
        finally {
            localStorage.clear();
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

