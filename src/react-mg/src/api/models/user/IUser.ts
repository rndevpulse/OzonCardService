import {IUserOrganization} from "./IUserOrganization";

export interface IUser {
    id: string
    email: string
    organizations: IUserOrganization[]
}