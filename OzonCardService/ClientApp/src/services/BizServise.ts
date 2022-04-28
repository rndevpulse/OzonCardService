import { AxiosResponse } from "axios";
import api from "../http";
import { ICustomerOptionResponse } from "../models/ICustomerOptionResponse";
import { IReportOptionResponse } from "../models/IReportOptionResponse";

export default class BizService {
    static async upladCustomersToBiz(option: ICustomerOptionResponse): Promise<AxiosResponse<string>> {
        return api.post<string>('/customer/upload', option)
    }
    static async ReportFromBiz(option: IReportOptionResponse): Promise<AxiosResponse<string>> {
        return api.post<string>('/report', option)
    }

}