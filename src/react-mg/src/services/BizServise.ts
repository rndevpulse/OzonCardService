import { AxiosResponse } from "axios";
import api from "../api";
import {ICustomerOption} from "../api/models/biz/ICustomerOption";
import {IReportOption} from "../api/models/biz/IReportOption";
import {ISearchCustomer} from "../api/models/biz/ISearchCustomer";
import {ISearchCustomerModel} from "../api/models/biz/ISearchCustomerModel";
import {IChangeCustomerCategory} from "../api/models/biz/IChangeCustomerCategory";
import {IChangeCustomerBalance} from "../api/models/biz/IChangeCustomerBalance";
import {ITask} from "../api/models/task/ITask";

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
        return api.post<number>('/customer/change_balance', option)
    }


    static async ReportFromBiz(option: IReportOption): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>('/report/payments', option)
    }

    static async TransactionsFromBiz(option: IReportOption): Promise<AxiosResponse<ITask>> {
        return api.post<ITask>('/report/transactions', option)
    }




}