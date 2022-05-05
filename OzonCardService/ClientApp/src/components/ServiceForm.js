"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var react_1 = require("react");
var React = require("react");
var UserService_1 = require("../services/UserService");
var react_2 = require("react");
var __1 = require("..");
var mobx_react_lite_1 = require("mobx-react-lite");
require("../css/ServiceForm.css");
var ServiceForm = function () {
    var organizationstore = react_1.useContext(__1.Context).organizationstore;
    var _a = react_1.useState(''), user = _a[0], setUser = _a[1];
    var _b = react_1.useState(''), organizationLogin = _b[0], setOrganizationLogin = _b[1];
    var _c = react_1.useState(''), organizationPassword = _c[0], setOrganizationPassword = _c[1];
    var _d = react_1.useState(''), organizationId = _d[0], setOrganizationId = _d[1];
    var _e = react_1.useState(false), hidden = _e[0], setHidden = _e[1];
    var _f = react_1.useState(false), hiddenOrg = _f[0], setHiddenOrg = _f[1];
    var _g = react_1.useState(''), password = _g[0], setPassword = _g[1];
    var _h = react_1.useState(true), ruleBasic = _h[0], setRuleBasic = _h[1];
    var _j = react_1.useState(true), ruleReport = _j[0], setRuleReport = _j[1];
    var _k = react_1.useState([]), users = _k[0], setUsers = _k[1];
    var _l = react_1.useState(''), filterUserEmail = _l[0], setFilterUserEmail = _l[1];
    function createUser() {
        return __awaiter(this, void 0, void 0, function () {
            var rules;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        rules = [];
                        if (ruleBasic) {
                            rules.push(101);
                        }
                        if (ruleReport) {
                            rules.push(111);
                        }
                        return [4 /*yield*/, UserService_1.default.createUser(user, password, rules)];
                    case 1:
                        _a.sent();
                        getUsers();
                        confirm('Пользователь добавлен');
                        return [2 /*return*/];
                }
            });
        });
    }
    function createOrganization() {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, organizationstore.createOrganization(organizationLogin, organizationPassword)];
                    case 1:
                        _a.sent();
                        confirm('Организация добавлена');
                        return [2 /*return*/];
                }
            });
        });
    }
    function addUserForOrganization(userId) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, UserService_1.default.addUserForOrganization(organizationId, userId)];
                    case 1:
                        _a.sent();
                        return [4 /*yield*/, getUsers()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    }
    function delUserForOrganization(userId) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, UserService_1.default.delUserForOrganization(organizationId, userId)];
                    case 1:
                        _a.sent();
                        return [4 /*yield*/, getUsers()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    }
    function getUsers() {
        var _a;
        return __awaiter(this, void 0, void 0, function () {
            var response;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0: return [4 /*yield*/, UserService_1.default.getUsers()];
                    case 1:
                        response = _b.sent();
                        console.log('users: ', response.data);
                        setUsers(response.data);
                        console.log('organizationstore.organizations.length = ', organizationstore.organizations.length);
                        if (!(organizationstore.organizations.length === 0)) return [3 /*break*/, 3];
                        return [4 /*yield*/, organizationstore.requestOrganizations()];
                    case 2:
                        _b.sent();
                        _b.label = 3;
                    case 3:
                        setOrganizationId((_a = organizationstore.organizations[0]) === null || _a === void 0 ? void 0 : _a.id);
                        return [2 /*return*/];
                }
            });
        });
    }
    react_2.useEffect(function () {
        getUsers();
    }, []);
    var formNewUser = function () { return (React.createElement("li", null,
        React.createElement("dt", null,
            React.createElement("div", { className: "email__pass" },
                React.createElement("label", { htmlFor: "user" }, "Email \u043D\u043E\u0432\u043E\u0433\u043E \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044F"),
                React.createElement("input", { id: 'user', onChange: function (e) { return setUser(e.target.value); }, value: user, type: 'text', placeholder: 'Email', className: "email" }),
                React.createElement("label", { htmlFor: "password" }, "\u041F\u0430\u0440\u043E\u043B\u044C \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044F"),
                React.createElement("input", { id: 'password', onChange: function (e) { return setPassword(e.target.value); }, value: password, type: !hidden ? 'password' : 'text', placeholder: 'Password' }),
                React.createElement("i", { className: classes_i.join(' '), onClick: function () { return setHidden(!hidden); } }, "remove_red_eye")),
            React.createElement("button", { className: "btn-primary button", onClick: createUser }, "\u0421\u043E\u0437\u0434\u0430\u0442\u044C")),
        React.createElement("dd", null,
            React.createElement("ul", null,
                React.createElement("li", null,
                    React.createElement("label", { htmlFor: "rule_basic", className: "label-checkbox" },
                        React.createElement("input", { id: 'rule_basic', type: 'checkbox', checked: ruleBasic, onChange: function () { return setRuleBasic(!ruleBasic); } }),
                        "\u0412\u044B\u0433\u0440\u0443\u0437\u043A\u0430 \u043A\u043B\u0438\u0435\u043D\u0442\u043E\u0432 \u0432 iikoBiz",
                        React.createElement("i", { className: "material-icons red-text" }, ruleBasic ? 'check_box' : 'check_box_outline_blank'))),
                React.createElement("li", null,
                    React.createElement("label", { htmlFor: "rule_report", className: "label-checkbox" },
                        React.createElement("input", { id: 'rule_report', type: 'checkbox', checked: ruleReport, onChange: function () { return setRuleReport(!ruleReport); } }),
                        "\u041F\u043E\u0441\u0442\u0440\u043E\u0435\u043D\u0438\u0435 \u043E\u0442\u0447\u0435\u0442\u043E\u0432 \u0438\u0437 iikoBiz",
                        React.createElement("i", { className: "material-icons red-text" }, ruleReport ? 'check_box' : 'check_box_outline_blank'))))))); };
    var formNewOrganization = function () { return (React.createElement("li", null,
        React.createElement("dt", null,
            React.createElement("div", { className: "email__pass" },
                React.createElement("label", { htmlFor: "organization_login" }, "Api \u043B\u043E\u0433\u0438\u043D \u043D\u043E\u0432\u043E\u0439 \u043E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438"),
                React.createElement("input", { id: 'organization_login', onChange: function (e) { return setOrganizationLogin(e.target.value); }, value: organizationLogin, type: 'text', placeholder: 'Login API' }),
                React.createElement("label", { htmlFor: "organization_password" }, "Api \u043F\u0430\u0440\u043E\u043B\u044C \u043E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438"),
                React.createElement("input", { id: 'organization_password', onChange: function (e) { return setOrganizationPassword(e.target.value); }, value: organizationPassword, type: !hiddenOrg ? 'password' : 'text', placeholder: 'Password' }),
                React.createElement("i", { className: classes_i_O.join(' '), onClick: function () { return setHiddenOrg(!hiddenOrg); } }, "remove_red_eye")),
            React.createElement("button", { className: "btn-primary button", onClick: createOrganization }, "\u0421\u043E\u0437\u0434\u0430\u0442\u044C")),
        React.createElement("dd", null))); };
    var formUsers = function () { return (React.createElement("li", null,
        React.createElement("dt", null,
            React.createElement("label", { htmlFor: "user_mail" },
                "\u0424\u0438\u043B\u044C\u0442\u0440 \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u0435\u0439",
                React.createElement("input", { id: 'user_mail', onChange: function (e) { return setFilterUserEmail(e.target.value); }, value: filterUserEmail, type: 'text', placeholder: 'email' })),
            React.createElement("span", null,
                React.createElement(CustomSelect, { id: "user_organization", value: organizationId, options: organizationstore.organizations, onChange: function (event) { return setOrganizationId(event.target.value); } }))),
        React.createElement("dd", null),
        React.createElement("ul", null, users && users.filter(function (u) { return u.mail.includes(filterUserEmail); }).map(function (u) {
            return (React.createElement("li", { key: u.id },
                React.createElement("dt", null,
                    React.createElement("label", { className: "label-checkbox" }, u.mail),
                    React.createElement("span", null,
                        React.createElement("button", { className: "btn-primary button", onClick: function () { return delUserForOrganization(u.id); } }, "\u0423\u0431\u0440\u0430\u0442\u044C"),
                        React.createElement("button", { className: "btn-primary button", onClick: function () { return addUserForOrganization(u.id); } }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C"))),
                React.createElement("dd", null,
                    React.createElement("ul", null, u && u.organizations.map(function (o) {
                        return (React.createElement("li", { key: o.id },
                            React.createElement("label", { className: "" }, o.name)));
                    })))));
        })))); };
    var CustomSelect = function (_a) {
        var id = _a.id, value = _a.value, options = _a.options, onChange = _a.onChange;
        return (React.createElement("select", { className: "custom-select", id: id, value: value, onChange: onChange }, options.map(function (option) {
            return React.createElement("option", { key: option.id, value: option.id }, option.name);
        })));
    };
    var classes_i = ['password material-icons'];
    if (hidden) {
        classes_i.push('red-text');
    }
    var classes_i_O = ['password material-icons'];
    if (hiddenOrg) {
        classes_i_O.push('red-text');
    }
    else {
        classes_i.push('black-text');
    }
    return (React.createElement("div", null,
        React.createElement("h1", null, "\u0421\u0435\u0440\u0432\u0438\u0441"),
        React.createElement("div", { className: "form-group col-md-10" },
            React.createElement("ul", { className: "service" },
                formNewUser(),
                formNewOrganization(),
                formUsers()))));
};
exports.default = mobx_react_lite_1.observer(ServiceForm);
//# sourceMappingURL=ServiceForm.js.map