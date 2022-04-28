import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useContext } from 'react';
import { Link } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Context } from '..';
import './NavMenu.css';
import { observer } from 'mobx-react-lite';
const NavMenu = () => {
    const { loginstore } = useContext(Context);
    console.log('NavMenu store.rules = ', loginstore.rules.toString());
    function linkBasic() {
        if (loginstore.rules.includes('101'))
            return (_jsx(NavItem, { children: _jsx(NavLink, Object.assign({ tag: Link, className: "text-dark", to: "/" }, { children: "UploadForm" }), void 0) }, void 0));
    }
    ;
    function linkReport() {
        if (loginstore.rules.includes('111'))
            return (_jsx(NavItem, { children: _jsx(NavLink, Object.assign({ tag: Link, className: "text-dark", to: "/report" }, { children: "ReportForm" }), void 0) }, void 0));
    }
    ;
    function linkAdmin() {
        if (loginstore.rules.includes('100'))
            return (_jsx(NavItem, { children: _jsx(NavLink, Object.assign({ tag: Link, className: "text-dark", to: "/service" }, { children: "ServiceForm" }), void 0) }, void 0));
    }
    ;
    return (_jsx("header", { children: _jsx(Navbar, Object.assign({ className: "navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 grey lighten-3 col s12 m2", light: true }, { children: _jsxs(Container, { children: [_jsx(NavbarBrand, Object.assign({ tag: Link, to: "/" }, { children: "OzonCardService" }), void 0), _jsxs("ul", Object.assign({ className: "navbar-nav flex-grow" }, { children: [linkBasic(), _jsx(NavItem, { children: _jsx(NavLink, Object.assign({ tag: Link, className: "text-dark", to: "/file" }, { children: "FilesForm" }), void 0) }, void 0), linkReport(), _jsx(NavItem, { children: _jsx(NavLink, Object.assign({ tag: Link, className: "text-dark", to: "/task" }, { children: "TasksForm" }), void 0) }, void 0), linkAdmin(), _jsx(NavItem, { children: _jsx(NavLink, Object.assign({ tag: Link, to: "", className: "text-dark", onClick: () => loginstore.logout() }, { children: "Logout" }), void 0) }, void 0)] }), void 0)] }, void 0) }), void 0) }, void 0));
};
export default observer(NavMenu);
