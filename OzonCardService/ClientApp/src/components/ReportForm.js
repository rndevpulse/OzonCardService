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
var react_router_dom_1 = require("react-router-dom");
var __1 = require("..");
var mobx_react_lite_1 = require("mobx-react-lite");
var react_datepicker_1 = require("react-datepicker");
require("react-datepicker/dist/react-datepicker.css");
require("bootstrap/dist/css/bootstrap.min.css");
var ru_1 = require("date-fns/locale/ru");
react_datepicker_1.registerLocale("ru", ru_1.default);
var moment = require("moment");
var BizServise_1 = require("../services/BizServise");
var react_tabs_1 = require("react-tabs");
require("../css/ReportForm.css");
var ReportForm = function () {
    var navigate = react_router_dom_1.useNavigate();
    var _a = react_1.useContext(__1.Context), organizationstore = _a.organizationstore, taskstore = _a.taskstore;
    var _b = react_1.useState(''), organizationId = _b[0], setOrganizationId = _b[1];
    var _c = react_1.useState([]), corporateNutritions = _c[0], setCorporateNutritions = _c[1];
    var _d = react_1.useState(''), corporateNutritionId = _d[0], setCorporateNutritionId = _d[1];
    var _e = react_1.useState(''), categoryId = _e[0], setCategoryId = _e[1];
    var _f = react_1.useState([]), categories = _f[0], setCategories = _f[1];
    var _g = react_1.useState(''), fileName = _g[0], setFileName = _g[1];
    var _h = react_1.useState(new Date(new Date().setDate(1))), dateFrom = _h[0], setDateFrom = _h[1];
    var _j = react_1.useState(new Date()), dateTo = _j[0], setDateTo = _j[1];
    var _k = react_1.useState(true), isFilter = _k[0], setIsFilter = _k[1];
    var CustomSelect = function (_a) {
        var id = _a.id, value = _a.value, options = _a.options, onChange = _a.onChange;
        return (React.createElement("select", { className: "custom-select", id: id, value: value, onChange: onChange }, options.map(function (option) {
            return React.createElement("option", { key: option.id, value: option.id }, option.name);
        })));
    };
    var onOrganizationSelectChange = function (e) {
        var _a, _b, _c, _d, _e, _f, _g;
        var orgId = e.target.options[e.target.selectedIndex].value;
        var organization = organizationstore.organizations.find(function (org) { return org.id === orgId; });
        setOrganizationId((_a = organization === null || organization === void 0 ? void 0 : organization.id) !== null && _a !== void 0 ? _a : '');
        setCategories((_b = organization === null || organization === void 0 ? void 0 : organization.categories) !== null && _b !== void 0 ? _b : []);
        setCategoryId((_d = (_c = organization === null || organization === void 0 ? void 0 : organization.categories[0]) === null || _c === void 0 ? void 0 : _c.id) !== null && _d !== void 0 ? _d : '');
        setCorporateNutritions((_e = organization === null || organization === void 0 ? void 0 : organization.corporateNutritions) !== null && _e !== void 0 ? _e : []);
        setCorporateNutritionId((_g = (_f = organization === null || organization === void 0 ? void 0 : organization.corporateNutritions[0]) === null || _f === void 0 ? void 0 : _f.id) !== null && _g !== void 0 ? _g : '');
    };
    function firstInit() {
        var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k, _l, _m;
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_o) {
                switch (_o.label) {
                    case 0: return [4 /*yield*/, organizationstore.requestOrganizations()];
                    case 1:
                        _o.sent();
                        setOrganizationId((_b = (_a = organizationstore.organizations[0]) === null || _a === void 0 ? void 0 : _a.id) !== null && _b !== void 0 ? _b : '');
                        setCorporateNutritions((_d = (_c = organizationstore.organizations[0]) === null || _c === void 0 ? void 0 : _c.corporateNutritions) !== null && _d !== void 0 ? _d : []);
                        setCorporateNutritionId((_g = (_f = (_e = organizationstore.organizations[0]) === null || _e === void 0 ? void 0 : _e.corporateNutritions[0]) === null || _f === void 0 ? void 0 : _f.id) !== null && _g !== void 0 ? _g : '');
                        setCategories((_j = (_h = organizationstore.organizations[0]) === null || _h === void 0 ? void 0 : _h.categories) !== null && _j !== void 0 ? _j : []);
                        setCategoryId((_m = (_l = (_k = organizationstore.organizations[0]) === null || _k === void 0 ? void 0 : _k.categories[0]) === null || _l === void 0 ? void 0 : _l.id) !== null && _m !== void 0 ? _m : '');
                        organizationstore.setLoading(false);
                        return [2 /*return*/];
                }
            });
        });
    }
    function reportFromBiz() {
        return __awaiter(this, void 0, void 0, function () {
            var option, response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        option = {
                            organizationId: organizationId,
                            categoryId: isFilter ? categoryId : "00000000-0000-0000-0000-000000000000",
                            corporateNutritionId: corporateNutritionId,
                            dateFrom: (moment(dateFrom)).format("YYYY-MM-DD"),
                            dateTo: (moment(dateTo)).add(1, 'days').format("YYYY-MM-DD"),
                            title: fileName === ''
                                ? "\u041E\u0442\u0447\u0435\u0442 \u043E\u0442 " + (moment(new Date())).format("DD.MM.YYYY HH.mm")
                                : fileName
                        };
                        return [4 /*yield*/, BizServise_1.default.ReportFromBiz(option)];
                    case 1:
                        response = _a.sent();
                        taskstore.onAddTask(response.data, 'Отчет: ' + option.title);
                        navigate("/task");
                        return [2 /*return*/];
                }
            });
        });
    }
    function transactionsFromBiz() {
        return __awaiter(this, void 0, void 0, function () {
            var option, response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        option = {
                            organizationId: organizationId,
                            categoryId: isFilter ? categoryId : "00000000-0000-0000-0000-000000000000",
                            corporateNutritionId: corporateNutritionId,
                            dateFrom: (moment(dateFrom)).format("YYYY-MM-DD"),
                            dateTo: (moment(dateTo)).add(1, 'days').format("YYYY-MM-DD"),
                            title: fileName === ''
                                ? "\u041E\u0442\u0447\u0435\u0442 \u043E\u0442 " + (moment(new Date())).format("DD.MM.YYYY HH.mm")
                                : fileName
                        };
                        return [4 /*yield*/, BizServise_1.default.TransactionsFromBiz(option)];
                    case 1:
                        response = _a.sent();
                        taskstore.onAddTask(response.data, 'Отчет: ' + option.title);
                        navigate("/task");
                        return [2 /*return*/];
                }
            });
        });
    }
    function div_datePickers() {
        return (React.createElement("div", { className: "div-datePicker" },
            React.createElement("label", { htmlFor: "dateFrom" }, "\u041F\u0435\u0440\u0438\u043E\u0434 \u0441 "),
            React.createElement(react_datepicker_1.default, { dateFormat: 'dd MMMM yyyy', selected: dateFrom, selectsStart: true, startDate: dateFrom, endDate: dateTo, onChange: function (date) { return setDateFrom(date); }, id: "dateFrom", locale: 'ru', placeholderText: "\u041F\u0435\u0440\u0438\u043E\u0434 \u0441" }),
            React.createElement("label", { htmlFor: "dateTo" }, " \u043F\u043E "),
            React.createElement(react_datepicker_1.default, { dateFormat: 'dd MMMM yyyy', selected: dateTo, selectsEnd: true, startDate: dateFrom, endDate: dateTo, minDate: dateFrom, onChange: function (date) { return setDateTo(date); }, name: "dateTo", locale: 'ru', placeholderText: "\u041F\u0435\u0440\u0438\u043E\u0434 \u043F\u043E" })));
    }
    function div_nameFileReport() {
        return (React.createElement("label", { htmlFor: "name" },
            "\u0418\u0437\u043C\u0435\u043D\u0438\u0442\u044C \u043D\u0430\u0438\u043C\u0435\u043D\u043E\u0432\u0430\u043D\u0438\u0435 \u043E\u0442\u0447\u0435\u0442\u0430 (\u0444\u0430\u0439\u043B\u0430) \u0434\u043B\u044F \u0441\u043E\u0445\u0440\u0430\u043D\u0435\u043D\u0438\u044F",
            React.createElement("input", { id: 'name', type: 'text', value: fileName, onChange: function (event) { return setFileName(event.target.value); }, placeholder: "\u041E\u0442\u0447\u0435\u0442 \u043E\u0442 " + (moment(new Date())).format("DD.MM.YYYY HH.mm") })));
    }
    react_1.useEffect(function () {
        firstInit();
    }, []);
    if (organizationstore.isLoading) {
        return React.createElement("h1", null, "Loading...");
    }
    return (React.createElement("div", null,
        React.createElement("h1", { className: "center form-group col-md-12" }, "\u041E\u0442\u0447\u0435\u0442\u044B"),
        React.createElement("div", { className: "center form-group col-md-12" },
            React.createElement(react_tabs_1.Tabs, { className: "Tabs" },
                React.createElement(react_tabs_1.TabList, null,
                    React.createElement(react_tabs_1.Tab, null, "\u041E\u0442\u0447\u0435\u0442 \u0437\u0430 \u043F\u0435\u0440\u0438\u043E\u0434"),
                    React.createElement(react_tabs_1.Tab, null, "\u041E\u0442\u0447\u0435\u0442 \u043F\u043E \u043E\u043F\u0435\u0440\u0430\u0446\u0438\u044F\u043C")),
                React.createElement(react_tabs_1.TabPanel, null,
                    React.createElement("label", { htmlFor: "organizations" }, "\u041E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438"),
                    React.createElement(CustomSelect, { id: "organizations", value: organizationId, options: organizationstore.organizations, onChange: onOrganizationSelectChange }),
                    React.createElement("label", { htmlFor: "allCategories", className: "label-checkbox-category" },
                        React.createElement("input", { id: 'allCategories', type: 'checkbox', checked: isFilter, onChange: function () { return setIsFilter(!isFilter); } }),
                        "\u0423\u0447\u0438\u0442\u044B\u0432\u0430\u0442\u044C \u0444\u0438\u043B\u044C\u0442\u0440 \u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u0439",
                        React.createElement("i", { className: "check_box material-icons red-text" }, isFilter ? 'check_box' : 'check_box_outline_blank')),
                    React.createElement("label", { htmlFor: "categories" }, "\u0424\u0438\u043B\u044C\u0442\u0440 \u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u0439"),
                    React.createElement(CustomSelect, { id: "categories", value: categoryId, options: categories, onChange: function (event) { return setCategoryId(event.target.value); } }),
                    React.createElement("label", { htmlFor: "corporateNutritions" }, "\u041F\u0440\u043E\u0433\u0440\u0430\u043C\u043C\u044B \u043F\u0438\u0442\u0430\u043D\u0438\u044F"),
                    React.createElement(CustomSelect, { id: "corporateNutritions", value: corporateNutritionId, options: corporateNutritions, onChange: function (event) { return setCorporateNutritionId(event.target.value); } }),
                    div_datePickers(),
                    div_nameFileReport(),
                    React.createElement("button", { className: "button", onClick: reportFromBiz }, "\u0412\u044B\u0433\u0440\u0443\u0437\u0438\u0442\u044C")),
                React.createElement(react_tabs_1.TabPanel, null,
                    React.createElement("label", { htmlFor: "organizations" }, "\u041E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438"),
                    React.createElement(CustomSelect, { id: "organizations", value: organizationId, options: organizationstore.organizations, onChange: onOrganizationSelectChange }),
                    React.createElement("label", { htmlFor: "corporateNutritions" }, "\u041F\u0440\u043E\u0433\u0440\u0430\u043C\u043C\u044B \u043F\u0438\u0442\u0430\u043D\u0438\u044F"),
                    React.createElement(CustomSelect, { id: "corporateNutritions", value: corporateNutritionId, options: corporateNutritions, onChange: function (event) { return setCorporateNutritionId(event.target.value); } }),
                    div_datePickers(),
                    div_nameFileReport(),
                    React.createElement("button", { className: "button", onClick: transactionsFromBiz }, "\u0412\u044B\u0433\u0440\u0443\u0437\u0438\u0442\u044C"))))));
};
exports.default = mobx_react_lite_1.observer(ReportForm);
//# sourceMappingURL=ReportForm.js.map