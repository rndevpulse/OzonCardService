import * as React from 'react'
import { FC, useContext, useState } from 'react'
import { Context } from '../index';
import { observer } from 'mobx-react-lite';


const LoginForm: FC = () => {
    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const { loginstore } = useContext(Context)
    return (
        <div className="form-group col-md-4">
            <h1>LoginForm</h1>
            <input
                className=""
                onChange={e => setEmail(e.target.value)}
                value={email}
                type='text'
                placeholder='Email'
            />
            <input
                className=""
                onChange={e => setPassword(e.target.value)}
                value={password}
                type='text'
                placeholder='Password'
            />
            <button
                onClick={() => loginstore.login(email, password)}>
                Login
            </button>
        </div>
    )
};

export default observer(LoginForm)
