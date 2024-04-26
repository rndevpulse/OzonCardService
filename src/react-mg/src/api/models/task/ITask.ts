
export interface ITask {
    id: string
    queuedAt : string
    completedAt? : string
    status : string
    error? : string
    progress : any
    comment: string
}