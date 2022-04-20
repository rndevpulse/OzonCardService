"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.NavMenu = void 0;
const jsx_runtime_1 = require("react/jsx-runtime");
const react_1 = require("react");
const react_router_dom_1 = require("react-router-dom");
const reactstrap_1 = require("reactstrap");
const __1 = require("..");
require("./NavMenu.css");
const NavMenu = () => {
    const { store } = react_1.useContext(__1.Context);
    console.log('store.rules = ', store.rules.toString());
    function linkBasic() {
        if (store.rules.includes('101'))
            return (jsx_runtime_1.jsx(reactstrap_1.NavItem, { children: jsx_runtime_1.jsx(reactstrap_1.NavLink, Object.assign({ tag: react_router_dom_1.Link, className: "text-dark", to: "/" }, { children: "UploadForm" }), void 0) }, void 0));
    }
    ;
    function linkReport() {
        if (store.rules.includes('111'))
            return (jsx_runtime_1.jsx(reactstrap_1.NavItem, { children: jsx_runtime_1.jsx(reactstrap_1.NavLink, Object.assign({ tag: react_router_dom_1.Link, className: "text-dark", to: "/report" }, { children: "ReportForm" }), void 0) }, void 0));
    }
    ;
    function linkAdmin() {
        if (store.rules.includes('100'))
            return (jsx_runtime_1.jsx(reactstrap_1.NavItem, { children: jsx_runtime_1.jsx(reactstrap_1.NavLink, Object.assign({ tag: react_router_dom_1.Link, className: "text-dark", to: "/service" }, { children: "ServiceForm" }), void 0) }, void 0));
    }
    ;
    return (jsx_runtime_1.jsx("header", { children: jsx_runtime_1.jsx(reactstrap_1.Navbar, Object.assign({ className: "navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3", light: true }, { children: jsx_runtime_1.jsxs(reactstrap_1.Container, { children: [jsx_runtime_1.jsx(reactstrap_1.NavbarBrand, Object.assign({ tag: react_router_dom_1.Link, to: "/" }, { children: "OzonCardService" }), void 0), jsx_runtime_1.jsxs("ul", Object.assign({ className: "navbar-nav flex-grow" }, { children: [linkBasic(), jsx_runtime_1.jsx(reactstrap_1.NavItem, { children: jsx_runtime_1.jsx(reactstrap_1.NavLink, Object.assign({ tag: react_router_dom_1.Link, className: "text-dark", to: "/file" }, { children: "FilesForm" }), void 0) }, void 0), linkReport(), linkAdmin(), jsx_runtime_1.jsx(reactstrap_1.NavItem, { children: jsx_runtime_1.jsx(reactstrap_1.NavLink, Object.assign({ tag: react_router_dom_1.Link, to: "", className: "text-dark", onClick: () => store.logout() }, { children: "Logout" }), void 0) }, void 0)] }), void 0)] }, void 0) }), void 0) }, void 0));
};
exports.NavMenu = NavMenu;
