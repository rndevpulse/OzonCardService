import axios, { AxiosResponse } from "axios";
import api from "../http";
import { IFileResponse } from "../models/IFileResponse"


export default class FileService {


    static async getMyFiles(): Promise<AxiosResponse<IFileResponse[]>> {
        return api.get<IFileResponse[]>('/file/user')
    }

    static async removeFile(url: string): Promise<AxiosResponse<void>> {
        return api.get(`/file/remove/${url}`)
    }
    
    static async downloadFile(url: string, name: string){
        return api.get(`/file/get/${url}`, {responseType: 'blob'})
    }

    
    
}