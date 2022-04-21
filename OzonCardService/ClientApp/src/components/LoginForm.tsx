import * as React from 'react'
import { FC, useContext, useState } from 'react'
import { Context } from '../index';
import { observer } from 'mobx-react-lite';


const LoginForm: FC = () => {
    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const { loginstore } = useContext(Context)
    return (
        <div>
            <h1>LoginForm</h1>
            <input
                onChange={e => setEmail(e.target.value)}
                value={email}
                type='text'
                placeholder='Email'
            />
            <input
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
