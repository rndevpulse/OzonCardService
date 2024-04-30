export interface IReportOption{
    organizationId :string
    programId: string
    categoriesId: string[]
    dateFrom: Date
    dateTo: Date
    title: string
    isOffline:boolean
}
