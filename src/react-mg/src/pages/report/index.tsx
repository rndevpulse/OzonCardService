import { FC, useContext, useEffect, useState } from 'react';
import * as React from 'react'
import { useNavigate } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import {ru} from "date-fns/locale/ru";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import DatePicker, {registerLocale} from "react-datepicker";
import Select from "react-select";
import {Context} from "../../index";
import {IOrganization} from "../../models/org/IOrganization";
import {IProgram} from "../../models/org/IProgram";
import {ICategory} from "../../models/org/ICategory";
import {IReportOption} from "../../models/biz/IReportOption";
import BizService from "../../services/BizServise";
import moment from "moment";
import "react-datepicker/dist/react-datepicker.css";
import './index.css'
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
        navigate(`/tasks`)
    }

    async function transactionsFromBiz() {
        const option = getOptions()
        const response = await BizService.TransactionsFromBiz(option)
        taskStore.onAddTask(response.data, 'Отчет: ' + option.title)
        navigate(`/tasks`)
    }


    function div_datePickers() {
        return (
            <div >
                <label htmlFor="formDateFrom" className="form-label">Период с</label>
                <DatePicker
                    id="formDateFrom"
                    dateFormat='dd MMMM yyyy'
                    selected={dateFrom}
                    selectsStart
                    startDate={dateFrom}
                    endDate={dateTo}
                    onChange={date => setDateFrom(date as Date)}
                    locale='ru'
                    placeholderText="Период с"
                />
                <label htmlFor="formDateTo" className="form-label">по</label>
                <DatePicker
                    id='formDateTo'
                    dateFormat='dd MMMM yyyy'
                    selected={dateTo}
                    selectsEnd
                    startDate={dateFrom}
                    endDate={dateTo}
                    minDate={dateFrom}
                    onChange={date => setDateTo(date as Date)}
                    locale='ru'
                    placeholderText="Период по"
                />


            </div>
        )
    }


    function div_nameFileReport() {
        return (
            <div>
                <label htmlFor="reportName">Изменить наименование отчета (файла) для сохранения</label>
                <input
                    id="reportName"
                    type="text"
                    value={fileName}
                    onChange={event => setFileName(event.target.value)}
                    placeholder={`Отчет от ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`}
                />

            </div>
        )
    }


    function div_TabItem() {
        return (
            <div>
                <label htmlFor="organizations">Организации</label>
                <Select
                    id='organizations'
                    value={organization}
                    options={organizationStore.organizations}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    onChange={organization => onOrganizationSelectChange(organization as IOrganization)}
                />
                <label htmlFor="categories">Фильтр категорий</label>
                <Select
                    id='categories'
                    onChange={values => setCategories(values as ICategory[])}
                    value={categories}
                    options={organization?.categories}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Категории'
                    isMulti
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
                {div_datePickers()}
                {div_nameFileReport()}
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
        <div className="center form-group col-md-12">
        <h1>Отчеты</h1>
            <div>
                <Tabs>
                    <TabList>
                        <Tab>Отчет за период</Tab>
                        <Tab>Отчет по операциям</Tab>
                    </TabList>
                    <TabPanel>
                        {div_TabItem()}
                        <button className="button"
                                onClick={reportFromBiz}>
                            Выгрузить
                        </button>
                    </TabPanel>
                    <TabPanel>
                        {div_TabItem()}
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
