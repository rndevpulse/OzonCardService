import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import LoginStore from "./stores/LoginStore";
import OrganizationStore from "./stores/OrganizationStore";
import TaskStore from "./stores/TaskStore";
import {ModalState} from "./context/modal";


interface IStoreState {
    loginStore: LoginStore
    organizationStore: OrganizationStore
    taskStore: TaskStore
}
const loginStore = new LoginStore();
const organizationStore = new OrganizationStore();
const taskStore = new TaskStore();


export const Context = React.createContext<IStoreState>({
    loginStore, organizationStore , taskStore
})

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
    <Context.Provider value={{ loginStore, organizationStore , taskStore}} >
        <ModalState>
            <App />
        </ModalState>
    </Context.Provider>
)


