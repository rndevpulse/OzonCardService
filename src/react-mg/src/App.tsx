import React, {useContext, useEffect} from 'react';
import './App.css';
import { observer } from 'mobx-react-lite';
import { BrowserRouter, Route, Routes } from 'react-router-dom';

import {Context} from "./index";
import NavMenu from './components/menu';

import LoginPage  from './pages/login';
import UploadPage from './pages/upload';
import {Container} from "reactstrap";





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
            {/*<Route path='/file' element={<FilesForm/>} />*/}
            {/*<Route path='/report' element={<ReportForm />} />*/}
            {/*<Route path='/task' element={<TasksForm />} />*/}
            {/*<Route path='/service' element={<ServiceForm />} />*/}
            {/*<Route path='/search_customer' element={<SearchCustomerForm />} />*/}


          </Routes>
        </Container>
      </BrowserRouter>
  );
}

export default observer(App);
