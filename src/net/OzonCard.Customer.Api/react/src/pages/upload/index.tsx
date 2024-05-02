import { FC, useContext, useState, useEffect } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import Select, {MultiValue, SingleValue} from 'react-select'
import { useNavigate } from 'react-router-dom';
import {Context} from "../../index";
import {ICategory} from "../../models/org/ICategory";
import {IProgram} from "../../models/org/IProgram";
import {ICustomerOption} from "../../models/biz/ICustomerOption";
import BizService from "../../services/BizServise";
import FileService from "../../services/FileServise";
import {IOrganization} from "../../models/org/IOrganization";
import "./index.css"



const UploadPage: FC = () => {
    const navigate = useNavigate()

    const { organizationStore, taskStore } = useContext(Context);


    const [organization, setOrganization] = useState<IOrganization>();
    const [program, setProgram] = useState<IProgram>();
    const [selectedCategories, setSelectedCategories] = useState<ICategory[]>([]);

    const [file, setFile] = useState('');
    const [fileName, setFileName] = useState('');
    const [refreshBalance, setRefreshBalance] = useState(false);
    const [rename, setRename] = useState(false);


    const [balance, setBalance] = useState<number>(0);
    const [customerName, setCustomerName] = useState('');
    const [customerCard, setCustomerCard] = useState('');

    const onOrganizationSelectChange = (org: SingleValue<IOrganization>) => {
        const organization = org as IOrganization;
        setOrganization(organization)
        setProgram(organization?.programs[0])
        setSelectedCategories([])
    }

    async function firstInit() {

        await organizationStore.requestOrganizations();
        setSetters()
        organizationStore.setLoading(false);
        //console.log('isLoading false')

    }
    function setSetters() {
        const org = organizationStore.organizations[0]
        console.log(org.id)
        setOrganization(org);
        setProgram(org?.programs[0])
        setSelectedCategories([])

    }

    async function  onChangeFile(e: any) {
        const formData = new FormData()
        formData.append('file', e.target.files[0])
        const response = await FileService.createFile(formData)
        setFile(response.data.url)
        setFileName(response.data.name)
    }

    function createCustomerOption(withCustomer:boolean) : ICustomerOption{
        const option: ICustomerOption = {
            organizationId: organization!.id,
            programId: program!.id,
            categoriesId: selectedCategories.map(x=>x.id),
            balance: balance,
            fileReport: file,
            options: {
                refreshBalance: refreshBalance,
                rename: rename
            },
            customer: withCustomer
                ? { name: customerName, card: customerCard }
                : null
        }
        return option;
    }
    async function  uploadToBiz() {
        if (file === '')
            window.confirm('Не выбран вайл выгрузки')
        const option = createCustomerOption(false)
        const response = await BizService.uploadCustomersToBiz(option)
        taskStore.onAddTask(response.data, 'Выгрузка: ' + fileName)
        navigate(`/tasks`)
    }

    async function singleUploadToBiz() {
        const option = createCustomerOption(true)
        const response = await BizService.uploadCustomersToBiz(option)
        taskStore.onAddTask(response.data, 'Выгрузка: ' + fileName)
        navigate(`/tasks`)
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

        <div className="center form-group col-md-12">
            <h1>Выгрузка в iiko Biz</h1>
            <div>
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
                    id='categories'
                    onChange={values => setSelectedCategories(values as ICategory[])}
                    value={selectedCategories}
                    options={organization?.categories}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Категории'
                    isMulti/>
                <label htmlFor="programs">Программы питания</label>
                <Select
                    id="programs"
                    onChange={event => setProgram(event as IProgram)}
                    value={program}
                    options={organization?.programs}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Программы питания'
                />

            </div>
            <div>
                <label htmlFor="balance">Баланс</label>
                <input
                    id='balance'
                    onChange={e => setBalance(parseInt(e.target.value))}
                    value={balance}
                    type='number'
                    placeholder='Баланс'
                />

                <label htmlFor='refreshBalance' className="label-checkbox">Обновлять баланс
                    <input id='refreshBalance' type='checkbox' checked={refreshBalance}
                           onChange={() => setRefreshBalance(!refreshBalance)}
                    />

                    <i className="material-icons red-text">
                        {refreshBalance ? 'check_box' : 'check_box_outline_blank'}
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

            </div>


            <div className="container-row-customer container-row-wrap">
                <label htmlFor="file">Выбирите файл
                    <input className="button"
                           id='file'
                           type='file'
                           onChange={onChangeFile}
                    />
                </label>
                <button className="button"
                        onClick={uploadToBiz}>
                    Выгрузить
                </button>
            </div>

            <div className="container-row-customer">
                <div>
                    <label htmlFor="customerName">Сотрудник
                        <input
                            id="customerName"
                            onChange={e => setCustomerName(e.target.value)}
                            value={customerName}
                            type='text'
                            placeholder='ФИО сотрудника'
                        />
                    </label>
                    <label htmlFor="customerCard">Карта сотрудника
                        <input
                            id="customerCard"
                            onChange={e => setCustomerCard(e.target.value)}
                            value={customerCard}
                            type='text'
                            placeholder='Карта сотрудника'
                        />
                    </label>
                </div>
                <button className="button"
                        onClick={singleUploadToBiz}>
                    Добавить одного
                </button>
            </div>
        </div>
    );
};
export default observer(UploadPage);

