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
import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Context } from '..';
import axios from 'axios';
import BizService from '../services/BizServise';
const UploadForm = () => {
    const { organizationstore } = useContext(Context);
    const [taskId, setTaskId] = useState('');
    const [organizationId, setOrganizationId] = useState('');
    const [categoryId, setCategoryId] = useState('');
    const [corporateNutritionId, setCorporateNutritionId] = useState('');
    const [file, setFile] = useState('');
    const [refreshBalance, setRefreshBalance] = useState(false);
    const [rename, setRename] = useState(false);
    const [categories, setCategories] = useState([]);
    const [corporateNutritions, setCorporateNutritions] = useState([]);
    const [balance, setBalance] = useState(0);
    const CustomSelect = ({ id, value, options, onChange }) => {
        return (_jsx("select", Object.assign({ className: "custom-select", id: id, value: value, onChange: onChange }, { children: options.map(option => _jsx("option", Object.assign({ value: option.id }, { children: option.name }), option.id)) }), void 0));
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
    function onChangeFile(e) {
        return __awaiter(this, void 0, void 0, function* () {
            const formdata = new FormData();
            formdata.append('file', e.target.files[0]);
            const config = {
                headers: {
                    'content-type': 'multipart/form-data',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            };
            const response = yield axios.post('/api/file/create', formdata, config);
            setFile(response.data.url);
        });
    }
    function uploadToBiz() {
        return __awaiter(this, void 0, void 0, function* () {
            if (file === '')
                confirm('Не выбран вайл выгрузки');
            const option = {
                organizationId: organizationId,
                corporateNutritionId: corporateNutritionId,
                categoryId: categoryId,
                balance: balance,
                fileReport: file,
                options: {
                    refreshBalance: refreshBalance,
                    rename: rename
                }
            };
            console.log('option ', JSON.stringify(option));
            const response = yield BizService.upladCustomersToBiz(option);
            setTaskId(response.data);
            console.log('taskId', response.data);
        });
    }
    useEffect(() => {
        firstInit();
        console.log('UploadForm useEffect');
    }, []);
    if (organizationstore.isLoading) {
        return _jsx("h1", { children: "Loading..." }, void 0);
    }
    console.log('UploadForm return');
    return (_jsxs("div", { children: [_jsx("h1", { children: "UPLOAD FORM" }, void 0), _jsxs("p", { children: ["\u043E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u044F: ", organizationId] }, void 0), _jsxs("p", { children: ["\u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044F: ", categoryId] }, void 0), _jsxs("p", { children: ["\u043A\u043E\u0440\u043F\u0438\u0442: ", corporateNutritionId] }, void 0), _jsxs("p", { children: ["\u0444\u0430\u0439\u043B: ", file] }, void 0), _jsxs("div", Object.assign({ className: "form-group col-md-6" }, { children: [_jsx("label", Object.assign({ htmlFor: "organizations" }, { children: "\u041E\u0440\u0433\u0430\u043D\u0438\u0437\u0430\u0446\u0438\u0438" }), void 0), _jsx(CustomSelect, { id: "organizations", value: organizationId, options: organizationstore.organizations, onChange: onOrganizationSelectChange }, void 0)] }), void 0), _jsxs("div", Object.assign({ className: "form-group col-md-6" }, { children: [_jsx("label", Object.assign({ htmlFor: "categories" }, { children: "\u041A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u0438" }), void 0), _jsx(CustomSelect, { id: "categories", value: categoryId, options: categories, onChange: event => setCategoryId(event.target.value) }, void 0)] }), void 0), _jsxs("div", Object.assign({ className: "form-group col-md-6" }, { children: [_jsx("label", Object.assign({ htmlFor: "corporateNutritions" }, { children: "\u041F\u0440\u043E\u0433\u0440\u0430\u043C\u043C\u044B \u043F\u0438\u0442\u0430\u043D\u0438\u044F" }), void 0), _jsx(CustomSelect, { id: "corporateNutritions", value: corporateNutritionId, options: corporateNutritions, onChange: event => setCorporateNutritionId(event.target.value) }, void 0)] }), void 0), _jsx("div", Object.assign({ className: "form-group col-md-6" }, { children: _jsxs("label", Object.assign({ htmlFor: "balance" }, { children: ["\u0411\u0430\u043B\u0430\u043D\u0441", _jsx("input", { id: 'balance', onChange: e => setBalance(parseInt(e.target.value)), value: balance, type: 'number', placeholder: '\u0411\u0430\u043B\u0430\u043D\u0441' }, void 0)] }), void 0) }), void 0), _jsxs("div", Object.assign({ className: "form-group col-md-7" }, { children: [_jsxs("label", Object.assign({ htmlFor: 'refreshBalance', className: "label-checkbox" }, { children: [_jsx("input", { id: 'refreshBalance', type: 'checkbox', checked: refreshBalance, onChange: () => setRefreshBalance(!refreshBalance) }, void 0), "\u041E\u0431\u043D\u043E\u0432\u043B\u044F\u0442\u044C \u0431\u0430\u043B\u0430\u043D\u0441", _jsx("i", Object.assign({ className: "material-icons red-text" }, { children: refreshBalance ? 'check_box' : 'check_box_outline_blank' }), void 0)] }), void 0), _jsxs("label", Object.assign({ htmlFor: "rename", className: "label-checkbox" }, { children: [_jsx("input", { id: 'rename', type: 'checkbox', checked: rename, onChange: () => setRename(!rename) }, void 0), "\u041F\u0435\u0440\u0435\u0438\u043C\u0435\u043D\u043E\u0432\u0430\u0442\u044C \u0432 \u0441\u043E\u043E\u0442\u0432\u0435\u0442\u0441\u0442\u0432\u0438\u0438 \u0441 \u043D\u043E\u0432\u044B\u043C \u0441\u043F\u0438\u0441\u043A\u043E\u043C", _jsx("i", Object.assign({ className: "material-icons red-text" }, { children: rename ? 'check_box' : 'check_box_outline_blank' }), void 0)] }), void 0), _jsx("label", Object.assign({ htmlFor: "file" }, { children: "\u0412\u044B\u0431\u0438\u0440\u0438\u0442\u0435 \u0444\u0430\u0439\u043B" }), void 0), _jsx("br", {}, void 0), _jsx("input", { className: "form-group", id: 'file', type: 'file', onChange: onChangeFile }, void 0)] }), void 0), _jsx("button", Object.assign({ className: "uploadToBiz", onClick: uploadToBiz }, { children: "\u0412\u044B\u0433\u0440\u0443\u0437\u0438\u0442\u044C" }), void 0)] }, void 0));
};
export default observer(UploadForm);
