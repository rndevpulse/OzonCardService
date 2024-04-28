import { AxiosResponse } from "axios";
import api from "../api"
import {IAuth} from "../models/auth/IAuth";
import {IUser} from "../models/user/IUser";

export default class AuthService {
    
    static async login(email: string, password: string): Promise<AxiosResponse<IAuth>> {
        return api.post<IAuth>('/auth/login', {email, password})
    }
    static async logout(token: string | undefined): Promise<void> {
        return api.post('/auth/logout', { token })
    }
    static async refresh(): Promise<AxiosResponse<IAuth>> {
        return api.get('/auth/refresh')
    }

    static async create(email: string, password: string, roles:string[]): Promise<AxiosResponse<IUser>>{
        return await api.post<IUser>('/account', { email, password, roles})
    }


    static async getUsers(): Promise<AxiosResponse<IUser[]>> {
        return await api.get<IUser[]>(`/account`)
    }
}