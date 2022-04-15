import 'bootstrap/dist/css/bootstrap.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { ConnectedRouter } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import App from './App';
import LoginStore from './store/LoginStore';

// Create browser history to use in the Redux store
//const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
//const history = createBrowserHistory({ basename: baseUrl });





interface IStoreState {
    store: LoginStore 
}
const store = new LoginStore();
export const Context = React.createContext<IStoreState>({
    store
})

ReactDOM.render(
    <Context.Provider value={{ store }} >
            <App />
    </Context.Provider>,
    document.getElementById('root'));

//registerServiceWorker();