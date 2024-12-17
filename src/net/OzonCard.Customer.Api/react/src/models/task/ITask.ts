import {IProgress} from "./IProgress";

export interface ITask {
    id: string
    queuedAt : Date
    completedAt : Date | undefined
    processedAt : Date | undefined
    status : "Enqueued"|"Processing"|"Deleted"|"Failed"|"Scheduled"|"Succeeded"
    error? : string
    title? : string
    progress : IProgress | undefined,
    result: any
}

