import 'bootstrap/dist/css/bootstrap.css';
import  * as React from 'react';
import * as ReactDOM from 'react-dom';
import  App   from './App';
import LoginStore from './store/LoginStore';
import OrganizationStore from './store/OrganizationStore';
import TaskStore from './store/TaskStore';

interface IStoreState {
    loginstore: LoginStore,
    organizationstore: OrganizationStore,
    taskstore: TaskStore
}
const loginstore = new LoginStore();
const organizationstore = new OrganizationStore();
const taskstore = new TaskStore();

export const Context = React.createContext<IStoreState>({
    loginstore, organizationstore, taskstore
})

ReactDOM.render(
    <Context.Provider value={{ loginstore, organizationstore, taskstore }} >
            <App />,
    </Context.Provider>,
    document.getElementById('root'));