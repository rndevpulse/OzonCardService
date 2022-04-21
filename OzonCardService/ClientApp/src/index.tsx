import 'bootstrap/dist/css/bootstrap.css';
import  * as React from 'react';
import * as ReactDOM from 'react-dom';
import  App   from './App';
import LoginStore from './store/LoginStore';
import OrganizationStore from './store/OrganizationStore';

interface IStoreState {
    loginstore: LoginStore,
    organizationstore: OrganizationStore
}
const loginstore = new LoginStore();
const organizationstore = new OrganizationStore()

export const Context = React.createContext<IStoreState>({
    loginstore, organizationstore
})

ReactDOM.render(
    <Context.Provider value={{ loginstore, organizationstore }} >
            <App />,
    </Context.Provider>,
    document.getElementById('root'));