
export interface IInfoDataUpload extends IInfoData{
    countCustomersAll :number
    countCustomersNew: number
    countCustomersFail: number
    countCustomersBalance: number
    countCustomersCategory: number
    countCustomersCorporateNutritions: number

    isCompleted :boolean
    timeCompleted: string
}

export interface IInfoData {
    isCompleted: boolean
    timeCompleted: string
}

export interface ITask {
    taskId: string,
    deskription: string,
    isCompleted: boolean,
    taskInfo: IInfoDataUpload | undefined,
    created: string
}
