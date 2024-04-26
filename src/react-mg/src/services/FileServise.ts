import axios, { AxiosResponse } from "axios";
import api from "../api";
import {IFile} from "../api/models/file/IFile";


export default class FileService {


    static async getMyFiles(): Promise<AxiosResponse<IFile[]>> {
        return api.get<IFile[]>('/file/user')
    }

    static async removeFile(url: string): Promise<AxiosResponse<void>> {
        return api.get(`/file/remove/${url}`)
    }
    
    static async downloadFile(url: string){
        return api.get(`/file/get/${url}`, {responseType: 'blob'})
    }

    static async createFile(formData: FormData) : Promise<AxiosResponse<IFile>>{
        const config = {
            headers: {
                'content-type': 'multipart/form-data',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        }
        return api.post<IFile>('/api/file', formData, config)
    }
    
}