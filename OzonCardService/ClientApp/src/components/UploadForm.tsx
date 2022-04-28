import { FC, useContext, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
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
    const [categoryId, setCategoryId] = useState('');
    const [corporateNutritionId, setCorporateNutritionId] = useState('');
    const [file, setFile] = useState('');
    const [fileName, setFileName] = useState('');
    const [refreshBalance, setRefreshBalance] = useState(false);
    const [rename, setRename] = useState(false);

    const [categories, setCategories] = useState<ICategoryResponse[]>([]);
    const [corporateNutritions, setCorporateNutritions] = useState<ICorporateNutritionResponse[]>([]);

    const [balance, setBalance] = useState<number>(0);


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
        setOrganizationId(organizationstore.organizations[0]?.id ?? []);

        setCategories(organizationstore.organizations[0]?.categories ?? []);
        setCategoryId(organizationstore.organizations[0]?.categories[0]?.id ?? '');

        setCorporateNutritions(organizationstore.organizations[0]?.corporateNutritions ?? [])
        setCorporateNutritionId(organizationstore.organizations[0]?.corporateNutritions[0]?.id ?? '')



        organizationstore.setLoading(false);
        console.log('isLoading false')

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
            categoryId: categoryId,
            balance: balance,
            fileReport: file,
            options: {
                refreshBalance: refreshBalance,
                rename: rename
            }
        }
        console.log('option ', JSON.stringify(option))
        const response = await BizService.upladCustomersToBiz(option)
        taskstore.onAddTask(response.data, 'Выгрузка ' + fileName)
        console.log('taskId', response.data)
        
        navigate(`/task`)
    }

    

    useEffect(() => {
        firstInit();
        console.log('UploadForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return <h1>Loading...</h1>
    }
    console.log('UploadForm return')
    return (

        <div>
            <h1>UPLOAD FORM</h1>
           
            <div className="form-group col-md-6">
                <label htmlFor="organizations">Организации</label>
                <CustomSelect id="organizations" value={organizationId} options={organizationstore.organizations}
                    onChange={onOrganizationSelectChange} />
            </div>
            <div className="form-group col-md-6">
                <label htmlFor="categories">Категории</label>
                <CustomSelect id="categories" value={categoryId} options={categories} onChange={event => setCategoryId(event.target.value)}/>
            </div>
            <div className="form-group col-md-6">
                <label htmlFor="corporateNutritions">Программы питания</label>
                <CustomSelect id="corporateNutritions" value={corporateNutritionId} options={corporateNutritions}
                    onChange={event => setCorporateNutritionId(event.target.value)} />
            </div>

            <div className="form-group col-md-6">
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
                <label htmlFor="file">Выбирите файл</label>
                <br/>
                <input className="form-group"
                    id='file'
                    type='file'
                    onChange={onChangeFile}
                />
            </div>
               

            <button className="uploadToBiz button"
                onClick={uploadToBiz}>
                Выгрузить
                </button>

        </div>
    );
};
export default observer(UploadForm);

