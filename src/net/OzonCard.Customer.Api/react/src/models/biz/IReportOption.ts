export interface IReportOption{
    organizationId :string
    programId: string
    categoriesId: string[]
    dateFrom: Date
    dateTo: Date
    offset: number
    title: string
    isOffline:boolean
    batch: string | undefined
}
