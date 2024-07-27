import {useContext, useState} from "react";
import {ErrorMessage} from "../error";
import "./index.css"
import * as React from "react";
import Select from "react-select";
import BizService from "../../services/BizServise";
import {ICategory} from "../../models/org";
import {ISearchCustomerModel} from "../../models/biz";
import {Loader} from "../loader";


interface IChangeCustomerProps{
    customer: ISearchCustomerModel,
    categories: ICategory[]
    onChange: (customer: ISearchCustomerModel) => void,
    onRemove: (customer: ISearchCustomerModel) => void,
}


export function ChangeCustomer({customer, categories, onChange, onRemove}: IChangeCustomerProps){

    const [error, setError] = useState('')
    const [isLoad, setIsLoad] = useState(false)


    const [name, setName] = useState(customer.name)
    const [tabNumber, setTabNumber] = useState(customer.tabNumber)
    const [position, setPosition] = useState(customer.position)
    const [division, setDivision] = useState(customer.division)
    const [selectedCategories, setSelectedCategories] = useState<ICategory[]>(customer.categories);
    const [balance, setBalance] = useState<number>(customer.balance);

    const submitChangeHandler = async (event : React.FormEvent) => {
        event.preventDefault()
        setError('')
        if (name.trim().length === 0)
        {
            setError('Поле не должно быть пустым!')
            return
        }
        setIsLoad(true)
        await tryUpdateCustomer()
        await tryUpdateCategories()
        await tryUpdateBalance()
        setIsLoad(false)
        onChange(customer)
    }

    async function tryUpdateCustomer(){
        if (customer.name !== name.trim()
            || customer.tabNumber !== tabNumber.trim()
            || customer.division !== division.trim()
            || customer.position !== position.trim()){

            await BizService.UpdateCustomer({
                id: customer.id,
                name: name.trim(),
                tabNumber: tabNumber.trim(),
                position: position.trim(),
                division: division.trim(),
            })
            customer.name = name.trim()
            customer.tabNumber = tabNumber.trim()
            customer.position = position.trim()
            customer.division = division.trim()
        }
    }
    async function tryUpdateCategories(){
        const selected = selectedCategories.map(x=>x.id)
        const base = customer.categories.map(x=>x.id)
        const removeCategories = base.filter(x=> !selected.includes(x))
        console.log("need removeCategories", removeCategories)
        const addCategories = selected.filter(x=> !base.includes(x))
        console.log("need addCategories", addCategories)

        if (removeCategories.length > 0){
            await BizService.ChangeCustomerBizCategory({
                id:customer.id,
                categories: removeCategories,
                isRemove:true
            })
        }
        if (addCategories.length > 0){
            await BizService.ChangeCustomerBizCategory({
                id:customer.id,
                categories: addCategories,
                isRemove:false
            })
        }
        customer.categories = selectedCategories;
    }
    async function tryUpdateBalance(){

        if (customer.balance === balance)
            return;
        await BizService.ChangeCustomerBizBalance({
            id: customer.id,
            programId: customer.programId,
            balance
        })
        customer.balance = balance
    }

    const submitRemoveHandler = async (event : React.FormEvent) => {
        event.preventDefault()
        await BizService.RemoveCustomer(customer.id)
        onRemove(customer)
    }

    if (isLoad){
        return <Loader/>
    }

    return(
        <div className="center form-group col-md-12 ">
            <div>
                <label className="changeCustomerLabel" htmlFor="customerName">ФИО
                    <input
                        id="customerName"
                        onChange={e => setName(e.target.value)}
                        value={name}
                        type='text'
                        placeholder='ФИО сотрудника'
                    />
                </label>
                {error && <ErrorMessage error={error}/>}
                <label className="changeCustomerLabel" htmlFor="customerCard">Карта
                    <input
                        id="customerCard"
                        value={customer.card}
                        type='text'
                        placeholder='Карта сотрудника'
                        disabled={true}
                    />
                </label>
                <label className="changeCustomerLabel" htmlFor="categories">Категории</label>
                <Select
                    id='categories'
                    onChange={values => setSelectedCategories(values as ICategory[])}
                    value={selectedCategories}
                    options={categories}
                    getOptionLabel={option => option.name}
                    getOptionValue={option => option.id}
                    placeholder='Категории'
                    isMulti
                />
                <label className="changeCustomerLabel" htmlFor="customerTabNumber">Табельный номер
                    <input
                        id="customerTabNumber"
                        onChange={e => setTabNumber(e.target.value)}
                        value={tabNumber}
                        type='text'
                        placeholder='Табельный номер'
                    />
                </label>
                <label className="changeCustomerLabel" htmlFor="customerDivision">Подразделение
                    <input
                        id="customerDivision"
                        onChange={e => setDivision(e.target.value)}
                        value={division}
                        type='text'
                        placeholder='Подразделение'
                    />
                </label>
                <label className="changeCustomerLabel" htmlFor="customerPosition">Должность
                    <input
                        id="customerPosition"
                        onChange={e => setPosition(e.target.value)}
                        value={position}
                        type='text'
                        placeholder='Должность'
                    />
                </label>
                <label className="changeCustomerLabel" htmlFor="balance">Баланс
                    <input
                        id='balance'
                        onChange={e => setBalance(parseInt(e.target.value))}
                        value={balance}
                        type='number'
                        placeholder='Баланс'
                    />
                </label>

            </div>
            <div className="container-row-customer">
                <button className="button red"
                        onClick={submitRemoveHandler}>
                    Удалить
                </button>

                <button className="button"
                        onClick={submitChangeHandler}>
                    Сохранить
                </button>
            </div>
        </div>
    )
}