import { AxiosResponse } from "axios";
import api from "../http";
import { ICustomerOptionResponse } from "../models/ICustomerOptionResponse";

export default class BizService {
    static async upladCustomersToBiz(option: ICustomerOptionResponse): Promise<AxiosResponse<string>> {
        return api.post<string>('/customer/upload', option)
    }
    
}