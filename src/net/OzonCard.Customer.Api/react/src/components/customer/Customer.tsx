import {ISearchCustomerModel} from "../../models/biz/ISearchCustomerModel";
import React from "react";
import {IOrganization} from "../../models/org/IOrganization";


interface ICustomerProps{
    customer:ISearchCustomerModel
    organization: IOrganization
    onChanging : (customer:ISearchCustomerModel, organization:IOrganization) => void
}

export function Customer({customer, organization, onChanging} : ICustomerProps) {




    return (
        <>
           <li key={customer.id} onClick={()=>onChanging(customer, organization)}>
                <dt>{customer.name}</dt>
                <dd>
                    <ul>
                        <li>Организация: {customer.organization}</li>
                        <li>Карта: {customer.card}</li>
                        <li>Табельный №: {customer.tabNumber}</li>
                        <li>Баланс: {customer.balance}</li>
                        <li>Сумма заказов: {customer.sum ?? 0}</li>
                        <li>Количество заказов: {customer.orders ?? 0}</li>
                        <li>Количество дней питания: {customer.daysGrant ?? 0}</li>
                    </ul>
                    <ul>
                        <li>Категории:</li>
                        {customer.categories && customer.categories.map(category => {
                            return (<li>{category.name}</li>)
                        })}
                        <li>Последний визит: {getTime(customer.lastVisit)}</li>
                        <li>Зарегистрирован: {getTime(customer.createdAt)}</li>
                        <li>Обновлено: {getTime(customer.updated)}</li>
                    </ul>
                </dd>
            </li>
        </>

    )
}

function getTime(value:Date):string{
    const dt = new Date(value)
    if (dt.getFullYear() < 2000)
        return "";
    return `${dt.toLocaleDateString()} ${dt.toLocaleTimeString()}`;
}