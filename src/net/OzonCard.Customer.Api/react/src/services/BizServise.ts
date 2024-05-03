import { AxiosResponse } from "axios";
import api from "../api";
import {
    IChangeCustomer,
    IChangeCustomerBalance,
    IChangeCustomerCategory,
    ICustomerOption, IReportOption,
    ISearchCustomer,
    ISearchCustomerModel
} from "../models/biz";
import {ITask} from "../models/task";

export default class BizService {

    static async uploadCustomersToBiz(option: ICustomerOption): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>('/customer', option)
    }

    static async SearchCustomerFromBiz(option: ISearchCustomer): Promise<AxiosResponse<ISearchCustomerModel[]>> {
        return api.post<ISearchCustomerModel[]>('/customer/search', option)
    }

    static async ChangeCustomerBizCategory(option: IChangeCustomerCategory): Promise<AxiosResponse<string>> {
        return api.post<string>('/customer/category', option)
    }

    static async ChangeCustomerBizBalance(option: IChangeCustomerBalance): Promise<AxiosResponse<number>> {
        return api.post<number>('/customer/balance', option)
    }

    static async RemoveCustomer(id: string): Promise<AxiosResponse<ISearchCustomerModel>> {
        return api.delete<ISearchCustomerModel>(`/customer/${id}`)
    }

    static async UpdateCustomer(option:IChangeCustomer): Promise<AxiosResponse<ISearchCustomerModel>> {
        return api.put<ISearchCustomerModel>('/customer', option)
    }


    static async ReportFromBiz(option: IReportOption): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>('/report/payments', option)
    }

    static async TransactionsFromBiz(option: IReportOption): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>('/report/transactions', option)
    }




}