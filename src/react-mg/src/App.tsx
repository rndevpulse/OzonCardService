import React, {useContext, useEffect} from 'react';
import './App.css';
import { observer } from 'mobx-react-lite';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import {Context} from "./index";
import NavMenu from './components/menu';

import LoginPage  from './pages/login';
import UploadPage from './pages/upload';
import {Container} from "reactstrap";
import FilesPage from "./pages/files";
import TasksPage from "./pages/tasks";
import ServicePage from "./pages/service";
import ReportPage from "./pages/report";
import SearchPage from "./pages/search";





function App() {
  const { loginStore } = useContext(Context);
  useEffect(() => {
    if (localStorage.getItem('token')) {
      loginStore.checkAuth()
    }
  }, [])
  if (loginStore.IsLoading) {
    return <div>Loading...</div>
  }

  if (!loginStore.IsAuth) {
    return (
        <div>
          <LoginPage />
        </div>
    )
  }

  return (
      <BrowserRouter>
        <NavMenu />
        <Container>
          <Routes>
            <Route path='/' element={<UploadPage/>} />
            <Route path='/files' element={<FilesPage/>} />
            <Route path='/report' element={<ReportPage/>} />
            <Route path='/tasks' element={<TasksPage/>} />
            <Route path='/service' element={<ServicePage />} />
            <Route path='/search_customer' element={<SearchPage />} />


          </Routes>
        </Container>
      </BrowserRouter>
  );
}

export default observer(App);
