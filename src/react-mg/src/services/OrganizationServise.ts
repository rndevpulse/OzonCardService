import { AxiosResponse } from "axios";
import api from "../api";
import {IOrganization} from "../api/models/org/IOrganization";
import {IUser} from "../api/models/user/IUser";

export default class OrganizationService {
    static async getMyOrganizations(): Promise<AxiosResponse<IOrganization[]>> {
        return api.get<IOrganization[]>('/organization')
    }

    static async updateOrganization(organizationId: string): Promise<AxiosResponse<IOrganization>> {
        return api.put<IOrganization>(`/organization/${organizationId}`)
    }

    static async createOrganization(login: string, password: string): Promise<AxiosResponse<IOrganization>> {
        return api.post<IOrganization>(`/organization?login=${login}&password=${password}`)
    }


    static async addUserForOrganization(organizationId: string, userId: string): Promise<AxiosResponse<IUser>> {
        return api.post<IUser>(`/organization/${organizationId}/members?userId=${userId}`)
    }
    static async delUserForOrganization(organizationId: string, userId: string): Promise<AxiosResponse<IUser>> {
        return api.delete<IUser>(`/organization/${organizationId}/members?userId=${userId}`)
    }
}