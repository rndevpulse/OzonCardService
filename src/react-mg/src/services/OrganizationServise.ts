import { AxiosResponse } from "axios";
import api from "../api";
import {IOrganization} from "../api/models/org/IOrganization";
import {IUser} from "../api/models/user/IUser";

export default class OrganizationService {
    static async getMyOrganizations(): Promise<AxiosResponse<IOrganization[]>> {
        return api.get<IOrganization[]>('/organization')
    }

    static async updateOrganization(organizationId: string): Promise<AxiosResponse<IOrganization>> {
        return api.get<IOrganization>(`/organization/${organizationId}/update`)
    }

    static async createOrganization(email: string, password: string): Promise<AxiosResponse<IOrganization>> {
        return api.post<IOrganization>('/organization', { email, password})
    }

    static async getUsers(organizationId: string): Promise<AxiosResponse<IUser[]>> {
        return await api.get<IUser[]>(`/organization/${organizationId}/members`)
    }

    static async addUserForOrganization(organizationId: string, userId: string): Promise<AxiosResponse<IUser>> {
        return api.post<IUser>(`/organization/${organizationId}/members?userId=${userId}`)
    }
    static async delUserForOrganization(organizationId: string, userId: string): Promise<AxiosResponse<IUser>> {
        return api.delete<IUser>(`/organization/${organizationId}/members?userId=${userId}`)
    }
}