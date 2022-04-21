"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const jsx_runtime_1 = require("react/jsx-runtime");
const mobx_react_lite_1 = require("mobx-react-lite");
const react_1 = require("react");
const react_router_dom_1 = require("react-router-dom");
const _1 = require(".");
const LoginForm_1 = __importDefault(require("./components/LoginForm"));
const NavMenu_1 = require("./components/NavMenu");
const UploadForm_1 = __importDefault(require("./components/UploadForm"));
require("./custom.css");
const FilesForm_1 = __importDefault(require("./components/FilesForm"));
const ServiceForm_1 = __importDefault(require("./components/ServiceForm"));
const ReportForm_1 = __importDefault(require("./components/ReportForm"));
const App = () => {
    const { loginstore } = react_1.useContext(_1.Context);
    react_1.useEffect(() => {
        if (localStorage.getItem('token')) {
            loginstore.checkAuth();
        }
    }, []);
    if (loginstore.isLoading) {
        return jsx_runtime_1.jsx("div", { children: "Loading..." }, void 0);
    }
    if (!loginstore.isAuth) {
        return (jsx_runtime_1.jsx("div", { children: jsx_runtime_1.jsx(LoginForm_1.default, {}, void 0) }, void 0));
    }
    return (jsx_runtime_1.jsxs(react_router_dom_1.BrowserRouter, { children: [jsx_runtime_1.jsx(NavMenu_1.NavMenu, {}, void 0), ")", jsx_runtime_1.jsx("div", Object.assign({ className: "container" }, { children: jsx_runtime_1.jsxs(react_router_dom_1.Routes, { children: [jsx_runtime_1.jsx(react_router_dom_1.Route, { path: '/', element: jsx_runtime_1.jsx(UploadForm_1.default, {}, void 0) }, void 0), jsx_runtime_1.jsx(react_router_dom_1.Route, { path: '/file', element: jsx_runtime_1.jsx(FilesForm_1.default, {}, void 0) }, void 0), jsx_runtime_1.jsx(react_router_dom_1.Route, { path: '/report', element: jsx_runtime_1.jsx(ReportForm_1.default, {}, void 0) }, void 0), jsx_runtime_1.jsx(react_router_dom_1.Route, { path: '/service', element: jsx_runtime_1.jsx(ServiceForm_1.default, {}, void 0) }, void 0)] }, void 0) }), void 0)] }, void 0));
};
exports.default = mobx_react_lite_1.observer(App);
