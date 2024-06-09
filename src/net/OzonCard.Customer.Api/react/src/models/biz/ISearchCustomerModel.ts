import {ICategory} from "../org/ICategory";


export interface ISearchCustomerModel {
    id: string
    bizId: string
    programId: string
    name: string
    card: string
    tabNumber: string
    position: string
    division: string
    organization: string
    balance: number
    sum: number
    orders: number
    daysGrant: number
    categories: ICategory[]
    lastVisit: Date
    updated: Date
}