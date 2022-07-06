import { AxiosResponse } from "axios";
import api from "../http";
import { IInfoDataUpload } from "../models/IInfoDataUpload";

export default class TaskService {
    static async getTaskUpload(task: string): Promise<AxiosResponse<IInfoDataUpload>>{
        return await api.get<IInfoDataUpload>(`/tasks/${task}`)
    }
    static async cancelTask(task: string): Promise<AxiosResponse<IInfoDataUpload>> {
        return await api.get<IInfoDataUpload>(`/tasks/cancel/${task}`)
    }
}