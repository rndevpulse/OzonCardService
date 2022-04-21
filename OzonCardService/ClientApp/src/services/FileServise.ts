import { AxiosResponse } from "axios";
import api from "../http";
import { IFileResponse } from "../models/IFileResponse"

export default class FileService {
    static async getMyFiles(): Promise<AxiosResponse<IFileResponse[]>> {
        return api.get<IFileResponse[]>('/file/user')
    }
    
}