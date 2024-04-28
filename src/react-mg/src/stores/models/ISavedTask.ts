import {ITask} from "../../models/task/ITask";

export interface ISavedTask {
    id: string
    description: string
    time:number
    task:ITask
}