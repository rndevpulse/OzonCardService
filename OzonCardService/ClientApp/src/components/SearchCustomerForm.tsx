import { FC, useContext, useState } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Context } from '..';
import BizService from '../services/BizServise';
import '../css/SearchForm.css'
import { IInfoSearhCustomerResponse } from '../models/IInfoSearhCustomerResponse';
import { ICorporateNutritionResponse } from '../models/ICorporateNutritionResponse';
import { ICategoryResponse } from '../models/ICategoryResponse';



const SearchCustomerForm: FC = () => {

    const { organizationstore } = useContext(Context);
    const [organizationId, setOrganizationId] = useState('');
    const [corporateNutritions, setCorporateNutritions] = useState<ICorporateNutritionResponse[]>([]);
    const [corporateNutritionId, setCorporateNutritionId] = useState('');
    const [categoryId, setCategoryId] = useState('');
    const [categories, setCategories] = useState<ICategoryResponse[]>([]);

    const [customerName, setCustomerName] = useState('');
    const [customerCard, setCustomerCard] = useState('');

    const [customersInfo, setCustomersInfo] = useState<IInfoSearhCustomerResponse[]>([]);
    const [isLoadCustomers, setIsLoadCustomers] = useState(false);

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
        setCategories(organization?.categories ?? [])
        setCategoryId(organization?.categories[0]?.id ?? '')
        setCorporateNutritions(organization?.corporateNutritions ?? [])
        setCorporateNutritionId(organization?.corporateNutritions[0]?.id ?? '')

    }
    async function firstInit() {
        await organizationstore.requestOrganizations();
        setOrganizationId(organizationstore.organizations[0]?.id ?? '');
        setCorporateNutritions(organizationstore.organizations[0]?.corporateNutritions ?? [])
        setCorporateNutritionId(organizationstore.organizations[0]?.corporateNutritions[0]?.id ?? '')
        setCategories(organizationstore.organizations[0]?.categories ?? []);
        setCategoryId(organizationstore.organizations[0]?.categories[0]?.id ?? '');

        organizationstore.setLoading(false);
        //console.log('isLoading false')

    }

    async function getCustomers(name: string, card: string) {
        setIsLoadCustomers(true)
        const response = await BizService.SearchCustomerFromBiz({
            name,
            card,
            organizationId,
            corporateNutritionId
        })
        //console.log('customers: ', response.data)
        setCustomersInfo(response.data)
        setIsLoadCustomers(false)

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
    async function ChangeCustomerCategory(id: string, name: string, isRemove: boolean) {
        const catName = categories.filter(x => x.id === categoryId)[0].name
        if (!isRemove && customersInfo.find(x => x.id === id)?.categories.includes(catName)
            || isRemove && !customersInfo.find(x => x.id === id)?.categories.includes(catName)) {
            return
        }

        await BizService.ChangeCustomerBizCategory({
            id,
            organizationId,
            categoryId,
            isRemove
        })
        setIsLoadCustomers(true)

        if (isRemove) {
            const customer_categories = customersInfo.find(x => x.id === id)?.categories.filter(x => x !== catName)
            const new_arr = customersInfo
            console.log(customer_categories)
            if (customer_categories)
                new_arr.find(x => x.id === id)!.categories = customer_categories
            console.log(new_arr)
            setCustomersInfo(new_arr)
            confirm(`У пользователя "${name}" удалена указанная категория`)
        }
        else {
            const new_arr = customersInfo
            new_arr.find(x => x.id === id)?.categories.push(catName)
            
            console.log(new_arr)
            setCustomersInfo(new_arr)
            confirm(`Пользователю "${name}" добавлена указанная категория`)
        }
        setIsLoadCustomers(false)


    }
    
    function getCustomersInfo() {
        //console.log("getCustomersInfo length", customersInfo.length)
        if (isLoadCustomers) {
            return <div className="center">Идет поиск...</div>
        }
        if (customersInfo.length === 0) {
            return <div className="center">Нет результатов</div>
        }
        return (
            <div className="center search form-group col-md-12">
                <ul>
                    {customersInfo && customersInfo.map(customer => {
                        return (
                            <li key={customer.id}>
                                <dt>{customer.name}</dt>
                                <div>
                                    <label>Добавить или удалить выбранную категорию</label>
                                    <span>
                                        <button className="button"
                                            onClick={() => ChangeCustomerCategory(customer.id, customer.name, true)}>
                                            Удалить
                                        </button>
                                        <button className="button"
                                            onClick={() => ChangeCustomerCategory(customer.id, customer.name, false)}>
                                            Добавить
                                        </button>
                                    </span>
                                </div>
                                <dd>
                                    <ul>
                                        <li>Организация: {customer.organization}</li>
                                        <li>Карта: {customer.card}</li>
                                        <li>Табельный №: {customer.tabNumber}</li>
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
        )
    }
    useEffect(() => {
        firstInit();
        //console.log('SearchCustomerForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return <h1>Loading...</h1>
    }

    return (

        <div>
            <h1 className="center form-group col-md-12">Поиск сотрудника</h1>

            <div className="center form-group col-md-12">
                <label htmlFor="organizations">Организации</label>
                <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations}
                    onChange={onOrganizationSelectChange} />
                <label htmlFor="corporateNutritions">Программы питания</label>
                <CustomSelect id="corporateNutritions" value={corporateNutritionId} options={corporateNutritions}
                    onChange={event => setCorporateNutritionId(event.target.value)} />

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
                <br/>
                <label htmlFor="categories">Укажите категорию для добавления/удаления у пользователя</label>
                <CustomSelect id="categories" value={categoryId} options={categories} onChange={event => setCategoryId(event.target.value)} />
            </div>
            {getCustomersInfo()}
            

        </div>

    );
};

export default observer(SearchCustomerForm);
