import { FC, useContext, useState, useEffect } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import './index.css'
import {Context} from "../../index";
import {IUser} from "../../models/user/IUser";
import AuthService from "../../services/AuthService";
import OrganizationService from "../../services/OrganizationServise";
import Select from "react-select";
import {IOrganization} from "../../models/org/IOrganization";



const ServicePage: FC = () => {

    const { organizationStore } = useContext(Context)


    const [user, setUser] = useState('')

    const [organizationLogin, setOrganizationLogin] = useState('')
    const [organizationPassword, setOrganizationPassword] = useState('')
    const [organization, setOrganization] = useState<IOrganization>()



    const [hidden, setHidden] = useState(false)
    const [hiddenOrg, setHiddenOrg] = useState(false)

    const [password, setPassword] = useState('')
    const [ruleBasic, setRuleBasic] = useState(true)
    const [ruleReport, setRuleReport] = useState(true)

    const [users, setUsers] = useState<IUser[]>([])
    const [filterUserEmail, setFilterUserEmail] = useState('')


    async function createUser() {
        const roles: string[] = []
        if (ruleBasic) { roles.push("Basic") }
        if (ruleReport) { roles.push("Report") }
        await AuthService.create(user, password, roles)
        getUsers()
        window.confirm('Пользователь добавлен')
    }
    async function createOrganization() {
        await organizationStore.createOrganization(organizationLogin, organizationPassword)
        window.confirm('Организация добавлена')

    }
    async function addUserForOrganization(userId: string){
        await OrganizationService.addUserForOrganization(organization!.id, userId)
        await getUsers()
    }
    async function delUserForOrganization(userId: string) {
        await OrganizationService.delUserForOrganization(organization!.id, userId)
        await getUsers()
    }

    async function getUsers() {
        const response = await AuthService.getUsers()
        console.log('users: ', response.data)
        setUsers(response.data)
        console.log('organizationStore.organizations.length = ', organizationStore.organizations.length)
        if (organizationStore.organizations.length === 0) {
            await organizationStore.requestOrganizations()
        }
        setOrganization(organizationStore.organizations[0])
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
                    <Select
                        id="user_organization"
                        value={organization}
                        options={organizationStore.organizations}
                        getOptionLabel={option => option.name}
                        getOptionValue={option => option.id}
                        onChange={value=>setOrganization(value as IOrganization)}
                    />
                </span>
            </dt>
            <dd></dd>

            <ul>
                {users && users.filter(u => u.email.includes(filterUserEmail)).map(u => {

                    return (
                        <li key={u.id}>
                            <dt>
                                <label className="label-checkbox">{u.email}</label>
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
        <div className="center form-group col-md-12">
            <h1 >Сервис</h1>
            <div>
                <ul className="service">
                    {formNewUser()}
                    {formNewOrganization()}
                    {formUsers()}
                    
                </ul>
            </div>
        </div>
    );
};
export default observer(ServicePage);
