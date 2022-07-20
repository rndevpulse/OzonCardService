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
var react_2 = require("react");
var __1 = require("..");
var BizServise_1 = require("../services/BizServise");
require("../css/SearchForm.css");
var SearchCustomerForm = function () {
    var organizationstore = react_1.useContext(__1.Context).organizationstore;
    var _a = react_1.useState(''), organizationId = _a[0], setOrganizationId = _a[1];
    var _b = react_1.useState([]), corporateNutritions = _b[0], setCorporateNutritions = _b[1];
    var _c = react_1.useState(''), corporateNutritionId = _c[0], setCorporateNutritionId = _c[1];
    var _d = react_1.useState(''), categoryId = _d[0], setCategoryId = _d[1];
    var _e = react_1.useState([]), categories = _e[0], setCategories = _e[1];
    var _f = react_1.useState(''), customerName = _f[0], setCustomerName = _f[1];
    var _g = react_1.useState(''), customerCard = _g[0], setCustomerCard = _g[1];
    var _h = react_1.useState([]), customersInfo = _h[0], setCustomersInfo = _h[1];
    var _j = react_1.useState(false), isLoadCustomers = _j[0], setIsLoadCustomers = _j[1];
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
    function getCustomers(name, card) {
        return __awaiter(this, void 0, void 0, function () {
            var response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        setIsLoadCustomers(true);
                        return [4 /*yield*/, BizServise_1.default.SearchCustomerFromBiz({
                                name: name,
                                card: card,
                                organizationId: organizationId,
                                corporateNutritionId: corporateNutritionId
                            })
                            //console.log('customers: ', response.data)
                        ];
                    case 1:
                        response = _a.sent();
                        //console.log('customers: ', response.data)
                        setCustomersInfo(response.data);
                        setIsLoadCustomers(false);
                        return [2 /*return*/];
                }
            });
        });
    }
    function onChangeCustomerName(value) {
        setCustomerName(value);
        if (value.length > 4) {
            getCustomers(value, customerCard);
        }
        else if (value.length == 0 && customerCard.length == 0) {
            setCustomersInfo([]);
        }
    }
    function onChangeCustomerCard(value) {
        setCustomerCard(value);
        if (value.length > 5) {
            getCustomers(customerName, value);
        }
        else if (value.length == 0 && customerName.length == 0) {
            setCustomersInfo([]);
        }
    }
    function ChangeCustomerCategory(id, name, isRemove) {
        var _a, _b, _c, _d;
        return __awaiter(this, void 0, void 0, function () {
            var catName, customer_categories, new_arr, new_arr;
            return __generator(this, function (_e) {
                switch (_e.label) {
                    case 0:
                        catName = categories.filter(function (x) { return x.id === categoryId; })[0].name;
                        if (!isRemove && ((_a = customersInfo.find(function (x) { return x.id === id; })) === null || _a === void 0 ? void 0 : _a.categories.includes(catName))
                            || isRemove && !((_b = customersInfo.find(function (x) { return x.id === id; })) === null || _b === void 0 ? void 0 : _b.categories.includes(catName))) {
                            return [2 /*return*/];
                        }
                        return [4 /*yield*/, BizServise_1.default.ChangeCustomerBizCategory({
                                id: id,
                                organizationId: organizationId,
                                categoryId: categoryId,
                                isRemove: isRemove
                            })];
                    case 1:
                        _e.sent();
                        setIsLoadCustomers(true);
                        if (isRemove) {
                            customer_categories = (_c = customersInfo.find(function (x) { return x.id === id; })) === null || _c === void 0 ? void 0 : _c.categories.filter(function (x) { return x !== catName; });
                            new_arr = customersInfo;
                            console.log(customer_categories);
                            if (customer_categories)
                                new_arr.find(function (x) { return x.id === id; }).categories = customer_categories;
                            console.log(new_arr);
                            setCustomersInfo(new_arr);
                            confirm("\u0423 \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044F \"" + name + "\" \u0443\u0434\u0430\u043B\u0435\u043D\u0430 \u0443\u043A\u0430\u0437\u0430\u043D\u043D\u0430\u044F \u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044F");
                        }
                        else {
                            new_arr = customersInfo;
                            (_d = new_arr.find(function (x) { return x.id === id; })) === null || _d === void 0 ? void 0 : _d.categories.push(catName);
                            console.log(new_arr);
                            setCustomersInfo(new_arr);
                            confirm("\u041F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044E \"" + name + "\" \u0434\u043E\u0431\u0430\u0432\u043B\u0435\u043D\u0430 \u0443\u043A\u0430\u0437\u0430\u043D\u043D\u0430\u044F \u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044F");
                        }
                        setIsLoadCustomers(false);
                        return [2 /*return*/];
                }
            });
        });
    }
    function getCustomersInfo() {
        //console.log("getCustomersInfo length", customersInfo.length)
        if (isLoadCustomers) {
            return React.createElement("div", { className: "center" }, "\u0418\u0434\u0435\u0442 \u043F\u043E\u0438\u0441\u043A...");
        }
        if (customersInfo.length === 0) {
            return React.createElement("div", { className: "center" }, "\u041D\u0435\u0442 \u0440\u0435\u0437\u0443\u043B\u044C\u0442\u0430\u0442\u043E\u0432");
        }
        return (React.createElement("div", { className: "center search form-group col-md-12" },
            React.createElement("ul", null, customersInfo && customersInfo.map(function (customer) {
                return (React.createElement("li", { key: customer.id },
                    React.createElement("dt", null, customer.name),
                    React.createElement("div", null,
                        React.createElement("label", null, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C \u0438\u043B\u0438 \u0443\u0434\u0430\u043B\u0438\u0442\u044C \u0432\u044B\u0431\u0440\u0430\u043D\u043D\u0443\u044E \u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044E"),
                        React.createElement("span", null,
                            React.createElement("button", { className: "button", onClick: function () { return ChangeCustomerCategory(customer.id, customer.name, true); } }, "\u0423\u0434\u0430\u043B\u0438\u0442\u044C"),
                            React.createElement("button", { className: "button", onClick: function () { return ChangeCustomerCategory(customer.id, customer.name, false); } }, "\u0414\u043E\u0431\u0430\u0432\u0438\u0442\u044C"))),
                    React.createElement("dd", null,
                        React.createElement("ul", null,
                            React.createElement("li", null,
                                "\u041E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u044F: ",
                                customer.organization),
                            React.createElement("li", null,
                                "\u041A\u0430\u0440\u0442\u0430: ",
                                customer.card),
                            React.createElement("li", null,
                                "\u0422\u0430\u0431\u0435\u043B\u044C\u043D\u044B\u0439 \u2116: ",
                                customer.tabNumber),
                            React.createElement("li", null,
                                "\u0411\u0430\u043B\u0430\u043D\u0441: ",
                                customer.balanse),
                            React.createElement("li", null,
                                "\u0421\u0443\u043C\u043C\u0430 \u0437\u0430\u043A\u0430\u0437\u043E\u0432: ",
                                customer.sum),
                            React.createElement("li", null,
                                "\u041A\u043E\u043B\u0438\u0447\u0435\u0441\u0442\u0432\u043E \u0437\u0430\u043A\u0430\u0437\u043E\u0432: ",
                                customer.orders)),
                        React.createElement("ul", null,
                            React.createElement("li", null, "\u041A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u0438:"),
                            customer.categories && customer.categories.map(function (category) {
                                return (React.createElement("li", null, category));
                            })))));
            }))));
    }
    react_2.useEffect(function () {
        firstInit();
        //console.log('SearchCustomerForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return React.createElement("h1", null, "Loading...");
    }
    return (React.createElement("div", null,
        React.createElement("h1", { className: "center form-group col-md-12" }, "\u041F\u043E\u0438\u0441\u043A \u0441\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A\u0430"),
        React.createElement("div", { className: "center form-group col-md-12" },
            React.createElement("label", { htmlFor: "organizations" }, "\u041E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438"),
            React.createElement(CustomSelect, { id: "organizations", value: organizationId, options: organizationstore.organizations, onChange: onOrganizationSelectChange }),
            React.createElement("label", { htmlFor: "corporateNutritions" }, "\u041F\u0440\u043E\u0433\u0440\u0430\u043C\u043C\u044B \u043F\u0438\u0442\u0430\u043D\u0438\u044F"),
            React.createElement(CustomSelect, { id: "corporateNutritions", value: corporateNutritionId, options: corporateNutritions, onChange: function (event) { return setCorporateNutritionId(event.target.value); } }),
            React.createElement("label", { className: "search_label", htmlFor: "customerName" },
                "\u0424\u0418\u041E \u0441\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A\u0430",
                React.createElement("input", { id: 'customerName', onChange: function (e) { return onChangeCustomerName(e.target.value); }, value: customerName, type: 'text', placeholder: '\u0424\u0430\u043C\u0438\u043B\u0438\u044F \u0418\u043C\u044F \u041E\u0442\u0447\u0435\u0441\u0442\u0432\u043E' })),
            React.createElement("label", { className: "search_label", htmlFor: "customerCard" },
                "\u041A\u0430\u0440\u0442\u0430 \u0441\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A\u0430",
                React.createElement("input", { id: 'customerCard', onChange: function (e) { return onChangeCustomerCard(e.target.value); }, value: customerCard, type: 'text', placeholder: 'xxxxxxxx' })),
            React.createElement("br", null),
            React.createElement("label", { htmlFor: "categories" }, "\u0423\u043A\u0430\u0436\u0438\u0442\u0435 \u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044E \u0434\u043B\u044F \u0434\u043E\u0431\u0430\u0432\u043B\u0435\u043D\u0438\u044F/\u0443\u0434\u0430\u043B\u0435\u043D\u0438\u044F \u0443 \u043F\u043E\u043B\u044C\u0437\u043E\u0432\u0430\u0442\u0435\u043B\u044F"),
            React.createElement(CustomSelect, { id: "categories", value: categoryId, options: categories, onChange: function (event) { return setCategoryId(event.target.value); } })),
        getCustomersInfo()));
};
exports.default = mobx_react_lite_1.observer(SearchCustomerForm);
//# sourceMappingURL=SearchCustomerForm.js.map