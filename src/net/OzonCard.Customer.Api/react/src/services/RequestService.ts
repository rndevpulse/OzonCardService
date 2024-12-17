import {AxiosResponse} from "axios";
import {IExtensionProperty, IHandlerInfo, IRequestModel} from "../models/request";
import api from "../api";
import {ITask} from "../models/task";


export default class RequestService {

    static async getHandlers(): Promise<AxiosResponse<IHandlerInfo[]>> {
        return api.get<IHandlerInfo[]>('/requests')
    }

    static async getHandlerProps(key:string): Promise<AxiosResponse<IExtensionProperty[]>> {
        return api.get<IExtensionProperty[]>(`/requests/${key}/properties`)
    }

    static async append(key:string, props:IRequestModel, title:string =""): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>(`/requests/${key}?title=${title}`, props)
    }
}