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
Object.defineProperty(exports, "__esModule", { value: true });
const jsx_runtime_1 = require("react/jsx-runtime");
const react_1 = require("react");
const mobx_react_lite_1 = require("mobx-react-lite");
const react_2 = require("react");
const __1 = require("..");
const UploadForm = () => {
    const { organizationstore } = react_1.useContext(__1.Context);
    const [organizationId, setOrganizationId] = react_1.useState('');
    const [categories, setCategories] = react_1.useState([]);
    const [categoryId, setCategoryId] = react_1.useState('');
    const [corporateNutritions, setCorporateNutritions] = react_1.useState([]);
    const [corporateNutritionId, setCorporateNutritionId] = react_1.useState('');
    const [balance, setBalance] = react_1.useState(0);
    const CustomSelect = ({ id, value, options, onChange }) => {
        return (jsx_runtime_1.jsx("select", Object.assign({ className: "custom-select", id: id, value: value, onChange: onChange }, { children: options.map(option => jsx_runtime_1.jsx("option", Object.assign({ value: option.id }, { children: option.name }), option.id)) }), void 0));
    };
    const onOrganizationSelectChange = (e) => {
        var _a, _b, _c, _d, _e, _f, _g;
        const orgId = e.target.options[e.target.selectedIndex].value;
        const organization = organizationstore.organizations.find(org => org.id === orgId);
        setOrganizationId((_a = organization === null || organization === void 0 ? void 0 : organization.id) !== null && _a !== void 0 ? _a : '');
        setCategories((_b = organization === null || organization === void 0 ? void 0 : organization.categories) !== null && _b !== void 0 ? _b : []);
        setCategoryId((_d = (_c = organization === null || organization === void 0 ? void 0 : organization.categories[0]) === null || _c === void 0 ? void 0 : _c.id) !== null && _d !== void 0 ? _d : '');
        setCorporateNutritions((_e = organization === null || organization === void 0 ? void 0 : organization.corporateNutritions) !== null && _e !== void 0 ? _e : []);
        setCorporateNutritionId((_g = (_f = organization === null || organization === void 0 ? void 0 : organization.corporateNutritions[0]) === null || _f === void 0 ? void 0 : _f.id) !== null && _g !== void 0 ? _g : '');
    };
    function firstInit() {
        var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k, _l, _m;
        return __awaiter(this, void 0, void 0, function* () {
            yield organizationstore.requestOrganizations();
            setOrganizationId((_b = (_a = organizationstore.organizations[0]) === null || _a === void 0 ? void 0 : _a.id) !== null && _b !== void 0 ? _b : []);
            setCategories((_d = (_c = organizationstore.organizations[0]) === null || _c === void 0 ? void 0 : _c.categories) !== null && _d !== void 0 ? _d : []);
            setCategoryId((_g = (_f = (_e = organizationstore.organizations[0]) === null || _e === void 0 ? void 0 : _e.categories[0]) === null || _f === void 0 ? void 0 : _f.id) !== null && _g !== void 0 ? _g : '');
            setCorporateNutritions((_j = (_h = organizationstore.organizations[0]) === null || _h === void 0 ? void 0 : _h.corporateNutritions) !== null && _j !== void 0 ? _j : []);
            setCorporateNutritionId((_m = (_l = (_k = organizationstore.organizations[0]) === null || _k === void 0 ? void 0 : _k.corporateNutritions[0]) === null || _l === void 0 ? void 0 : _l.id) !== null && _m !== void 0 ? _m : '');
            organizationstore.setLoading(false);
            console.log('isLoading false');
        });
    }
    react_2.useEffect(() => {
        firstInit();
        console.log('UploadForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return jsx_runtime_1.jsx("h1", { children: "Loading..." }, void 0);
    }
    console.log('UploadForm return');
    return (jsx_runtime_1.jsxs("div", { children: [jsx_runtime_1.jsx("h1", { children: "UPLOAD FORM" }, void 0), jsx_runtime_1.jsxs("p", { children: ["\u043E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u044F: ", organizationId] }, void 0), jsx_runtime_1.jsxs("p", { children: ["\u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044F: ", categoryId] }, void 0), jsx_runtime_1.jsxs("p", { children: ["\u043A\u043E\u0440\u043F\u0438\u0442: ", corporateNutritionId] }, void 0), jsx_runtime_1.jsxs("div", Object.assign({ className: "form-group col-md-6" }, { children: [jsx_runtime_1.jsx("label", Object.assign({ htmlFor: "organizations" }, { children: "\u041E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438" }), void 0), jsx_runtime_1.jsx(CustomSelect, { id: "organizations", value: organizationId, options: organizationstore.organizations, onChange: onOrganizationSelectChange }, void 0)] }), void 0), jsx_runtime_1.jsxs("div", Object.assign({ className: "form-group col-md-6" }, { children: [jsx_runtime_1.jsx("label", Object.assign({ htmlFor: "categories" }, { children: "\u041A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u0438" }), void 0), jsx_runtime_1.jsx(CustomSelect, { id: "categories", value: categoryId, options: categories, onChange: event => setCategoryId(event.target.value) }, void 0)] }), void 0), jsx_runtime_1.jsxs("div", Object.assign({ className: "form-group col-md-6" }, { children: [jsx_runtime_1.jsx("label", Object.assign({ htmlFor: "corporateNutritions" }, { children: "\u041F\u0440\u043E\u0433\u0440\u0430\u043C\u043C\u044B \u043F\u0438\u0442\u0430\u043D\u0438\u044F" }), void 0), jsx_runtime_1.jsx(CustomSelect, { id: "corporateNutritions", value: corporateNutritionId, options: corporateNutritions, onChange: event => setCorporateNutritionId(event.target.value) }, void 0)] }), void 0), jsx_runtime_1.jsxs("div", Object.assign({ className: "form-group col-md-6" }, { children: [jsx_runtime_1.jsx("label", Object.assign({ htmlFor: "balance" }, { children: "\u0411\u0430\u043B\u0430\u043D\u0441" }), void 0), jsx_runtime_1.jsx("input", { id: 'balance', onChange: e => setBalance(parseInt(e.target.value)), value: balance, type: 'number', placeholder: '\u0411\u0430\u043B\u0430\u043D\u0441' }, void 0)] }), void 0)] }, void 0));
};
exports.default = mobx_react_lite_1.observer(UploadForm);
