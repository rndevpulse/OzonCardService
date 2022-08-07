import { AxiosResponse } from "axios";
import api from "../http";
import { IChangeCustomerBalance } from "../models/IChangeCustomerBalance";
import { IChangeCustomerCategory } from "../models/IChangeCustomerCategory";
import { ICustomerOptionResponse } from "../models/ICustomerOptionResponse";
import { IInfoSearhCustomerResponse } from "../models/IInfoSearhCustomerResponse";
import { IReportOptionResponse } from "../models/IReportOptionResponse";
import { ISearchCustomer } from "../models/ISearchCustomer";

export default class BizService {
    static async upladCustomersToBiz(option: ICustomerOptionResponse): Promise<AxiosResponse<string>> {
        return api.post<string>('/customer/upload', option)
    }
    static async ReportFromBiz(option: IReportOptionResponse): Promise<AxiosResponse<string>> {
        return api.post<string>('/report', option)
    }
    static async TransactionsFromBiz(option: IReportOptionResponse): Promise<AxiosResponse<string>> {
        return api.post<string>('/report/transactions', option)
    }
    static async SearchCustomerFromBiz(option: ISearchCustomer): Promise<AxiosResponse<IInfoSearhCustomerResponse[]>> {
        return api.post<IInfoSearhCustomerResponse[]>('/customer/search', option)
    }
    static async ChangeCustomerBizCategory(option: IChangeCustomerCategory): Promise<AxiosResponse<void>> {
        return api.post<void>('/customer/change_category', option)
    }
    static async ChangeCustomerBizBalance(option: IChangeCustomerBalance): Promise<AxiosResponse<number>> {
        return api.post<number>('/customer/change_balance', option)
    }
}