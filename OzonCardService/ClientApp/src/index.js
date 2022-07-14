"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Context = void 0;
require("bootstrap/dist/css/bootstrap.css");
var React = require("react");
var ReactDOM = require("react-dom");
var App_1 = require("./App");
var LoginStore_1 = require("./store/LoginStore");
var OrganizationStore_1 = require("./store/OrganizationStore");
var TaskStore_1 = require("./store/TaskStore");
var loginstore = new LoginStore_1.default();
var organizationstore = new OrganizationStore_1.default();
var taskstore = new TaskStore_1.default();
exports.Context = React.createContext({
    loginstore: loginstore,
    organizationstore: organizationstore,
    taskstore: taskstore
});
ReactDOM.render(React.createElement(exports.Context.Provider, { value: { loginstore: loginstore, organizationstore: organizationstore, taskstore: taskstore } },
    React.createElement(App_1.default, null)), document.getElementById('root'));
//# sourceMappingURL=index.js.map