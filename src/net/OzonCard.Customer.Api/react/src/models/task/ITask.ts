import {IProgress} from "./IProgress";

export interface ITask {
    id: string
    queuedAt : string
    completedAt? : string
    status : "Enqueued"|"Processing"|"Deleted"|"Failed"|"Scheduled"|"Succeeded"
    error? : string
    progress : IProgress | undefined,
    result: any
}

