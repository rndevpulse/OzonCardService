

export interface ISearchCustomerModel {
    id: string;
    bizId: string;
    name: string;
    card: string;
    tabNumber: string;
    organization: string;
    balance: number;
    sum: number;
    orders: string;
    categories: string[];
    lastVisit: Date;
}