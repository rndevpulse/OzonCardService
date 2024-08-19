import {AxiosResponse} from "axios";
import api from "../api";
import {IBatch} from "../models/batch";

export default class PropsService {

    static async getBatches(): Promise<AxiosResponse<IBatch[]>> {
        return await api.get<IBatch[]>(`/props`)
    }

    static async setBatch(batch: IBatch) : Promise<AxiosResponse<IBatch>>{
        return await api.post<IBatch>(`/props`,batch)
    }
}