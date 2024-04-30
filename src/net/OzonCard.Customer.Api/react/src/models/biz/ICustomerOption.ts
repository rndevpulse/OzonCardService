
export interface ICustomerOption {
    organizationId: string
    programId: string
    categoriesId: string[]
    balance: number
    fileReport: string
    options: IAdvancedOptions,
    customer: ICustomer | null
}
export interface ICustomer {
    name: string,
    card: string
}

export interface IAdvancedOptions {
    refreshBalance: boolean;
    rename: boolean;
}