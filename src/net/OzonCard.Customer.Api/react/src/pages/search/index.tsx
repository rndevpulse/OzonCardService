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
import BizService from "../../services/BizServise";
import "react-datepicker/dist/react-datepicker.css";
import './index.css'
import {ISearchCustomerModel} from "../../models/biz/ISearchCustomerModel";
import {ChangeCustomer, Customer} from "../../components/customer";
import {Loader} from "../../components/loader";
import {useToast} from "../../components/toast";
import {ModalContext} from "../../context/modal";
import {Modal} from "../../components/modal";
registerLocale("ru", ru)



const SearchPage: FC = () => {

    const toast = useToast();
    const {modal, open, close}=useContext(ModalContext)


    const { organizationStore } = useContext(Context);
    const [organization, setOrganization] = useState<IOrganization>();
    const [program, setProgram] = useState<IProgram>();

    const [customerName, setCustomerName] = useState('');
    const [customerCard, setCustomerCard] = useState('');

    const [customers, setCustomers] = useState<ISearchCustomerModel[]>([]);
    // const [isLoadCustomers, setIsLoadCustomers] = useState(false);

    const [dateFrom, setDateFrom] = useState<Date>(new Date(new Date().setDate(1)));
    const [dateTo, setDateTo] = useState<Date>(new Date());

    const [customer, setCustomer] = useState<ISearchCustomerModel>();


    const onOrganizationSelectChange = (organization: IOrganization) => {
        setOrganization(organization)
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
        toast.show("Идет поиск...", "info")
        // setIsLoadCustomers(true)
        const response = await BizService.SearchCustomerFromBiz({
            name:customerName,
            card:customerCard,
            organizationId:organization!.id,
            programId: program!.id,
            dateFrom: dateFrom,
            dateTo: dateTo,
            offset: -(new Date().getTimezoneOffset()),
            isOffline:false
        })
        console.log('customers: ', response)
        if (!response){
            return
        }
        setCustomers(response.data)
        // setIsLoadCustomers(false)
    }




    function getCustomersInfo() {
        //console.log("getCustomersInfo length", customersInfo.length)
        // if (isLoadCustomers) {
        //     toast.show("Идет поиск...", "info");
        //     return //<div className="center">Идет поиск...</div>
        // }
        if (customers.length === 0) {
            // toast.show("Сотрудников не найдено", "info");

            return //<div className="center">Нет результатов</div>
        }

        return (
            <ul className="center search form-group col-md-12">
                {customers && customers.map(customer =>
                    <Customer
                        customer={customer}
                        organization={organization as IOrganization}
                        onChanging={OnChangingCustomer}
                        key={customer.id}
                    />)}
            </ul>
        )
    }
    function OnChangingCustomer(customer:ISearchCustomerModel, organization:IOrganization) {
        console.log("OnChangingCustomer", customer.card)
        setCustomer(customer)
        open();
    }



    const onChangeHandler = (customer:ISearchCustomerModel) => {
        console.log("OnChangeCustomer", customer)
        const temp = customers.filter(x=>x.id !== customer.id)
        temp.push(customer)
        setCustomers(temp)
        close()
        toast.show("Сотрудник изменен", "info")
    }
    const onRemoveHandler = (customer:ISearchCustomerModel) => {
        console.log("OnRemoveCustomer", customer)
        const temp = customers.filter(x=>x.id !== customer.id)
        setCustomers(temp)
        close()
        toast.show("Сотрудник удален", "info")

    }

    function div_OnlineParams() {

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

    useEffect(() => {
        firstInit();
        console.log("time offset", -(new Date().getTimezoneOffset()))
        //console.log('SearchCustomerForm useEffect');
    }, []);
    if (organizationStore.isLoading) {
        return <Loader/>
    }

    return (

        <div  className="center form-group col-md-12">
            <h1>Поиск сотрудника</h1>
            <div >
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
            {modal && customer && organization && <Modal title={'Редактирование сотрудника'} onClose={close}>
                <ChangeCustomer
                    customer={customer}
                    categories={organization.categories}
                    onChange={onChangeHandler}
                    onRemove={onRemoveHandler}
                />
            </Modal>}
        </div>
    );
};

export default observer(SearchPage);
