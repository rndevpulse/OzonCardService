"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var react_1 = require("react");
var React = require("react");
var react_router_dom_1 = require("react-router-dom");
var reactstrap_1 = require("reactstrap");
var __1 = require("..");
var mobx_react_lite_1 = require("mobx-react-lite");
require("../css/NavMenu.css");
var useOnClickOutside = function (ref, closeMenu) {
    react_1.useEffect(function () {
        var listener = function (event) {
            if (ref.current && event.target &&
                ref.current.contains(event.target)) {
                return;
            }
            closeMenu();
        };
        document.addEventListener("mousedown", listener);
        return function () {
            document.removeEventListener("mousedown", listener);
        };
    }, [ref, closeMenu]);
};
var NavMenu = function () {
    var loginstore = react_1.useContext(__1.Context).loginstore;
    var navigate = react_router_dom_1.useNavigate();
    var _a = react_1.useState(false), open = _a[0], setOpen = _a[1];
    var close = function () { return setOpen(false); };
    var node = react_1.useRef(null);
    useOnClickOutside(node, function () { return setOpen(false); });
    var links = [];
    var StyledMenu = ['styledMenu'];
    var Menu = ['material-icons'];
    if (open) {
        StyledMenu.push('open');
        Menu.push('black-text ');
    }
    else {
        Menu.push('red-text ');
    }
    function linkBasic() {
        if (loginstore.rules.includes('101')) {
            links.push({ link: '/', name: 'Выгрузка' });
            links.push({ link: '/search_customer', name: 'Поиск' });
            return (React.createElement("div", null,
                React.createElement(reactstrap_1.NavItem, null,
                    React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: "/" }, "\u0412\u044B\u0433\u0440\u0443\u0437\u043A\u0430")),
                React.createElement(reactstrap_1.NavItem, null,
                    React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: "/search_customer" }, "\u041F\u043E\u0438\u0441\u043A"))));
        }
    }
    ;
    function linkReport() {
        if (loginstore.rules.includes('111')) {
            links.push({ link: '/report', name: 'Отчеты' });
            return (React.createElement(reactstrap_1.NavItem, null,
                React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: "/report" }, "\u041E\u0442\u0447\u0435\u0442\u044B")));
        }
    }
    ;
    function linkAdmin() {
        if (loginstore.rules.includes('100')) {
            links.push({ link: '/service', name: 'Сервис' });
            return (React.createElement(reactstrap_1.NavItem, null,
                React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: "/service" }, "\u0421\u0435\u0440\u0432\u0438\u0441")));
        }
    }
    ;
    function onClickLink(l) {
        close();
        if (l.link === '/logout')
            loginstore.logout();
        else
            navigate(l.link);
    }
    var getLinks = function () {
        links.push({ link: '/file', name: 'Файлы' });
        links.push({ link: '/task', name: 'Задачи' });
        links.push({ link: '/logout', name: 'Выход' });
        return (React.createElement("div", { className: StyledMenu.join(' '), ref: node }, links && links.map(function (l) {
            return (React.createElement("div", { className: "styledLink ", key: l.name, onClick: function (e) { return onClickLink(l); } }, l.name));
        })));
    };
    return (React.createElement("header", null,
        React.createElement(reactstrap_1.Navbar, { className: "navbar-expand navbar-toggleable-sm border-bottom box-shadow mb-3 grey lighten-3 col s12 m2", light: true },
            React.createElement(reactstrap_1.Container, null,
                React.createElement("div", null,
                    React.createElement("img", { src: "../logo.png", alt: "logo", className: "logo_image" }),
                    React.createElement(reactstrap_1.NavbarBrand, { tag: react_router_dom_1.Link, to: "/" }, "Corporate Catering Card Service ")),
                React.createElement("ul", { className: "navbar-nav__custom navbar-nav flex-grow" },
                    linkBasic(),
                    linkReport(),
                    React.createElement(reactstrap_1.NavItem, null,
                        React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: "/file" }, "\u0424\u0430\u0439\u043B\u044B")),
                    React.createElement(reactstrap_1.NavItem, null,
                        React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, className: "text-dark", to: "/task" }, "\u0417\u0430\u0434\u0430\u0447\u0438")),
                    linkAdmin(),
                    React.createElement(reactstrap_1.NavItem, null,
                        React.createElement(reactstrap_1.NavLink, { tag: react_router_dom_1.Link, to: "", className: "text-dark logout", onClick: function () { return loginstore.logout(); } }, "\u0412\u044B\u0445\u043E\u0434"))),
                React.createElement("div", { className: "text-dark navbar-nav__button" },
                    React.createElement("i", { className: Menu.join(' '), onClick: function () { return setOpen(!open); } }, "menu"),
                    getLinks())))));
};
exports.default = mobx_react_lite_1.observer(NavMenu);
//# sourceMappingURL=NavMenu.js.map