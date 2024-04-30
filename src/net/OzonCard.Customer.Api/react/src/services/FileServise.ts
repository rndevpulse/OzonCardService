import { AxiosResponse } from "axios";
import api from "../api";
import {IFile} from "../models/file/IFile";


export default class FileService {


    static async getMyFiles(): Promise<AxiosResponse<IFile[]>> {
        return api.get<IFile[]>('/file')
    }

    static async removeFile(url: string): Promise<AxiosResponse<void>> {
        return api.post(`/file/remove/${url}`)
    }
    
    static async downloadFile(url: string){
        return api.get(`/file/${url}`, {responseType: 'blob'})
    }

    static async createFile(formData: FormData) : Promise<AxiosResponse<IFile>>{
        const config = {
            headers: {
                'content-type': 'multipart/form-data',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        }
        return api.post<IFile>('/file', formData, config)
    }
    
}