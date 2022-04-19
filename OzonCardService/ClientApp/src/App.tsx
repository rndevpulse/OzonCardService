
import { observer } from 'mobx-react-lite';
import { FC, useContext, useEffect } from 'react'
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Context } from '.';

import FilesForm from './components/FilesForm';
import LoginForm  from './components/LoginForm';
import { NavMenu } from './components/NavMenu';
import UploadForm from './components/UploadForm';

import './custom.css'


const App: FC = () => {
    const { store } = useContext(Context);
    useEffect(() => {
        console.log(`App useEffect = ${store.email} ${store.isAuth}`)

        if (localStorage.getItem('token')) {
            store.checkAuth()
        }
    }, [])
    if (!store.isAuth) {
    console.log(`App store = ${store.email} ${store.isAuth}`)
        return (
            <div>
                <LoginForm />
            </div>
        );
    }
    return (

        <BrowserRouter>
            <NavMenu />
            <div className="container">
                <Routes>
                    <Route path='/' element={<UploadForm/>} />
                    <Route path='/files' element={<FilesForm/>} />
                </Routes>
            </div>
        </BrowserRouter>
    );

};

export default observer(App)


