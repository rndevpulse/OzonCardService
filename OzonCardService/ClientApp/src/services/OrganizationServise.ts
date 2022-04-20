import { AxiosResponse } from "axios";
import api from "../http";
import { IOrganizationResponse } from "../models/IOrganizationResponse"

export default class OrganizationService {
    static async getMyOrganizations(): Promise<AxiosResponse<IOrganizationResponse[]>> {
        return api.get<IOrganizationResponse[]>('/organization/list')
    }
    
}