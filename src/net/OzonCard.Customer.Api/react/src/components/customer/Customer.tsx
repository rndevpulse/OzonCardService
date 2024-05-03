import {ISearchCustomerModel} from "../../models/biz/ISearchCustomerModel";
import React, {useContext} from "react";
import {ModalContext} from "../../context/modal";
import {Modal} from "../modal";
import {ChangeCustomer} from "./ChangeCustomer";
import {IOrganization} from "../../models/org/IOrganization";


interface ICustomerProps{
    customer:ISearchCustomerModel
    organization: IOrganization
    onChange : (customer:ISearchCustomerModel) => void
    onRemove : (customer:ISearchCustomerModel) => void
}

export function Customer({customer, organization, onChange, onRemove} : ICustomerProps) {

    const {modal, open, close}=useContext(ModalContext)

    // const div_ChangeCustomerCategory = () => {
    //     return (<div>
    //         <label>Добавить или удалить выбранные категории</label>
    //         <span>
    //                 <button className="button"
    //                         onClick={() => onChangeCategory(customer.bizId, customer.name, false)}>
    //                     Добавить
    //                 </button>
    //                 <button className="button red"
    //                         onClick={() => onChangeCategory(customer.bizId, customer.name, true)}>
    //                     Удалить
    //                 </button>
    //             </span>
    //     </div>)
    // }
    // const div_ChangeCustomerBalance = () => {
    //     return (<div>
    //         <label>Изменение баланса</label>
    //         <button className="button"
    //                 onClick={() => onChangeBalance(customer.bizId, customer.name)}>
    //             Установить
    //         </button>
    //     </div>)
    // }

    const onChangeHandler = (customer:ISearchCustomerModel) => {
        close()
        onChange(customer)
    }
    const onRemoveHandler = (customer:ISearchCustomerModel) => {
        close()
        onRemove(customer)
    }

    return (
        <>
            {modal && <Modal title={'Редактирование сотрудника'} onClose={close}>
                <ChangeCustomer
                    customer={customer}
                    categories={organization.categories}
                    onChange={onChangeHandler}
                    onRemove={onRemoveHandler}
                />
            </Modal>}
            <li key={customer.id} onClick={open}>
                <dt>{customer.name}</dt>
                {/*{div_ChangeCustomerCategory()}*/}
                {/*{div_ChangeCustomerBalance()}*/}
                <dd>
                    <ul>
                        <li>Организация: {customer.organization}</li>
                        <li>Карта: {customer.card}</li>
                        <li>Табельный №: {customer.tabNumber}</li>
                        <li>Баланс: {customer.balance}</li>
                        <li>Сумма заказов: {customer.sum}</li>
                        <li>Количество заказов: {customer.orders}</li>
                    </ul>
                    <ul>
                        <li>Категории:</li>
                        {customer.categories && customer.categories.map(category => {
                            return (<li>{category.name}</li>)
                        })}
                        <li>Последний визит: {getLastVisit(customer.lastVisit)}</li>
                    </ul>
                </dd>
            </li>
        </>

    )
}

function getLastVisit(value:Date):string{
    const dt = new Date(value)
    if (dt.getFullYear() < 2000)
        return "";
    return `${dt.toLocaleDateString()} ${dt.toLocaleTimeString()}`;
}