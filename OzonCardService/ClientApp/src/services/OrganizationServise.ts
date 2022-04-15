import api from "../http";
import { IAuthResponce } from "../models/AuthResponse";

export default class OrganizationService {
    static async getMyOrganizations(): Promise<IAuthResponce> {
        return api.get<IOrganizationResponse[]>
    }
    
}