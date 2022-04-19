import 'bootstrap/dist/css/bootstrap.css';
import  * as React from 'react';
import * as ReactDOM from 'react-dom';
import  App   from './App';
import LoginStore from './store/LoginStore';

interface IStoreState {
    store: LoginStore 
}
const store = new LoginStore();
export const Context = React.createContext<IStoreState>({
    store
})
console.log(`index store = ${store.email} ${store.isAuth}`)
ReactDOM.render(
    <Context.Provider value={{ store }} >
            <App />,
    </Context.Provider>,
    document.getElementById('root'));