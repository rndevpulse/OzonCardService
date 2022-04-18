
import { observer, useObserver } from 'mobx-react-lite';
import React, { FC, useContext, useEffect } from 'react'
//import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { Context } from '.';
//import FilesForm from './components/FilesForm';
import { LoginForm }  from './components/LoginForm';


//import { NavMenu } from './components/NavMenu';
//import UploadForm from './components/UploadForm';

import './custom.css'


export const App: FC = observer(() => {
    const { store } = useContext(Context);
    useEffect(() => {
        console.log(`App useEffect = ${store.email} ${store.isAuth}`)

        if (localStorage.getItem('token')) {
            store.checkAuth()
        }
    }, [])
    console.log(`App store = ${store.email} ${store.isAuth}`)
    if (!store.isAuth) {
        return (
            <div>
                <LoginForm />

            </div>
        );
    }
    return (
        <div>
            <h1>{store.isAuth ? `autorize ${store.email}` : ' not autorize'}</h1>
            <button onClick={() => store.logout()}>Logout</button>
        </div>
        //<BrowserRouter>
        //    <NavMenu />
        //    <div className="container">
        //        <Switch>
        //            <Route path='/' component={UploadForm} exact/>
        //            <Route path='/files' component={FilesForm} />
        //            <button onClick={() => store.logout() }>Logout</button>
        //        </Switch>
        //    </div>
        //</BrowserRouter>
    );

})


