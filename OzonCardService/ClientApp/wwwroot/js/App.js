import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { observer } from 'mobx-react-lite';
import { useContext, useEffect } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Context } from '.';
import LoginForm from './components/LoginForm';
import NavMenu from './components/NavMenu';
import UploadForm from './components/UploadForm';
import './custom.css';
import FilesForm from './components/FilesForm';
import ServiceForm from './components/ServiceForm';
import ReportForm from './components/ReportForm';
import TasksForm from './components/TasksForm';
const App = () => {
    const { loginstore } = useContext(Context);
    useEffect(() => {
        if (localStorage.getItem('token')) {
            loginstore.checkAuth();
        }
    }, []);
    if (loginstore.isLoading) {
        return _jsx("div", { children: "Loading..." }, void 0);
    }
    if (!loginstore.isAuth) {
        return (_jsx("div", { children: _jsx(LoginForm, {}, void 0) }, void 0));
    }
    return (_jsxs(BrowserRouter, { children: [_jsx(NavMenu, {}, void 0), _jsx("div", Object.assign({ className: "container" }, { children: _jsxs(Routes, { children: [_jsx(Route, { path: '/', element: _jsx(UploadForm, {}, void 0) }, void 0), _jsx(Route, { path: '/file', element: _jsx(FilesForm, {}, void 0) }, void 0), _jsx(Route, { path: '/report', element: _jsx(ReportForm, {}, void 0) }, void 0), _jsx(Route, { path: '/task', element: _jsx(TasksForm, {}, void 0) }, void 0), _jsx(Route, { path: '/service', element: _jsx(ServiceForm, {}, void 0) }, void 0)] }, void 0) }), void 0)] }, void 0));
};
export default observer(App);
