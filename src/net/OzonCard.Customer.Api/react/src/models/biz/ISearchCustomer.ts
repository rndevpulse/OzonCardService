
export interface ISearchCustomer {
    name: string;
    card: string;
    organizationId: string;
    programId: string;
    dateFrom: Date;
    dateTo: Date;
    offset: number
    isOffline: boolean;
}