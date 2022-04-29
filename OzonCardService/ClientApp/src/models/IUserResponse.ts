import { IOrganizationResponse } from "./IOrganizationResponse";

export interface IUserResponce {
    id: string
    mail: string
    organizations: IOrganizationResponse[]
}