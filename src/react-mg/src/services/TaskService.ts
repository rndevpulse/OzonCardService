import { AxiosResponse } from "axios";
import api from "../api";
import {ITask} from "../api/models/task/ITask";

export default class TaskService {

    static async getTaskUpload(task: string): Promise<AxiosResponse<ITask>>{
        return await api.get<ITask>(`/tasks?id=${task}`)
    }
    static async cancelTask(task: string): Promise<void> {
        return await api.get(`/tasks/cancel?id=${task}`)
    }
}