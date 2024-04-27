import { AxiosResponse } from "axios";
import api from "../api";
import {ITask} from "../api/models/task/ITask";

export default class TaskService {

    static async getTasks(id: string[]): Promise<AxiosResponse<ITask[]>>{
        return await api.get<ITask[]>(`/tasks`,{
            params: { id: id },
            paramsSerializer:{
                indexes:null
            }
        })
    }
    static async cancelTask(task: string): Promise<AxiosResponse<ITask|undefined>> {
        return await api.get<ITask|undefined>(`/tasks/cancel?id=${task}`)
    }
}