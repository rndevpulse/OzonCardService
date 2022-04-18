import { AxiosResponse } from "axios";
import api from "../http"
import { IAuthResponce } from "../models/IAuthResponse";

export default class AuthService {
    
    static async login(email: string, password: string): Promise<AxiosResponse<IAuthResponce>> {
        return api.post<IAuthResponce>('/auth', {email, password})
    }
    static async logout(token: string | undefined): Promise<void> {
        return api.post('/auth/logout', { token })
    }


    
}