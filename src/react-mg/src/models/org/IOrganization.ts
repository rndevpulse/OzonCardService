import {IProgram} from "./IProgram";
import {ICategory} from "./ICategory";


export interface IOrganization {
    id: string
    name: string
    categories: ICategory[]
    programs: IProgram[]
}