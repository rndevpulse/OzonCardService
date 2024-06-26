﻿import { AxiosResponse } from "axios";
import api from "../http";
import { IInfoDataUpload } from "../models/IInfoDataUpload";
import { IUserResponce } from "../models/IUserResponse";

export default class UserService {
    static async createUser(email: string, password: string, rules:number[]): Promise<void>{
        return await api.post('/user/create', { email, password, rules})
    }

    static async getUsers(): Promise<AxiosResponse<IUserResponce[]>> {
        return await api.get('/user/list')
    }

    static async addUserForOrganization(organizationId: string, userId: string): Promise<AxiosResponse<boolean>> {
        return api.get<boolean>(`/user/${userId}/add_organization/${organizationId}`)
    }
    static async delUserForOrganization(organizationId: string, userId: string): Promise<AxiosResponse<boolean>> {
        return api.get<boolean>(`/user/${userId}/del_organization/${organizationId}`)
    }
}