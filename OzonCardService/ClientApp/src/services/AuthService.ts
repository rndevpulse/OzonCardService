import { AuthResponce } from "../models/AuthResponse";

export default class AuthService {
    static async login(username: string, password: string): Promise<AuthResponce> {
        const reqOption = {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ login: username, password: password })
        }
        return await fetch(`api/auth`, reqOption)
            .then(response => response.json() as Promise<AuthResponce>)
    }
}