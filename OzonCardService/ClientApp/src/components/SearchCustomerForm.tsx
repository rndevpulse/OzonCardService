import { FC, useContext, useState } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Context } from '..';
import axios from 'axios';
import BizService from '../services/BizServise';
import '../css/SearchForm.css'
import { IInfoSearhCustomerResponse } from '../models/IInfoSearhCustomerResponse';
import { ISearchCustomer } from '../models/ISearchCustomer';



const SearchCustomerForm: FC = () => {

    const { organizationstore } = useContext(Context);
    const [organizationId, setOrganizationId] = useState('');

    const [customerName, setCustomerName] = useState('');
    const [customerCard, setCustomerCard] = useState('');

    const [customersInfo, setCustomersInfo] = useState<IInfoSearhCustomerResponse[]>([]);
    

    const CustomSelect = ({ id, value, options, onChange }) => {
        return (
            <select className="custom-select" id={id} value={value} onChange={onChange}>
                {options.map(option =>
                    <option key={option.id} value={option.id}>{option.name}</option>
                )}
            </select>
        )
    }
    const onOrganizationSelectChange = (e) => {
        const orgId = e.target.options[e.target.selectedIndex].value
        const organization = organizationstore.organizations.find(org => org.id === orgId);

        setOrganizationId(organization?.id ?? '')
    }
    async function firstInit() {
        await organizationstore.requestOrganizations();
        setOrganizationId(organizationstore.organizations[0]?.id ?? '');

        organizationstore.setLoading(false);
        console.log('isLoading false')

    }

    async function getCustomers(name: string, card:string) {
        const response = await BizService.SearchCustomerFromBiz({
            name,
            card,
            organizationId
        })
        console.log('customers: ', response.data)
        setCustomersInfo(response.data)
    }

    function onChangeCustomerName(value: string) {
        setCustomerName(value)
        if (value.length > 4) {
            getCustomers(value, customerCard)
        } else if (value.length == 0 && customerCard.length == 0) {
            setCustomersInfo([])
        }
    }
    function onChangeCustomerCard(value: string) {
        setCustomerCard(value)
        if (value.length > 5) {
            getCustomers(customerName, value)
        }
        else if (value.length == 0 && customerName.length == 0) {
            setCustomersInfo([])
        }
    }
    useEffect(() => {
        firstInit();
        console.log('SearchCustomerForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return <h1>Loading...</h1>
    }

    return (

        <div>
            <h1>Поиск сотрудника</h1>

            <div className="form-group col-md-8">
                <label htmlFor="organizations">Организации</label>
                <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations}
                    onChange={onOrganizationSelectChange} />
                <label className="search_label" htmlFor="customerName">ФИО сотрудника
                    <input 
                        id='customerName'
                        onChange={e => onChangeCustomerName(e.target.value)}
                        value={customerName}
                        type='text'
                        placeholder='Фамилия Имя Отчество'
                    /></label>
                <label className="search_label" htmlFor="customerCard">Карта сотрудника
                    <input
                        id='customerCard'
                        onChange={e => onChangeCustomerCard(e.target.value)}
                        value={customerCard}
                        type='text'
                        placeholder='xxxxxxxx'
                        /></label>
            </div>
            <div className="search form-group col-md-8">
                <ul>
                    {customersInfo && customersInfo.map(customer => {
                        return (
                            <li key={customer.id}>
                                <dt>{customer.name}</dt>
                                <dd>
                                    <ul>
                                        <li>Организация: {customer.organization}</li>
                                        <li>Карта: {customer.card}</li>
                                        <li>Баланс: {customer.balanse}</li>
                                        <li>Сумма заказов: {customer.sum}</li>
                                        <li>Количество заказов: {customer.orders}</li>
                                    </ul>
                                    <ul>
                                        <li>Категории:</li>
                                        {customer.categories && customer.categories.map(category => {
                                            return (<li>{category}</li>)
                                        })}
                                    </ul>
                                </dd>
                            </li>
                        )
                    })}
                </ul>

            </div>

        </div>

    );
};

export default observer(SearchCustomerForm);
