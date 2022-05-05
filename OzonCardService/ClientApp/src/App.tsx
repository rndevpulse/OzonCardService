import * as React from 'react'
import { observer } from 'mobx-react-lite';
import { FC, useContext, useEffect } from 'react'
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Context } from '.';

import LoginForm  from './components/LoginForm';
import NavMenu from './components/NavMenu';
import UploadForm from './components/UploadForm';

import '../src/css/custom.css'
import FilesForm from './components/FilesForm';
import ServiceForm from './components/ServiceForm';
import ReportForm from './components/ReportForm';
import TasksForm from './components/TasksForm';


const App: FC = () => {
    const { loginstore } = useContext(Context);
    useEffect(() => {
        if (localStorage.getItem('token')) {
            loginstore.checkAuth()
        }
    }, [])
    if (loginstore.isLoading) {
        return <div>Loading...</div>
    }

    if (!loginstore.isAuth) {
        return (
            <div>
                <LoginForm />
            </div>
        )
    }
    return (
        <BrowserRouter>
            <NavMenu />
            <div className="container">
                <Routes>
                    <Route path='/' element={<UploadForm/>} />
                    <Route path='/file' element={<FilesForm/>} />
                    <Route path='/report' element={<ReportForm />} />
                    <Route path='/task' element={<TasksForm />} />
                    <Route path='/service' element={<ServiceForm />} />
                </Routes>
            </div>
        </BrowserRouter>
    )
};

export default observer(App)


