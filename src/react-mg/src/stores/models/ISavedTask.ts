import {ITask} from "../../api/models/task/ITask";

export interface ISavedTask {
    id: string
    description: string
    time:number
    task:ITask
}