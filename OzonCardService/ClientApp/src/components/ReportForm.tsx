import { FC, useContext, useEffect, useState } from 'react';
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
registerLocale("ru", ru);
import * as moment from 'moment';
import BizService from '../services/BizServise';

const ReportForm: FC = () => {
    const navigate = useNavigate()
    const { organizationstore, taskstore } = useContext(Context);


    const [organizationId, setOrganizationId] = useState('');
    const [corporateNutritions, setCorporateNutritions] = useState<ICorporateNutritionResponse[]>([]);
    const [corporateNutritionId, setCorporateNutritionId] = useState('');
    const [fileName, setFileName] = useState('');
    const [dateFrom, setDateFrom] = useState<Date>(new Date(new Date().setDate(1)));
    const [dateTo, setDateTo] = useState<Date>(new Date());



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
        setCorporateNutritions(organization?.corporateNutritions ?? [])
        setCorporateNutritionId(organization?.corporateNutritions[0]?.id ?? '')
    }
    async function firstInit() {
        await organizationstore.requestOrganizations();
        setOrganizationId(organizationstore.organizations[0]?.id ?? '');

        setCorporateNutritions(organizationstore.organizations[0]?.corporateNutritions ?? [])
        setCorporateNutritionId(organizationstore.organizations[0]?.corporateNutritions[0]?.id ?? '')
        organizationstore.setLoading(false);

    }
    async function reportFromBiz() {
        const option: IReportOptionResponse = {
            organizationId: organizationId,
            corporateNutritionId: corporateNutritionId,
            dateFrom: (moment(dateFrom)).format("YYYY-MM-DD"),
            dateTo: (moment(dateTo)).add(1, 'days').format("YYYY-MM-DD"),
            title: fileName === ''
                ? `Отчет от ${(moment(new Date())).format("DD.MM.YYYY HH.mm")}`
                : fileName
        }
        const response = await BizService.ReportFromBiz(option)
        taskstore.onAddTask(response.data, 'Отчет: ' + option.title)
        navigate(`/task`)
    }
    useEffect(() => {
        firstInit();
    }, []);
    if (organizationstore.isLoading) {
        return <h1>Loading...</h1>
    }
    
    return (
        <div>
            <h1>Отчеты</h1>
            <div className="form-group col-md-6">
                <label htmlFor="organizations">Организации</label>
                <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations}
                    onChange={onOrganizationSelectChange} />
           
                <label htmlFor="corporateNutritions">Программы питания</label>
                <CustomSelect id="corporateNutritions" value={corporateNutritionId} options={corporateNutritions}
                    onChange={event => setCorporateNutritionId(event.target.value)} />
                
                <label htmlFor="dateFrom" >Период с</label>
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
                
                <label htmlFor="dateTo" >Период по</label>
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

                <button className="btn-primary button"
                    onClick={reportFromBiz}>
                    Выгрузить
                </button>
            </div>



        </div>
    );
};
export default observer(ReportForm);
