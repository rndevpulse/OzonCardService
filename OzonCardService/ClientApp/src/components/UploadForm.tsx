import { FC, useContext, useState, useEffect } from 'react';
import Select from 'react-select'
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import { Context } from '..';
import { ICategoryResponse } from '../models/ICategoryResponse';
import { ICorporateNutritionResponse } from '../models/ICorporateNutritionResponse';
import axios from 'axios';
import { IFileResponse } from '../models/IFileResponse';
import { ICustomerOptionResponse } from '../models/ICustomerOptionResponse';
import BizService from '../services/BizServise';
import { useNavigate } from 'react-router-dom';



const UploadForm: FC = () => {
    const navigate = useNavigate()

    const { organizationstore, taskstore } = useContext(Context);


    const [organizationId, setOrganizationId] = useState('');
    
    const [corporateNutritionId, setCorporateNutritionId] = useState('');
    const [file, setFile] = useState('');
    const [fileName, setFileName] = useState('');
    const [refreshBalance, setRefreshBalance] = useState(false);
    const [rename, setRename] = useState(false);

    const [categories, setCategories] = useState<ICategoryResponse[]>([]);
    const [corporateNutritions, setCorporateNutritions] = useState<ICorporateNutritionResponse[]>([]);

    const [balance, setBalance] = useState<number>(0);



    const [customerName, setCustomerName] = useState('');
    const [customerCard, setCustomerCard] = useState('');

    const [currentCategories, setCurrentCategories] = useState<string[]>([]);
    const getValue = () => {
        return currentCategories
            ? categories.filter(c => currentCategories.indexOf(c.id) >= 0)
            : [] 
    }
    const onChangeCategory = (newCategory: any) => {
        setCurrentCategories(newCategory.map(c => c.id))
    }

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
        
        setCorporateNutritions(organization?.corporateNutritions ?? [])
        setCorporateNutritionId(organization?.corporateNutritions[0]?.id ?? '')

        setCurrentCategories([]);
    }
    async function firstInit() {
        await organizationstore.requestOrganizations();
        setSetters()


        organizationstore.setLoading(false);
        //console.log('isLoading false')

    }
    function setSetters() {
        setOrganizationId(organizationstore.organizations[0]?.id ?? '');
        setCategories(organizationstore.organizations[0]?.categories ?? []);
        

        setCorporateNutritions(organizationstore.organizations[0]?.corporateNutritions ?? [])
        setCorporateNutritionId(organizationstore.organizations[0]?.corporateNutritions[0]?.id ?? '')
        setCurrentCategories([]);
    }

    async function  onChangeFile(e) {
        const formdata = new FormData()
        
        formdata.append('file', e.target.files[0])
        const config = {
            headers: {
                'content-type': 'multipart/form-data',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        }
        const response = await axios.post<IFileResponse>('/api/file/create', formdata, config)
        setFile(response.data.url)
        setFileName(response.data.name)
    }

    async function  uploadToBiz() {
        if (file === '')
            confirm('Не выбран вайл выгрузки')
        const option: ICustomerOptionResponse = {
            organizationId: organizationId,
            corporateNutritionId: corporateNutritionId,
            categoriesId: currentCategories,
            balance: balance,
            fileReport: file,
            options: {
                refreshBalance: refreshBalance,
                rename: rename
            },
            customer: null
        }
        
        const response = await BizService.upladCustomersToBiz(option)
        taskstore.onAddTask(response.data, 'Выгрузка: ' + fileName)
        
        navigate(`/task`)
    }
    

    async function singleUploadToBiz() {
        const option: ICustomerOptionResponse = {
            organizationId: organizationId,
            corporateNutritionId: corporateNutritionId,
            categoriesId: currentCategories,
            balance: balance,
            fileReport: file,
            options: {
                refreshBalance: refreshBalance,
                rename: rename
            },
            customer: {
                name: customerName,
                card: customerCard
            }
        }

        const response = await BizService.upladCustomersToBiz(option)
        taskstore.onAddTask(response.data, 'Выгрузка: ' + fileName)

        navigate(`/task`)
    }

    async function updateOrganization() {
        await organizationstore.updateOrganization(organizationId)
        setSetters()
    }

    useEffect(() => {
        firstInit();
        //console.log('UploadForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return <h1>Loading...</h1>
    }
    return (

        <div >
            <h1 className="form-group col-md-7">Выгрузка в iikoBiz</h1>
           
            <div className="form-group col-md-7">
                <h5 className="link" onClick={updateOrganization}>Обновить данные по организации</h5>
                <label htmlFor="organizations">Организации</label>
                <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations}
                    onChange={onOrganizationSelectChange} />
                <label htmlFor="categories">Категории</label>
                
                <Select
                    id= 'categories'
                    onChange={onChangeCategory}
                    value={getValue()}
                    options={categories}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Категории'
                    isMulti />
                <label htmlFor="corporateNutritions">Программы питания</label>
                <CustomSelect id="corporateNutritions" value={corporateNutritionId} options={corporateNutritions}
                    onChange={event => setCorporateNutritionId(event.target.value)} />
                <label htmlFor="balance">Баланс
                <input
                    id='balance'
                    onChange={e => setBalance(parseInt(e.target.value))}
                    value={balance}
                    type='number'
                    placeholder='Баланс'
                    /></label>
            </div>
            <div className="form-group col-md-7">
                <label htmlFor='refreshBalance' className="label-checkbox">
                    <input id='refreshBalance' type='checkbox' checked={refreshBalance}
                        onChange={() => setRefreshBalance(!refreshBalance)}
                    />
                    Обновлять баланс
                    <i className="material-icons red-text">
                        {refreshBalance ? 'check_box' : 'check_box_outline_blank' }
                    </i>
                </label>
                <label htmlFor="rename" className="label-checkbox">
                    <input id='rename' type='checkbox' checked={rename}
                        onChange={() => setRename(!rename)}
                    />
                    Переименовать в соответствии с новым списком
                    <i className="material-icons red-text">
                        {rename ? 'check_box' : 'check_box_outline_blank'}
                    </i>
                </label>

                <label htmlFor="customerName">Сотрудник
                    <input
                        id='customerName'
                        onChange={e => setCustomerName(e.target.value)}
                        value={customerName}
                        type='text'
                        placeholder='ФИО сотрудника'
                    /></label>
                <br />
                <label htmlFor="customerCard">Сотрудник
                    <input
                        id='customerName'
                        onChange={e => setCustomerCard(e.target.value)}
                        value={customerCard}
                        type='text'
                        placeholder='Карта сотрудника'
                    /></label>
                <button className="button"
                    onClick={singleUploadToBiz}>
                    Добавить одного
                </button>
                        <br />
                <label htmlFor="file">Выбирите файл</label>
                <br/>
                
                <input className="form-group button"
                    id='file'
                    type='file'
                    onChange={onChangeFile}
                />

                <button className="button"
                onClick={uploadToBiz}>
                Выгрузить
                </button>
            </div>
               

           

        </div>
    );
};
export default observer(UploadForm);

