import { FC, useContext, useEffect, useState } from 'react';
import * as React from 'react'
import { useNavigate } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import {ru} from "date-fns/locale/ru";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import './index.css'
import DatePicker, {registerLocale} from "react-datepicker";
import Select from "react-select";
import {Context} from "../../index";
import {IOrganization} from "../../api/models/org/IOrganization";
import {IProgram} from "../../api/models/org/IProgram";
import {ICategory} from "../../api/models/org/ICategory";
import {IReportOption} from "../../api/models/biz/IReportOption";
import BizService from "../../services/BizServise";
import moment, {locale} from "moment";
import "react-datepicker/dist/react-datepicker.css";

registerLocale("ru", ru)

const ReportPage: FC = () => {
    const navigate = useNavigate()
    const {organizationStore, taskStore} = useContext(Context);


    const [organization, setOrganization] = useState<IOrganization>()
    const [program, setProgram] = useState<IProgram>();
    const [categories, setCategories] = useState<ICategory[]>([]);


    
    const [fileName, setFileName] = useState('');
    const [dateFrom, setDateFrom] = useState<Date>(new Date(new Date().setDate(1)));
    const [dateTo, setDateTo] = useState<Date>(new Date());
    

    const onOrganizationSelectChange = (organization : IOrganization) => {
        setOrganization(organization)
        setProgram(organization.programs[0])
        setCategories([])
    }

    async function firstInit() {
        await organizationStore.requestOrganizations();
        const org = organizationStore.organizations[0];
        setOrganization(org);
        setProgram(org.programs[0])
        setCategories([])
        organizationStore.setLoading(false);
    }

     function getOptions() : IReportOption{
         const option: IReportOption = {
             organizationId: organization!.id,
             categoriesId: categories.map(c=>c.id),
             programId: program!.id,
             dateFrom: moment(dateFrom).toISOString(),
             dateTo: moment(dateTo).add(1, 'days').toISOString(),
             title: fileName === ''
                 ? `Отчет от ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`
                 : `${fileName} ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`,
             isOffline:false
         }
         return option;
     }

    async function reportFromBiz() {
        const option = getOptions()
        const response = await BizService.ReportFromBiz(option)
        taskStore.onAddTask(response.data, 'Отчет: ' + option.title)
        navigate(`/task`)
    }

    async function transactionsFromBiz() {
        const option = getOptions()
        const response = await BizService.TransactionsFromBiz(option)
        taskStore.onAddTask(response.data, 'Отчет: ' + option.title)
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
                <label htmlFor="categories">Фильтр категорий</label>
                <Select
                    id= 'categories'
                    onChange={values=> setCategories(values as ICategory[])}
                    value={categories}
                    options={organization?.categories}
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




    if (organizationStore.isLoading) {
        return <h1>Loading...</h1>
    }
    
    return (
        <div>
            <h1 className="center form-group col-md-12">Отчеты</h1>
            <div className="center form-group col-md-12">
                <Tabs className="Tabs">
                    <TabList>
                        <Tab>Отчет за период</Tab>
                        <Tab>Отчет по операциям</Tab>
                    </TabList>
                    <TabPanel>
                        <label htmlFor="organizations">Организации</label>
                        <Select
                            id= 'organizations'
                            value={organization}
                            options={organizationStore.organizations}
                            getOptionLabel={option => option.name}
                            getOptionValue={option => option.id}
                            onChange={organization => onOrganizationSelectChange(organization as IOrganization)}
                        />

                        {div_SelectorCategories()}
                        
                        <label htmlFor="programs">Программы питания</label>
                        <Select
                            id= 'programs'
                            value={program}
                            options={organization?.programs}
                            getOptionLabel={option => option.name}
                            getOptionValue={option => option.id}
                            onChange={program => setProgram(program as IProgram)}
                        />
                        {div_datePickers()}
                        {div_nameFileReport()}
                        <button className="button"
                            onClick={reportFromBiz}>
                            Выгрузить
                        </button>
                    </TabPanel>
                    <TabPanel>
                        <label htmlFor="organizations">Организации</label>
                        <Select
                            id= 'organizations'
                            value={organization}
                            options={organizationStore.organizations}
                            getOptionLabel={option => option.name}
                            getOptionValue={option => option.id}
                            onChange={organization => onOrganizationSelectChange(organization as IOrganization)}
                        />

                        {div_SelectorCategories()}
                        
                        <label htmlFor="programs">Программы питания</label>
                        <Select
                            id= 'programs'
                            value={program}
                            options={organization?.programs}
                            getOptionLabel={option => option.name}
                            getOptionValue={option => option.id}
                            onChange={program => setProgram(program as IProgram)}
                            />
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
export default observer(ReportPage);
