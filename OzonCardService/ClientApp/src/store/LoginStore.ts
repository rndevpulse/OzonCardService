import axios from 'axios';
import { makeAutoObservable } from 'mobx';
import { IAuthResponce } from '../models/IAuthResponse';
import AuthService from '../services/AuthService';



export default class LoginStore {
    email :string = '';
    isAuth = false;

    public constructor() {
        makeAutoObservable(this);
    }

    setIsAuth(bool: boolean) {
        this.isAuth = bool;
    }

    setMail(mail: string) {
        this.email = mail;
    }

    async login(email: string, password: string) {
        try {
            

            const response = await AuthService.login(email, password);
            
            localStorage.setItem('token', response.data.token);

            this.setIsAuth(true);
            this.setMail(response.data.email);
            console.log(response);
            console.log(this.email, this.isAuth);
        }
        catch (e) {
            console.log(e);
        }
    }

    async logout() {
        try {
            const responce = await AuthService.logout(undefined);
            localStorage.removeItem('token');
            this.setIsAuth(false);
            this.setMail("");
            console.log(responce);
        }
        catch (e) {
            console.log(e);
        }
    }

    async checkAuth() {
        try {
            const response = await axios.post<IAuthResponce>('https://localhost:5401/api/auth/refresh', { withCredentials: true })
            localStorage.setItem('token', response.data.token);
            this.setIsAuth(true);
            this.setMail(response.data.email);
            console.log(response);
        }
        catch (e)
        {
            console.log(e);
        }
        
    }

}

