import { FC, useContext, useState } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import {ru} from "date-fns/locale/ru";
import DatePicker, {registerLocale} from "react-datepicker";
import Select from "react-select";
import {Context} from "../../index";
import {IOrganization} from "../../models/org/IOrganization";
import {IProgram} from "../../models/org/IProgram";
import {ICategory} from "../../models/org/ICategory";
import BizService from "../../services/BizServise";
import "react-datepicker/dist/react-datepicker.css";
import moment from "moment";
import './index.css'
import {ISearchCustomerModel} from "../../models/biz/ISearchCustomerModel";
registerLocale("ru", ru)



const SearchPage: FC = () => {

    const { organizationStore } = useContext(Context);
    const [organization, setOrganization] = useState<IOrganization>();
    const [program, setProgram] = useState<IProgram>();
    const [selectedCategories, setSelectedCategories] = useState<ICategory[]>([]);

    const [balance, setBalance] = useState<number>(0);
    const [customerName, setCustomerName] = useState('');
    const [customerCard, setCustomerCard] = useState('');

    const [customers, setCustomers] = useState<ISearchCustomerModel[]>([]);
    const [isLoadCustomers, setIsLoadCustomers] = useState(false);

    const [dateFrom, setDateFrom] = useState<Date>(new Date(new Date().setDate(1)));
    const [dateTo, setDateTo] = useState<Date>(new Date());

    const onOrganizationSelectChange = (organization: IOrganization) => {
        setOrganization(organization)
        setSelectedCategories([])
        setProgram(organization?.programs[0])
    }
    async function firstInit() {
        await organizationStore.requestOrganizations();
        onOrganizationSelectChange(organizationStore.organizations[0])
        //console.log('isLoading false')
    }


    function onChangeCustomerName(value: string) {
        setCustomerName(value)
        if (value.length == 0 && customerCard.length == 0) {
            setCustomers([])
        }
    }
    function onChangeCustomerCard(value: string) {
        setCustomerCard(value)
        if (value.length == 0 && customerName.length == 0) {
            setCustomers([])
        }
    }
    async function clickSearchButton() {
        setIsLoadCustomers(true)
        const response = await BizService.SearchCustomerFromBiz({
            name:customerName,
            card:customerCard,
            organizationId:organization!.id,
            programId: program!.id,
            dateFrom: moment(dateFrom).toISOString(),
            dateTo: moment(dateTo).add(1, 'days').toISOString(),
            isOffline:false
        })
        //console.log('customers: ', response.data)
        setCustomers(response.data)
        setIsLoadCustomers(false)
    }
    async function ChangeCustomerCategory(id: string, name: string, isRemove: boolean) {
        const catNames= await BizService.ChangeCustomerBizCategory({
            id:id,
            organizationId: organization!.id,
            categories: selectedCategories.map(category=> category.id),
            isRemove
        })
        setIsLoadCustomers(true)

        if (isRemove) {
            const customerNewCategories = customers
                .find(x => x.id === id)?.categories
                .filter(x => !catNames.data.includes(x))
            const temp = customers
            console.log(customerNewCategories)
            if (customerNewCategories)
                temp.find(x => x.id === id)!.categories = customerNewCategories
            console.log(temp)
            setCustomers(temp)
            window.confirm(`У пользователя "${name}" удалены указанные категории`)
        }
        else {
            const temp = customers
            temp.find(x => x.id === id)?.categories.push(catNames.data)
            console.log(temp)
            setCustomers(temp)
            window.confirm(`Пользователю "${name}" добавлены указанные категории`)
        }
        setIsLoadCustomers(false)


    }
    async function ChangeCustomerBalance(id: string, name: string) {
        await BizService.ChangeCustomerBizBalance({
            id,
            organizationId: organization!.id,
            programId: program!.id,
            balance
        })
        setIsLoadCustomers(true)
        window.confirm(`Пользователю "${name}" установлен баланс в размере ${balance} рублей`)
        customers.find(x => x.id === id)!.balance = balance
        // console.log(customers)
        setCustomers(customers)
        setIsLoadCustomers(false)
    }

    function div_datePickers() {
        return (
            <div className="div-datePicker">
                <label htmlFor="dateFrom" >Период с </label>
                <DatePicker
                    dateFormat='dd MMMM yyyy'
                    selected={dateFrom}
                    selectsStart
                    startDate={dateFrom}
                    endDate={dateTo}
                    onChange={date => setDateFrom(date as Date)}
                    id="dateFrom"
                    locale='ru'
                    placeholderText="Период с"

                />

                <label htmlFor="dateTo" > по </label>
                <DatePicker
                    dateFormat='dd MMMM yyyy'
                    selected={dateTo}
                    selectsEnd
                    startDate={dateFrom}
                    endDate={dateTo}
                    minDate={dateFrom}
                    onChange={date => setDateTo(date as Date)}
                    name="dateTo"
                    locale='ru'
                    placeholderText="Период по"
                />
            </div>
        )
    }
    function getCustomersInfo() {
        //console.log("getCustomersInfo length", customersInfo.length)
        if (isLoadCustomers) {
            return <div className="center">Идет поиск...</div>
        }
        if (customers.length === 0) {
            return <div className="center">Нет результатов</div>
        }
        const div_ChangeCustomerCategory = (customer:ISearchCustomerModel) => {
            return (<div>
                <label>Добавить или удалить выбранные категории</label>
                <span>
                    <button className="button"
                        onClick={() => ChangeCustomerCategory(customer.id, customer.name, false)}>
                        Добавить
                    </button>
                    <button className="button red"
                        onClick={() => ChangeCustomerCategory(customer.id, customer.name, true)}>
                        Удалить
                    </button>
                </span>
            </div>)
        }
        const div_ChangeCustomerBalance = (customer: ISearchCustomerModel) => {
            return (<div>
                <label>Изменение баланса</label>
                    <button className="button"
                        onClick={() => ChangeCustomerBalance(customer.id, customer.name)}>
                        Установить
                    </button>
            </div>)
        }
        return (
            <div className="center search form-group col-md-12">
                <ul>
                    {customers && customers.map(customer => {
                        return (
                            <li key={customer.id}>
                                <dt>{customer.name}</dt>
                                {div_ChangeCustomerCategory(customer)}
                                {div_ChangeCustomerBalance(customer)}
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
                                        <li>Обновлено: {convertUTCDateToLocalDate(customer.timeUpdateBalance)}</li>
                                        <li>Категории:</li>
                                        {customer.categories && customer.categories.map(category => {
                                            return (<li>{category}</li>)
                                        })}
                                        <li>Последний визит: {customer.lastVisit}</li>
                                    </ul>
                                </dd>
                                
                            </li>
                        )
                    })}
                </ul>

            </div>
        )
    }
    function convertUTCDateToLocalDate(date_string: string): string {
        date_string = date_string.replace('Z', '');
        console.log("string",date_string);
        const date = new Date(date_string);
        console.log("date", date);

        const newDate = new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
        console.log("newDate", newDate);

        return newDate.toLocaleDateString() + ' ' + newDate.toLocaleTimeString();
    }
    function div_OnlineParams() {

        return (
            <div>
                <label htmlFor="categories">Укажите категорию для добавления/удаления у пользователя</label>
                <Select
                    id="categories"
                    onChange={values => setSelectedCategories(values as ICategory[])}
                    value={selectedCategories}
                    options={organization?.categories}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Категории'
                    isMulti
                />
                { div_datePickers() }
                <label htmlFor="balance">Баланс</label>
                <input
                    id='balance'
                    onChange={e => setBalance(parseInt(e.target.value))}
                    value={balance}
                    type='number'
                    placeholder='Баланс'
                />
            </div>
        )
    }
    useEffect(() => {
        firstInit();
        //console.log('SearchCustomerForm useEffect');
    }, []);
    if (organizationStore.isLoading) {
        return <h1>Loading...</h1>
    }

    return (

        <div>
            <h1 className="center form-group col-md-12">Поиск сотрудника</h1>
            <div className="center form-group col-md-12">
                <label htmlFor="organizations">Организации</label>
                <Select
                    id='organizations'
                    value={organization}
                    options={organizationStore.organizations}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    onChange={organization => onOrganizationSelectChange(organization as IOrganization)}
                />
                <label htmlFor="programs">Программы питания</label>
                <Select
                    id='programs'
                    value={program}
                    options={organization?.programs}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    onChange={program => setProgram(program as IProgram)}
                />
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
                <button className="button"
                        onClick={clickSearchButton}>
                    Найти
                </button>
                <br/>
                {div_OnlineParams()}

            </div>
            {getCustomersInfo()}


        </div>

    );
};

export default observer(SearchPage);
