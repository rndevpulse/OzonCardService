import { AxiosResponse } from "axios";
import api from "../api";
import {ICustomerOption} from "../models/biz/ICustomerOption";
import {IReportOption} from "../models/biz/IReportOption";
import {ISearchCustomer} from "../models/biz/ISearchCustomer";
import {ISearchCustomerModel} from "../models/biz/ISearchCustomerModel";
import {IChangeCustomerCategory} from "../models/biz/IChangeCustomerCategory";
import {IChangeCustomerBalance} from "../models/biz/IChangeCustomerBalance";
import {ITask} from "../models/task/ITask";

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


    static async ReportFromBiz(option: IReportOption): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>('/report/payments', option)
    }

    static async TransactionsFromBiz(option: IReportOption): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>('/report/transactions', option)
    }




}