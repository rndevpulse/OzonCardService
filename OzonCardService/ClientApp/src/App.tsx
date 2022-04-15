import * as React from 'react';
import { FC, useContext, useEffect } from 'react'
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { Context } from '.';
import FilesForm from './components/FilesForm';
import  LoginForm  from './components/LoginForm';


import { NavMenu } from './components/NavMenu';
import UploadForm from './components/UploadForm';

import './custom.css'


const App: FC = () => {
    //const { } = useContext(Context);
    //useEffect(() => {

    //},[])
    return (
        <BrowserRouter>
            <NavMenu />
            <div className="container">
                <Switch>
                    <Route path='/' component={UploadForm} exact/>
                    <Route path='/auth' component={LoginForm} />
                    <Route path='/files' component={FilesForm} />
                </Switch>
            </div>
        </BrowserRouter>
    );
}
export default App;