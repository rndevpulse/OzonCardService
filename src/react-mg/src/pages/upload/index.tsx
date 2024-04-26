import { FC, useContext, useState, useEffect } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import Select, {MultiValue, SingleValue} from 'react-select'
import { useNavigate } from 'react-router-dom';
import {Context} from "../../index";
import {ICategory} from "../../api/models/org/ICategory";
import {IProgram} from "../../api/models/org/IProgram";
import {ICustomerOption} from "../../api/models/biz/ICustomerOption";
import BizService from "../../services/BizServise";
import FileService from "../../services/FileServise";
import {IOrganization} from "../../api/models/org/IOrganization";



const UploadPage: FC = () => {
    const navigate = useNavigate()

    const { organizationStore, taskStore } = useContext(Context);


    const [organization, setOrganization] = useState<IOrganization>();
    const [program, setProgram] = useState<IProgram>();

    const [file, setFile] = useState('');
    const [fileName, setFileName] = useState('');
    const [refreshBalance, setRefreshBalance] = useState(false);
    const [rename, setRename] = useState(false);

    const [categories, setCategories] = useState<ICategory[]>([]);
    const [programs, setPrograms] = useState<IProgram[]>([]);

    const [balance, setBalance] = useState<number>(0);



    const [customerName, setCustomerName] = useState('');
    const [customerCard, setCustomerCard] = useState('');

    const [currentCategories, setCurrentCategories] = useState<string[]>([]);
    const getValue = () => {
        return currentCategories
            ? categories.filter(c => currentCategories.indexOf(c.id) >= 0)
            : []
    }
    const onChangeCategory = (value: MultiValue<ICategory>) => {
        setCurrentCategories(value.map(c => c.id))
    }

    // const CustomSelect({id, value, options, onChange}) => {
    //     return (
    //         <select className="custom-select" id={id} value={value} onChange={onChange}>
    //             { options.map(option =>
    //                 <option key={option.id} value={option.id}>{option.name}</option>
    //             )}
    //         </select>
    //     )
    // }
    const onOrganizationSelectChange = (org: SingleValue<IOrganization>) => {
        const organization = org as IOrganization;
        setOrganization(organization)
        setProgram(organization?.programs[0])
        setCategories(organization?.categories ?? [])
        setPrograms(organization?.programs ?? [])
        setCurrentCategories([]);
    }

    async function firstInit() {

        await organizationStore.requestOrganizations();
        setSetters()
        organizationStore.setLoading(false);
        //console.log('isLoading false')

    }
    function setSetters() {
        setOrganization(organizationStore.organizations[0]);
        setCategories(organizationStore.organizations[0]?.categories ?? []);
        setPrograms(organizationStore.organizations[0]?.programs ?? [])
        setProgram(organizationStore.organizations[0]?.programs[0])
        setCurrentCategories([]);
    }

    async function  onChangeFile(e: any) {
        const formData = new FormData()
        formData.append('file', e.target.files[0])
        const response = await FileService.createFile(formData)
        setFile(response.data.url)
        setFileName(response.data.name)
    }

    async function  uploadToBiz() {
        if (file === '')
            window.confirm('Не выбран вайл выгрузки')
        const option: ICustomerOption = {
            organizationId: organization!.id,
            programId: program!.id,
            categoriesId: currentCategories,
            balance: balance,
            fileReport: file,
            options: {
                refreshBalance: refreshBalance,
                rename: rename
            },
            customer: null
        }

        const response = await BizService.uploadCustomersToBiz(option)
        taskStore.onAddTask(response.data, 'Выгрузка: ' + fileName)

        navigate(`/task`)
    }


    async function singleUploadToBiz() {
        const option: ICustomerOption = {
            organizationId: organization!.id,
            programId: program!.id,
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

        const response = await BizService.uploadCustomersToBiz(option)
        taskStore.onAddTask(response.data, 'Выгрузка: ' + fileName)

        navigate(`/task`)
    }

    async function updateOrganization() {
        await organizationStore.updateOrganization(organization!.id)
        setSetters()
    }

    useEffect(() => {
        firstInit();
        //console.log('UploadForm useEffect');
    }, []);
    if (organizationStore.isLoading) {
        return <h1>Loading...</h1>
    }
    return (

        <div >
            <h1 className="form-group col-md-7">Выгрузка в iikoBiz</h1>

            <div className="form-group col-md-7">
                <h5 className="link" onClick={updateOrganization}>Обновить данные по организации</h5>
                <label htmlFor="organizations">Организации</label>
                <Select
                    id="organizations"
                    value={organization}
                    onChange={onOrganizationSelectChange}
                    options={organizationStore.organizations}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Организации'
                />
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
                <label htmlFor="programs">Программы питания</label>
                <Select
                    id="programs"
                    onChange={event => setProgram(event as IProgram)}
                    value={program}
                    options={programs}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Программы питания'
                />
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
                <label htmlFor="customerCard">Карта сотрудника
                    <input
                        id='customerCard'
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
export default observer(UploadPage);

