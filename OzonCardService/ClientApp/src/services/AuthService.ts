import { AxiosResponse } from "axios";
import { IAuthResponce } from "../models/AuthResponse";
import api from "../http"

export default class AuthService {
    //static async login(emal: string, password: string): Promise<AuthResponce> {
    //    const reqOption = {
    //        method: "POST",
    //        headers: { 'Content-Type': 'application/json' },
    //        body: JSON.stringify({ login: emal, password: password })
    //    }
    //    return await fetch('api/auth', reqOption)
    //        .then(response => response.json() as Promise<AuthResponce>)
    //}
    //static async logout(token: string): Promise<void> {
    //    const reqOption = {
    //        method: "POST",
    //        headers: { 'Content-Type': 'application/json' },
    //        body: JSON.stringify({ token: token})
    //    }
    //    return await fetch('api/auth/logout', reqOption)
    //        .then(response => response.json() as Promise<void>)
    //}

    static async login(email: string, password: string): Promise<AxiosResponse<IAuthResponce>> {
        return api.post<IAuthResponce>('/auth', {email, password})
    }
    static async logout(token: string): Promise<void> {
        return api.post('/auth/logout', { token })
    }
}