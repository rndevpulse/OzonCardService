import {IProgress} from "./IProgress";

export interface ITask {
    id: string
    queuedAt : string
    completedAt? : string
    status : string
    error? : string
    progress : IProgress | undefined,
    result: any
}

