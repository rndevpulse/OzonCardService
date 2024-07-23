import * as React from 'react'
import { FC, useContext, useState } from 'react'
import { Context } from '../../index';
import { observer } from 'mobx-react-lite';
import {toast, ToastContainer} from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css';
import './index.css'


const LoginPage: FC = () => {
    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const { loginStore } = useContext(Context)

    const [hidden, setHidden] = useState(false)

    const classes_i = ['password material-icons']
    if (hidden) {
        classes_i.push('red-text')
    }
    else {
        classes_i.push('black-text')
    }
    return (
        <div className="center form-group  col-md-6">
            <ToastContainer/>
            <h5>Corporate Catering Card Service</h5>
            <h1>Authorization</h1>
            <br/>
            <div className="autorization">
                <label htmlFor='email' className="">
                    Email
                </label>
                <input
                    id='email'
                    className="autorization__email"
                    onChange={e => setEmail(e.target.value)}
                    value={email}
                    type='text'
                    placeholder='Email'
                />
                <label htmlFor='pass' className="">
                    Password
                </label>
                <input
                    id='pass'
                    className=""
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
            <button className="btn-primary button_login"
                    onClick={() => loginStore.login(email, password)}>
                Login
            </button>
        </div>
    )
};

export default observer(LoginPage)
