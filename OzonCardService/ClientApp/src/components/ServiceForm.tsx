import { FC, useContext, useState } from 'react';
import * as React from 'react'
import UserService from '../services/UserService';
import { useEffect } from 'react';
import { IUserResponce } from '../models/IUserResponse';
import { Context } from '..';
import { observer } from 'mobx-react-lite';

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

    async function createUser() {
        const rules: number[] = []
        if (ruleBasic) { rules.push(101)}
        if (ruleReport) { rules.push(111) }
        await UserService.createUser(user, password, rules)
        getUsers()
        confirm('Пользователь добавлен')
    }
    async function createOrganization() {
        await organizationstore.createOrganization(organizationLogin, organizationPassword)
        confirm('Организация добавлена')

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
    async function getUsers() {
        const response = await UserService.getUsers()
        console.log('users: ', response.data)
        setUsers(response.data)
    }
    useEffect(() => {
        getUsers()
    },[])

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
        <div>
            <h1>Сервис</h1>
            <div className="form-group col-md-10">
                <ul className="service">
                    <li>
                        <dt>
                        <label htmlFor="user">
                            Email нового пользователя
                                <input
                                    id='user'
                                    onChange={e => setUser(e.target.value)}
                                    value={user}
                                    type='text'
                                    placeholder='Email'
                                />
                            </label>
                            <span>
                                <label htmlFor="password">
                                    Пароль пользователя
                                <input
                                    id='password'
                                    onChange={e => setPassword(e.target.value)}
                                    value={password}
                                        type={!hidden ? 'password' : 'text'}
                                    placeholder='Password'
                                
                                />
                                </label>
                                <i className={classes_i.join(' ')}
                                    onClick={() => setHidden(!hidden) }>                                
                                    remove_red_eye
                                    </i>
                            </span>
                                    <button className="btn-primary button"
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
                    <li>
                        <dt>
                            <label htmlFor="organization_login">
                                Api логин новой организации
                                <input
                                    id='organization_login'
                                    onChange={e => setOrganizationLogin(e.target.value)}
                                    value={organizationLogin}
                                    type='text'
                                    placeholder='Login API'
                                />
                            </label>
                            <span>
                                <label htmlFor="organization_password">
                                    Api пароль организации
                                    <input
                                        id='organization_password'
                                        onChange={e => setOrganizationPassword(e.target.value)}
                                        value={organizationPassword}
                                        type={!hiddenOrg ? 'password' : 'text'}
                                        placeholder='Password'

                                    />
                                </label>
                                <i className={classes_i_O.join(' ')}
                                    onClick={() => setHiddenOrg(!hiddenOrg)}>
                                    remove_red_eye
                                </i>
                            </span>
                            <button className="btn-primary button"
                                onClick={createOrganization}>
                                Создать
                            </button>
                        </dt>
                        <dd></dd>
                    </li>
                    {users && users.map(u => {

                        return (
                            <li key={u.id}>
                                <dt>
                                    <label>
                                        {u.mail}
                                    </label>
                                    <span>
                                    <CustomSelect id="user_organization"
                                        value={organizationId}
                                        options={organizationstore.organizations}
                                        onChange={event => setOrganizationId(event.target.value)} />
                                    </span>
                                            <button className="btn-primary button"
                                        >
                                        Добавить
                                    </button>
                                </dt>
                                <dd>
                                    <ul>
                                        {u && u.organizations.map(o => {
                                            return (
                                                <li key={o.id}>
                                                    <label className="label-checkbox">{o.name }</label>
                                                </li>
                                                )
                                        })}
                                    </ul>
                                </dd>
                            </li>
                        )
                    })
                    }
                </ul>
            </div>
        </div>
    );
};
export default observer(ServiceForm);
