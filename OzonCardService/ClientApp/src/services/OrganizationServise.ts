import { AxiosResponse } from "axios";
import api from "../http";
import { IOrganizationResponce } from "../models/IOrganizationResponse"

export default class OrganizationService {
    static async getMyOrganizations(): Promise<AxiosResponse<IOrganizationResponce[]>> {
        return api.get<IOrganizationResponce[]>('/')
    }
    
}