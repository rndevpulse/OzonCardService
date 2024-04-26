import { AxiosResponse } from "axios";
import api from "../api"
import {IAuth} from "../api/models/auth/IAuth";
import {IUser} from "../api/models/user/IUser";

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

    static async create(email: string, password: string, roles:number[]): Promise<AxiosResponse<IUser>>{
        return await api.post<IUser>('/account', { email, password, roles})
    }
}