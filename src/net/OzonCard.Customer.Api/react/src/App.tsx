import React, {useContext, useEffect} from 'react';
import './App.css';
import { observer } from 'mobx-react-lite';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
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
import PatternsPage from "./pages/patterns";
import {Loader} from "./components/loader";
import {Slide, ToastContainer} from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css';




function App() {
  const { loginStore } = useContext(Context);
  useEffect(() => {
    if (localStorage.getItem('token')) {
      loginStore.checkAuth()
    }
  }, [])

  if (loginStore.IsLoading) {
    return <Loader/>
  }

  if (!loginStore.IsAuth) {
    return <LoginPage />
  }

  return (
      <BrowserRouter>
        <NavMenu />
        <ToastContainer
            position={"bottom-right"}
            autoClose={3000}
            hideProgressBar={false}
            closeOnClick={true}
            pauseOnHover={true}
            draggable={true}
            theme={"light"}
            transition={Slide}
        />
        <Container>
          <Routes>
            <Route path='/' element={<UploadPage/>} />
            <Route path='/files' element={<FilesPage/>} />
            <Route path='/report' element={<ReportPage/>} />
            <Route path='/patterns' element={<PatternsPage/>} />
            <Route path='/tasks' element={<TasksPage/>} />
            <Route path='/service' element={<ServicePage />} />
            <Route path='/search' element={<SearchPage />} />
          </Routes>
        </Container>
      </BrowserRouter>
  );
}

export default observer(App);
