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
var mobx_react_lite_1 = require("mobx-react-lite");
var __1 = require("..");
var axios_1 = require("axios");
var BizServise_1 = require("../services/BizServise");
var react_router_dom_1 = require("react-router-dom");
var UploadForm = function () {
    var navigate = react_router_dom_1.useNavigate();
    var _a = react_1.useContext(__1.Context), organizationstore = _a.organizationstore, taskstore = _a.taskstore;
    var _b = react_1.useState(''), organizationId = _b[0], setOrganizationId = _b[1];
    var _c = react_1.useState(''), categoryId = _c[0], setCategoryId = _c[1];
    var _d = react_1.useState(''), corporateNutritionId = _d[0], setCorporateNutritionId = _d[1];
    var _e = react_1.useState(''), file = _e[0], setFile = _e[1];
    var _f = react_1.useState(''), fileName = _f[0], setFileName = _f[1];
    var _g = react_1.useState(false), refreshBalance = _g[0], setRefreshBalance = _g[1];
    var _h = react_1.useState(false), rename = _h[0], setRename = _h[1];
    var _j = react_1.useState([]), categories = _j[0], setCategories = _j[1];
    var _k = react_1.useState([]), corporateNutritions = _k[0], setCorporateNutritions = _k[1];
    var _l = react_1.useState(0), balance = _l[0], setBalance = _l[1];
    var _m = react_1.useState(''), customerName = _m[0], setCustomerName = _m[1];
    var _o = react_1.useState(''), customerCard = _o[0], setCustomerCard = _o[1];
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
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, organizationstore.requestOrganizations()];
                    case 1:
                        _a.sent();
                        setSetters();
                        organizationstore.setLoading(false);
                        return [2 /*return*/];
                }
            });
        });
    }
    function setSetters() {
        var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k, _l, _m;
        setOrganizationId((_b = (_a = organizationstore.organizations[0]) === null || _a === void 0 ? void 0 : _a.id) !== null && _b !== void 0 ? _b : '');
        setCategories((_d = (_c = organizationstore.organizations[0]) === null || _c === void 0 ? void 0 : _c.categories) !== null && _d !== void 0 ? _d : []);
        setCategoryId((_g = (_f = (_e = organizationstore.organizations[0]) === null || _e === void 0 ? void 0 : _e.categories[0]) === null || _f === void 0 ? void 0 : _f.id) !== null && _g !== void 0 ? _g : '');
        setCorporateNutritions((_j = (_h = organizationstore.organizations[0]) === null || _h === void 0 ? void 0 : _h.corporateNutritions) !== null && _j !== void 0 ? _j : []);
        setCorporateNutritionId((_m = (_l = (_k = organizationstore.organizations[0]) === null || _k === void 0 ? void 0 : _k.corporateNutritions[0]) === null || _l === void 0 ? void 0 : _l.id) !== null && _m !== void 0 ? _m : '');
    }
    function onChangeFile(e) {
        return __awaiter(this, void 0, void 0, function () {
            var formdata, config, response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        formdata = new FormData();
                        formdata.append('file', e.target.files[0]);
                        config = {
                            headers: {
                                'content-type': 'multipart/form-data',
                                'Authorization': "Bearer " + localStorage.getItem('token')
                            }
                        };
                        return [4 /*yield*/, axios_1.default.post('/api/file/create', formdata, config)];
                    case 1:
                        response = _a.sent();
                        setFile(response.data.url);
                        setFileName(response.data.name);
                        return [2 /*return*/];
                }
            });
        });
    }
    function uploadToBiz() {
        return __awaiter(this, void 0, void 0, function () {
            var option, response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (file === '')
                            confirm('Не выбран вайл выгрузки');
                        option = {
                            organizationId: organizationId,
                            corporateNutritionId: corporateNutritionId,
                            categoryId: categoryId,
                            balance: balance,
                            fileReport: file,
                            options: {
                                refreshBalance: refreshBalance,
                                rename: rename
                            },
                            customer: null
                        };
                        return [4 /*yield*/, BizServise_1.default.upladCustomersToBiz(option)];
                    case 1:
                        response = _a.sent();
                        taskstore.onAddTask(response.data, 'Выгрузка: ' + fileName);
                        navigate("/task");
                        return [2 /*return*/];
                }
            });
        });
    }
    function singleUploadToBiz() {
        return __awaiter(this, void 0, void 0, function () {
            var option, response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        option = {
                            organizationId: organizationId,
                            corporateNutritionId: corporateNutritionId,
                            categoryId: categoryId,
                            balance: balance,
                            fileReport: file,
                            options: {
                                refreshBalance: refreshBalance,
                                rename: rename
                            },
                            customer: {
                                name: customerName,
                                card: customerCard
                            }
                        };
                        return [4 /*yield*/, BizServise_1.default.upladCustomersToBiz(option)];
                    case 1:
                        response = _a.sent();
                        taskstore.onAddTask(response.data, 'Выгрузка: ' + fileName);
                        navigate("/task");
                        return [2 /*return*/];
                }
            });
        });
    }
    function updateOrganization() {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, organizationstore.updateOrganization(organizationId)];
                    case 1:
                        _a.sent();
                        setSetters();
                        return [2 /*return*/];
                }
            });
        });
    }
    react_1.useEffect(function () {
        firstInit();
        //console.log('UploadForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return React.createElement("h1", null, "Loading...");
    }
    return (React.createElement("div", null,
        React.createElement("h1", { className: "form-group col-md-7" }, "\u0412\u044B\u0433\u0440\u0443\u0437\u043A\u0430 \u0432 iikoBiz"),
        React.createElement("div", { className: "form-group col-md-7" },
            React.createElement("h5", { className: "link", onClick: updateOrganization }, "\u041E\u0431\u043D\u043E\u0432\u0438\u0442\u044C \u0434\u0430\u043D\u043D\u044B\u0435 \u043F\u043E \u043E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438"),
            React.createElement("label", { htmlFor: "organizations" }, "\u041E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438"),
            React.createElement(CustomSelect, { id: "organizations", value: organizationId, options: organizationstore.organizations, onChange: onOrganizationSelectChange }),
            React.createElement("label", { htmlFor: "categories" }, "\u041A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u0438"),
            React.createElement(CustomSelect, { id: "categories", value: categoryId, options: categories, onChange: function (event) { return setCategoryId(event.target.value); } }),
            React.createElement("label", { htmlFor: "corporateNutritions" }, "\u041F\u0440\u043E\u0433\u0440\u0430\u043C\u043C\u044B \u043F\u0438\u0442\u0430\u043D\u0438\u044F"),
            React.createElement(CustomSelect, { id: "corporateNutritions", value: corporateNutritionId, options: corporateNutritions, onChange: function (event) { return setCorporateNutritionId(event.target.value); } }),
            React.createElement("label", { htmlFor: "balance" },
                "\u0411\u0430\u043B\u0430\u043D\u0441",
                React.createElement("input", { id: 'balance', onChange: function (e) { return setBalance(parseInt(e.target.value)); }, value: balance, type: 'number', placeholder: '\u0411\u0430\u043B\u0430\u043D\u0441' }))),
        React.createElement("div", { className: "form-group col-md-7" },
            React.createElement("label", { htmlFor: 'refreshBalance', className: "label-checkbox" },
                React.createElement("input", { id: 'refreshBalance', type: 'checkbox', checked: refreshBalance, onChange: function () { return setRefreshBalance(!refreshBalance); } }),
                "\u041E\u0431\u043D\u043E\u0432\u043B\u044F\u0442\u044C \u0431\u0430\u043B\u0430\u043D\u0441",
                React.createElement("i", { className: "material-icons red-text" }, refreshBalance ? 'check_box' : 'check_box_outline_blank')),
            React.createElement("label", { htmlFor: "rename", className: "label-checkbox" },
                React.createElement("input", { id: 'rename', type: 'checkbox', checked: rename, onChange: function () { return setRename(!rename); } }),
                "\u041F\u0435\u0440\u0435\u0438\u043C\u0435\u043D\u043E\u0432\u0430\u0442\u044C \u0432 \u0441\u043E\u043E\u0442\u0432\u0435\u0442\u0441\u0442\u0432\u0438\u0438 \u0441 \u043D\u043E\u0432\u044B\u043C \u0441\u043F\u0438\u0441\u043A\u043E\u043C",
                React.createElement("i", { className: "material-icons red-text" }, rename ? 'check_box' : 'check_box_outline_blank')),
            React.createElement("label", { htmlFor: "customerName" },
                "\u0421\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A",
                React.createElement("input", { id: 'customerName', onChange: function (e) { return setCustomerName(e.target.value); }, value: customerName, type: 'text', placeholder: '\u0424\u0418\u041E \u0441\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A\u0430' })),
            React.createElement("br", null),
            React.createElement("label", { htmlFor: "customerCard" },
                "\u0421\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A",
                React.createElement("input", { id: 'customerName', onChange: function (e) { return setCustomerCard(e.target.value); }, value: customerCard, type: 'text', placeholder: '\u041A\u0430\u0440\u0442\u0430 \u0441\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A\u0430' })),
            React.createElement("button", { className: "button", onClick: singleUploadToBiz }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C \u043E\u0434\u043D\u043E\u0433\u043E"),
            React.createElement("br", null),
            React.createElement("label", { htmlFor: "file" }, "\u0412\u044B\u0431\u0438\u0440\u0438\u0442\u0435 \u0444\u0430\u0439\u043B"),
            React.createElement("br", null),
            React.createElement("input", { className: "form-group button", id: 'file', type: 'file', onChange: onChangeFile }),
            React.createElement("button", { className: "button", onClick: uploadToBiz }, "\u0412\u044B\u0433\u0440\u0443\u0437\u0438\u0442\u044C"))));
};
exports.default = mobx_react_lite_1.observer(UploadForm);
//# sourceMappingURL=UploadForm.js.map