﻿import { FC, useContext, useEffect, useState } from 'react';
import * as React from 'react'
import { useNavigate } from 'react-router-dom';
import { Context } from '..';
import { ICorporateNutritionResponse } from '../models/ICorporateNutritionResponse';
import { observer } from 'mobx-react-lite';
import DatePicker, { registerLocale, setDefaultLocale }  from 'react-datepicker'
import "react-datepicker/dist/react-datepicker.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import ru from "date-fns/locale/ru";
import { IReportOptionResponse } from '../models/IReportOptionResponse';
import { ICategoryResponse } from '../models/ICategoryResponse';
import * as moment from 'moment';
import BizService from '../services/BizServise';
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import '../css/ReportForm.css'
import Select from "react-select";
registerLocale("ru", ru);

const ReportForm: FC = () => {
    const navigate = useNavigate()
    const { organizationstore, taskstore } = useContext(Context);


    const [organizationId, setOrganizationId] = useState('');
    const [corporateNutritions, setCorporateNutritions] = useState<ICorporateNutritionResponse[]>([]);
    const [corporateNutritionId, setCorporateNutritionId] = useState('');
    
    const [categories, setCategories] = useState<ICategoryResponse[]>([]);


    
    const [fileName, setFileName] = useState('');
    const [dateFrom, setDateFrom] = useState<Date>(new Date(new Date().setDate(1)));
    const [dateTo, setDateTo] = useState<Date>(new Date());
    
    const [isFilter, setIsFilter] = useState(false);
    const [isOffline, setIsOffline] = useState(false);


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
        setCurrentCategories([]);
        setCorporateNutritions(organization?.corporateNutritions ?? [])
        setCorporateNutritionId(organization?.corporateNutritions[0]?.id ?? '')
    }
    async function firstInit() {
        await organizationstore.requestOrganizations();
        setOrganizationId(organizationstore.organizations[0]?.id ?? '');

        setCorporateNutritions(organizationstore.organizations[0]?.corporateNutritions ?? [])
        setCorporateNutritionId(organizationstore.organizations[0]?.corporateNutritions[0]?.id ?? '')

        setCategories(organizationstore.organizations[0]?.categories ?? []);
        setCurrentCategories([]);

        organizationstore.setLoading(false);
    }

    async function reportFromBiz() {
        const option: IReportOptionResponse = {
            organizationId: organizationId,
            categoriesId: currentCategories,
            corporateNutritionId: corporateNutritionId,
            dateFrom: (moment(dateFrom)).format("YYYY-MM-DD"),
            dateTo: (moment(dateTo)).add(1, 'days').format("YYYY-MM-DD"),
            title: fileName === ''
                ? `Отчет от ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`
                : `${fileName} ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`,
            isOffline
        }
        const response = await BizService.ReportFromBiz(option)
        taskstore.onAddTask(response.data, 'Отчет: ' + option.title)
        navigate(`/task`)
    }
    async function transactionsFromBiz() {
        const option: IReportOptionResponse = {
            organizationId: organizationId,
            categoriesId: currentCategories,
            corporateNutritionId: corporateNutritionId,
            dateFrom: (moment(dateFrom)).format("YYYY-MM-DD"),
            dateTo: (moment(dateTo)).add(1, 'days').format("YYYY-MM-DD"),
            title: fileName === ''
                ? `Отчет от ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`
                : `${fileName} ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`,
            isOffline
        }
        const response = await BizService.TransactionsFromBiz(option)
        taskstore.onAddTask(response.data, 'Отчет: ' + option.title)
        navigate(`/task`)
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
                    onChange={date => setDateFrom(date)}
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
                    onChange={date => setDateTo(date)}
                    name="dateTo"
                    locale='ru'
                    placeholderText="Период по"
                />
            </div>
        )
    }

    const [currentCategories, setCurrentCategories] = useState<string[]>([]);
    const getValue = () => {
        return currentCategories
            ? categories.filter(c => currentCategories.indexOf(c.id) >= 0)
            : []
    }
    const onChangeCategory = (newCategory: any) => {
        setCurrentCategories(newCategory.map(c => c.id))
    }
    function div_nameFileReport() {
        return (
            <label htmlFor="name" >
                Изменить наименование отчета (файла) для сохранения
                <input
                    id='name'
                    type='text'
                    value={fileName}
                    onChange={event => setFileName(event.target.value)}
                    placeholder={`Отчет от ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`}
                />
            </label>
        )
    }


    function div_SelectorCategories(){
        return (
            <div>
                <label htmlFor="allCategories" className="label-checkbox-category">
                    <input id='allCategories' type='checkbox' checked={isFilter}
                           onChange={() => setIsFilter(!isFilter)}
                    />
                    Учитывать фильтр категорий
                    <i className="check_box material-icons red-text">
                        {isFilter ? 'check_box' : 'check_box_outline_blank'}
                    </i>
                </label>

                <label htmlFor="categories">Фильтр категорий</label>
                <Select
                    id= 'categories'
                    onChange={onChangeCategory}
                    value={getValue()}
                    options={categories}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Категории'
                    isMulti />
            </div>
        )
    }
    
    useEffect(() => {
        firstInit();
    }, []);




    if (organizationstore.isLoading) {
        return <h1>Loading...</h1>
    }
    
    return (
        <div>
            <h1 className="center form-group col-md-12">Отчеты</h1>
            <div className="center form-group col-md-12">
                <label htmlFor="isOffline" className="label-checkbox-category">
                    <input id='isOffline' type='checkbox' checked={isOffline}
                        onChange={() => setIsOffline(!isOffline)}
                    />
                    Работать в оффлайн режиме
                    <i className="check_box material-icons red-text">
                        {isOffline ? 'check_box' : 'check_box_outline_blank'}
                    </i>
                </label>
                <Tabs className="Tabs">
                    <TabList>
                        <Tab>Отчет за период</Tab>
                        <Tab>Отчет по операциям</Tab>
                    </TabList>
                    <TabPanel>
                        <label htmlFor="organizations">Организации</label>
                        <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations}
                            onChange={onOrganizationSelectChange} />

                        {div_SelectorCategories()}
                        
                        <label htmlFor="corporateNutritions">Программы питания</label>
                        <CustomSelect id="corporateNutritions" value={corporateNutritionId} options={corporateNutritions}
                            onChange={event => setCorporateNutritionId(event.target.value)} />
                        {div_datePickers()}
                        {div_nameFileReport()}
                        <button className="button"
                            onClick={reportFromBiz}>
                            Выгрузить
                        </button>
                    </TabPanel>
                    <TabPanel>
                        <label htmlFor="organizations">Организации</label>
                        <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations}
                            onChange={onOrganizationSelectChange} />

                        {div_SelectorCategories()}
                        
                        <label htmlFor="corporateNutritions">Программы питания</label>
                        <CustomSelect id="corporateNutritions" value={corporateNutritionId} options={corporateNutritions}
                            onChange={event => setCorporateNutritionId(event.target.value)} />
                        {div_datePickers()}
                        {div_nameFileReport()}
                        <button className="button"
                            onClick={transactionsFromBiz}>
                            Выгрузить
                        </button>
                    </TabPanel>
                </Tabs>

            
                
                
               
                

                
            </div>



        </div>
    );
};
export default observer(ReportForm);
