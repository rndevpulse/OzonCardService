var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useContext, useState } from 'react';
import UserService from '../services/UserService';
import { useEffect } from 'react';
import { Context } from '..';
import { observer } from 'mobx-react-lite';
const ServiceForm = () => {
    const { organizationstore } = useContext(Context);
    const [user, setUser] = useState('');
    const [organizationLogin, setOrganizationLogin] = useState('');
    const [organizationPassword, setOrganizationPassword] = useState('');
    const [organizationId, setOrganizationId] = useState('');
    const [hidden, setHidden] = useState(false);
    const [hiddenOrg, setHiddenOrg] = useState(false);
    const [password, setPassword] = useState('');
    const [ruleBasic, setRuleBasic] = useState(true);
    const [ruleReport, setRuleReport] = useState(true);
    const [users, setUsers] = useState([]);
    function createUser() {
        return __awaiter(this, void 0, void 0, function* () {
            const rules = [];
            if (ruleBasic) {
                rules.push(101);
            }
            if (ruleReport) {
                rules.push(111);
            }
            yield UserService.createUser(user, password, rules);
            getUsers();
            confirm('Пользователь добавлен');
        });
    }
    function createOrganization() {
        return __awaiter(this, void 0, void 0, function* () {
            yield organizationstore.createOrganization(organizationLogin, organizationPassword);
            confirm('Организация добавлена');
        });
    }
    const CustomSelect = ({ id, value, options, onChange }) => {
        return (_jsx("select", Object.assign({ className: "custom-select", id: id, value: value, onChange: onChange }, { children: options.map(option => _jsx("option", Object.assign({ value: option.id }, { children: option.name }), option.id)) }), void 0));
    };
    function getUsers() {
        return __awaiter(this, void 0, void 0, function* () {
            const response = yield UserService.getUsers();
            console.log('users: ', response.data);
            setUsers(response.data);
        });
    }
    useEffect(() => {
        getUsers();
    }, []);
    const classes_i = ['password material-icons'];
    if (hidden) {
        classes_i.push('red-text');
    }
    const classes_i_O = ['password material-icons'];
    if (hiddenOrg) {
        classes_i_O.push('red-text');
    }
    else {
        classes_i.push('black-text');
    }
    return (_jsxs("div", { children: [_jsx("h1", { children: "\u0421\u0435\u0440\u0432\u0438\u0441" }, void 0), _jsx("div", Object.assign({ className: "form-group col-md-10" }, { children: _jsxs("ul", Object.assign({ className: "service" }, { children: [_jsxs("li", { children: [_jsxs("dt", { children: [_jsxs("label", Object.assign({ htmlFor: "user" }, { children: ["Email \u043D\u043E\u0432\u043E\u0433\u043E \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044F", _jsx("input", { id: 'user', onChange: e => setUser(e.target.value), value: user, type: 'text', placeholder: 'Email' }, void 0)] }), void 0), _jsxs("span", { children: [_jsxs("label", Object.assign({ htmlFor: "password" }, { children: ["\u041F\u0430\u0440\u043E\u043B\u044C \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044F", _jsx("input", { id: 'password', onChange: e => setPassword(e.target.value), value: password, type: !hidden ? 'password' : 'text', placeholder: 'Password' }, void 0)] }), void 0), _jsx("i", Object.assign({ className: classes_i.join(' '), onClick: () => setHidden(!hidden) }, { children: "remove_red_eye" }), void 0)] }, void 0), _jsx("button", Object.assign({ className: "btn-primary button", onClick: createUser }, { children: "\u0421\u043E\u0437\u0434\u0430\u0442\u044C" }), void 0)] }, void 0), _jsx("dd", { children: _jsxs("ul", { children: [_jsx("li", { children: _jsxs("label", Object.assign({ htmlFor: "rule_basic", className: "label-checkbox" }, { children: [_jsx("input", { id: 'rule_basic', type: 'checkbox', checked: ruleBasic, onChange: () => setRuleBasic(!ruleBasic) }, void 0), "\u0412\u044B\u0433\u0440\u0443\u0437\u043A\u0430 \u043A\u043B\u0438\u0435\u043D\u0442\u043E\u0432 \u0432 iikoBiz", _jsx("i", Object.assign({ className: "material-icons red-text" }, { children: ruleBasic ? 'check_box' : 'check_box_outline_blank' }), void 0)] }), void 0) }, void 0), _jsx("li", { children: _jsxs("label", Object.assign({ htmlFor: "rule_report", className: "label-checkbox" }, { children: [_jsx("input", { id: 'rule_report', type: 'checkbox', checked: ruleReport, onChange: () => setRuleReport(!ruleReport) }, void 0), "\u041F\u043E\u0441\u0442\u0440\u043E\u0435\u043D\u0438\u0435 \u043E\u0442\u0447\u0435\u0442\u043E\u0432 \u0438\u0437 iikoBiz", _jsx("i", Object.assign({ className: "material-icons red-text" }, { children: ruleReport ? 'check_box' : 'check_box_outline_blank' }), void 0)] }), void 0) }, void 0)] }, void 0) }, void 0)] }, void 0), _jsxs("li", { children: [_jsxs("dt", { children: [_jsxs("label", Object.assign({ htmlFor: "organization_login" }, { children: ["Api \u043B\u043E\u0433\u0438\u043D \u043D\u043E\u0432\u043E\u0439 \u043E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438", _jsx("input", { id: 'organization_login', onChange: e => setOrganizationLogin(e.target.value), value: organizationLogin, type: 'text', placeholder: 'Login API' }, void 0)] }), void 0), _jsxs("span", { children: [_jsxs("label", Object.assign({ htmlFor: "organization_password" }, { children: ["Api \u043F\u0430\u0440\u043E\u043B\u044C \u043E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438", _jsx("input", { id: 'organization_password', onChange: e => setOrganizationPassword(e.target.value), value: organizationPassword, type: !hiddenOrg ? 'password' : 'text', placeholder: 'Password' }, void 0)] }), void 0), _jsx("i", Object.assign({ className: classes_i_O.join(' '), onClick: () => setHiddenOrg(!hiddenOrg) }, { children: "remove_red_eye" }), void 0)] }, void 0), _jsx("button", Object.assign({ className: "btn-primary button", onClick: createOrganization }, { children: "\u0421\u043E\u0437\u0434\u0430\u0442\u044C" }), void 0)] }, void 0), _jsx("dd", {}, void 0)] }, void 0), users && users.map(u => {
                            return (_jsxs("li", { children: [_jsxs("dt", { children: [_jsx("label", { children: u.mail }, void 0), _jsx("span", { children: _jsx(CustomSelect, { id: "user_organization", value: organizationId, options: organizationstore.organizations, onChange: event => setOrganizationId(event.target.value) }, void 0) }, void 0), _jsx("button", Object.assign({ className: "btn-primary button" }, { children: "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C" }), void 0)] }, void 0), _jsx("dd", { children: _jsx("ul", { children: u && u.organizations.map(o => {
                                                return (_jsx("li", { children: _jsx("label", Object.assign({ className: "label-checkbox" }, { children: o.name }), void 0) }, o.id));
                                            }) }, void 0) }, void 0)] }, u.id));
                        })] }), void 0) }), void 0)] }, void 0));
};
export default observer(ServiceForm);
