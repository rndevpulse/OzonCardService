
export interface ITask {
    id: string
    queuedAt : string
    completedAt? : string
    status : string
    error? : string
    progress : any
}

export interface ICustomersTasksProgress {
    countAll :number
    countNew: number
    countFail: number
    countBalance: number
    countCategory: number
    countProgram: number
}