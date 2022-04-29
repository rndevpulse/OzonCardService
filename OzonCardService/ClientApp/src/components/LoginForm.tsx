import * as React from 'react'
import { FC, useContext, useState } from 'react'
import { Context } from '../index';
import { observer } from 'mobx-react-lite';



const LoginForm: FC = () => {
    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const { loginstore } = useContext(Context)

    const [hidden, setHidden] = useState(false)

    const classes_i = ['password material-icons']
    if (hidden) {
        classes_i.push('red-text')
    }
    else {
        classes_i.push('black-text')
    }
    return (
        <div className="form-group col-md-12 center">
            <h1>Authorization</h1>
            <br/>
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
            <br />
            <span>
            <label htmlFor='pass'>
                Password
                <input
                    id='pass'
                    className=""
                    onChange={e => setPassword(e.target.value)}
                    value={password}
                    type={!hidden ? 'password' : 'text'}
                    placeholder='Password'
                    />
            </label>
            <i className={classes_i.join(' ')}
                onClick={() => setHidden(!hidden)}>
                remove_red_eye
            </i>
            </span>
                    <br />
            <button
                onClick={() => loginstore.login(email, password)}>
                Login
            </button>
        </div>
    )
};

export default observer(LoginForm)
