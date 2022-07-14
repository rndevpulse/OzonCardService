"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var mobx_react_lite_1 = require("mobx-react-lite");
var react_1 = require("react");
var react_router_dom_1 = require("react-router-dom");
var _1 = require(".");
var LoginForm_1 = require("./components/LoginForm");
var NavMenu_1 = require("./components/NavMenu");
var UploadForm_1 = require("./components/UploadForm");
require("../src/css/custom.css");
var FilesForm_1 = require("./components/FilesForm");
var ServiceForm_1 = require("./components/ServiceForm");
var ReportForm_1 = require("./components/ReportForm");
var TasksForm_1 = require("./components/TasksForm");
var SearchCustomerForm_1 = require("./components/SearchCustomerForm");
var App = function () {
    var loginstore = react_1.useContext(_1.Context).loginstore;
    react_1.useEffect(function () {
        if (localStorage.getItem('token')) {
            loginstore.checkAuth();
        }
    }, []);
    if (loginstore.isLoading) {
        return React.createElement("div", null, "Loading...");
    }
    if (!loginstore.isAuth) {
        return (React.createElement("div", null,
            React.createElement(LoginForm_1.default, null)));
    }
    return (React.createElement(react_router_dom_1.BrowserRouter, null,
        React.createElement(NavMenu_1.default, null),
        React.createElement("div", { className: "container" },
            React.createElement(react_router_dom_1.Routes, null,
                React.createElement(react_router_dom_1.Route, { path: '/', element: React.createElement(UploadForm_1.default, null) }),
                React.createElement(react_router_dom_1.Route, { path: '/file', element: React.createElement(FilesForm_1.default, null) }),
                React.createElement(react_router_dom_1.Route, { path: '/report', element: React.createElement(ReportForm_1.default, null) }),
                React.createElement(react_router_dom_1.Route, { path: '/task', element: React.createElement(TasksForm_1.default, null) }),
                React.createElement(react_router_dom_1.Route, { path: '/service', element: React.createElement(ServiceForm_1.default, null) }),
                React.createElement(react_router_dom_1.Route, { path: '/search_customer', element: React.createElement(SearchCustomerForm_1.default, null) })))));
};
exports.default = mobx_react_lite_1.observer(App);
//# sourceMappingURL=App.js.map