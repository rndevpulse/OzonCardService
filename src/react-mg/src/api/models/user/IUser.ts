import {IUserOrganization} from "./IUserOrganization";

export interface IUser {
    id: string
    mail: string
    organizations: IUserOrganization[]
}