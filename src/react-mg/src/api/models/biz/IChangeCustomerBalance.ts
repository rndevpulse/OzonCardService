export interface IChangeCustomerBalance {
    id: string
    organizationId: string
    corporateNutritionId: string
    isIncrement: boolean
    balance:number
}