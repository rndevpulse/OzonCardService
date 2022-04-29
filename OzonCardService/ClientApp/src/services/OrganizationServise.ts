import { AxiosResponse } from "axios";
import api from "../http";
import { IOrganizationResponse } from "../models/IOrganizationResponse"

export default class OrganizationService {
    static async getMyOrganizations(): Promise<AxiosResponse<IOrganizationResponse[]>> {
        return api.get<IOrganizationResponse[]>('/organization/list')
    }
    static async updateOrganization(organizationId: string): Promise<AxiosResponse<IOrganizationResponse>> {
        return api.get<IOrganizationResponse>(`/organization/${organizationId}/update`)
    }
    static async createOrganization(email: string, password: string): Promise<AxiosResponse<IOrganizationResponse>> {
        return api.post<IOrganizationResponse>('/organization/create', { email, password})
    }
}