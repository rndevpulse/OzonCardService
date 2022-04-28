import * as React from 'react'
import { FC, useContext, useState } from 'react'
import { Context } from '../index';
import { observer } from 'mobx-react-lite';



const LoginForm: FC = () => {
    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const { loginstore } = useContext(Context)
    return (
        <div className="form-group col-md-4 center">
            <h1>Authorization</h1>
            <label htmlFor='email'>
                Email
                <input 
                    id='email'
                    className=""
                    onChange={e => setEmail(e.target.value)}
                    value={email}
                    type='text'
                    placeholder='Email'
                    />
            </label>
            <br/>
            <label htmlFor='pass'>
                Password
                <input
                    id='pass'
                    className=""
                    onChange={e => setPassword(e.target.value)}
                    value={password}
                    type='text'
                    placeholder='Password'
                    />
            </label>
            <br/>
            <button
                onClick={() => loginstore.login(email, password)}>
                Login
            </button>
        </div>
    )
};

export default observer(LoginForm)
