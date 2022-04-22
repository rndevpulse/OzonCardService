import { IAdvancedOptionsResponse } from "./IAdvancedOptionsResponse";

export interface ICustomerOptionResponse {
    organizationId: string
    corporateNutritionId: string
    categoryId: string
    balance: number
    fileReport: string
    options: IAdvancedOptionsResponse
}