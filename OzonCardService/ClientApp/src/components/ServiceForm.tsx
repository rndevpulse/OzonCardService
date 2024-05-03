import { FC, useContext, useState, useEffect } from 'react';
import * as React from 'react'
import UserService from '../services/UserService';
import { IUserResponce } from '../models/IUserResponse';
import { Context } from '..';
import { observer } from 'mobx-react-lite';
import '../css/ServiceForm.css'


const ServiceForm: FC = () => {

    const { organizationstore } = useContext(Context)


    const [user, setUser] = useState('')

    const [organizationLogin, setOrganizationLogin] = useState('')
    const [organizationPassword, setOrganizationPassword] = useState('')
    const [organizationId, setOrganizationId] = useState('')



    const [hidden, setHidden] = useState(false)
    const [hiddenOrg, setHiddenOrg] = useState(false)

    const [password, setPassword] = useState('')
    const [ruleBasic, setRuleBasic] = useState(true)
    const [ruleReport, setRuleReport] = useState(true)

    const [users, setUsers] = useState<IUserResponce[]>([])
    const [filterUserEmail, setFilterUserEmail] = useState('')


    async function createUser() {
        const rules: number[] = []
        if (ruleBasic) { rules.push(101) }
        if (ruleReport) { rules.push(111) }
        await UserService.createUser(user, password, rules)
        getUsers()
        window.confirm('Пользователь добавлен')
    }
    async function createOrganization() {
        await organizationstore.createOrganization(organizationLogin, organizationPassword)
        window.confirm('Организация добавлена')

    }
    async function addUserForOrganization(userId: string){
        await UserService.addUserForOrganization(organizationId, userId)
        await getUsers()
    }
    async function delUserForOrganization(userId: string) {
        await UserService.delUserForOrganization(organizationId, userId)
        await getUsers()
    }

    async function getUsers() {
        const response = await UserService.getUsers()
        //console.log('users: ', response.data)
        setUsers(response.data)
        //console.log('organizationstore.organizations.length = ', organizationstore.organizations.length)
        if (organizationstore.organizations.length === 0) {
            await organizationstore.requestOrganizations()
        }
        setOrganizationId(organizationstore.organizations[0]?.id)
    }
    useEffect(() => {
        getUsers()
        
    }, [])

    const formNewUser = () => (
        <li>
            <dt>
                <div className="email__pass">
                    <label htmlFor="user">
                        Email нового пользователя
                    </label>
                    <input
                        id='user'
                        onChange={e => setUser(e.target.value)}
                        value={user}
                        type='text'
                    placeholder='Email'
                    className="email"
                    />
                    <label htmlFor="password">
                        Пароль пользователя
                    </label>
                        <input
                            id='password'
                            onChange={e => setPassword(e.target.value)}
                            value={password}
                            type={!hidden ? 'password' : 'text'}
                            placeholder='Password'

                        />
                    <i className={classes_i.join(' ')}
                        onClick={() => setHidden(!hidden)}>
                        remove_red_eye
                    </i>
                </div>
                <button className="button"
                    onClick={createUser}>
                    Создать
                </button>
            </dt>
            <dd>
                <ul>
                    <li>
                        <label htmlFor="rule_basic" className="label-checkbox">
                            <input id='rule_basic' type='checkbox' checked={ruleBasic}
                                onChange={() => setRuleBasic(!ruleBasic)}
                            />
                            Выгрузка клиентов в iikoBiz
                            <i className="material-icons red-text">
                                {ruleBasic ? 'check_box' : 'check_box_outline_blank'}
                            </i>
                        </label>
                    </li>
                    <li>
                        <label htmlFor="rule_report" className="label-checkbox">
                            <input id='rule_report' type='checkbox' checked={ruleReport}
                                onChange={() => setRuleReport(!ruleReport)}
                            />
                            Построение отчетов из iikoBiz
                            <i className="material-icons red-text">
                                {ruleReport ? 'check_box' : 'check_box_outline_blank'}
                            </i>
                        </label>
                    </li>
                </ul>

            </dd>
        </li>
    )
    const formNewOrganization = () => (
        <li>
            <dt>
                <div className="email__pass">
                    <label htmlFor="organization_login">
                        Api логин новой организации
                    </label>
                    <input
                        id='organization_login'
                        onChange={e => setOrganizationLogin(e.target.value)}
                        value={organizationLogin}
                        type='text'
                        placeholder='Login API'
                    />
                    <label htmlFor="organization_password">
                        Api пароль организации
                    </label>
                    <input
                        id='organization_password'
                        onChange={e => setOrganizationPassword(e.target.value)}
                        value={organizationPassword}
                        type={!hiddenOrg ? 'password' : 'text'}
                        placeholder='Password'

                    />
                    <i className={classes_i_O.join(' ')}
                        onClick={() => setHiddenOrg(!hiddenOrg)}>
                        remove_red_eye
                    </i>
                </div>
                <button className="button"
                    onClick={createOrganization}>
                    Создать
                </button>
            </dt>
            <dd></dd>
        </li>
    )

    const formUsers = () => (
        <li>
            <dt>
                <label htmlFor="user_mail">
                    Фильтр пользователей
                    <input
                        id='user_mail'
                        onChange={e => setFilterUserEmail(e.target.value)}
                        value={filterUserEmail}
                        type='text'
                        placeholder='email'
                    />
                </label>
                <span>
                    <CustomSelect id="user_organization"
                        value={organizationId}
                        options={organizationstore.organizations}
                        onChange={event => setOrganizationId(event.target.value)} />
                </span>
            </dt>
            <dd></dd>

            <ul>
                {users && users.filter(u => u.mail.includes(filterUserEmail)).map(u => {

                    return (
                        <li key={u.id}>
                            <dt>
                                <label className="label-checkbox">{u.mail}</label>
                                <span>
                                    <button className="button"
                                        onClick={() => delUserForOrganization(u.id)}>
                                        Убрать
                                    </button>
                                    <button className="button"
                                        onClick={() => addUserForOrganization(u.id)}>
                                        Добавить
                                    </button>
                                </span>
                            </dt>
                            <dd>
                                <ul>
                                    {u && u.organizations.map(o => {
                                        return (
                                            <li key={o.id}>
                                                <label className="">{o.name}</label>
                                            </li>
                                        )
                                    })}
                                </ul>
                            </dd>
                        </li>
                    )
                })}
            </ul>

        </li>
        )
    const CustomSelect = ({ id, value, options, onChange }) => {
        return (
            <select className="custom-select" id={id} value={value} onChange={onChange}>
                {options.map(option =>
                    <option key={option.id} value={option.id}>{option.name}</option>
                )}
            </select>
        )
    }

    const classes_i = ['password material-icons']
    if (hidden) {
        classes_i.push('red-text')
    }
    const classes_i_O = ['password material-icons']
    if (hiddenOrg) {
        classes_i_O.push('red-text')
    }
    else {
        classes_i.push('black-text')
    }

    return (
        <div className="">
            <h1 className="center form-group col-md-12">Сервис</h1>
            <div className="center form-group col-md-12">
                <ul className="service">
                    {formNewUser()}
                    {formNewOrganization()}
                    {formUsers()}
                    
                </ul>
            </div>
        </div>
    );
};
export default observer(ServiceForm);
