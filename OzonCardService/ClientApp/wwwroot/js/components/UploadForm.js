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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const jsx_runtime_1 = require("react/jsx-runtime");
const react_1 = require("react");
const OrganizationServise_1 = __importDefault(require("../services/OrganizationServise"));
const react_2 = require("react");
const UploadForm = () => {
    const [organizations, setOrganizations] = react_1.useState([]);
    function getOrganizations() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const response = yield OrganizationServise_1.default.getMyOrganizations();
                console.log('UploadForm response.data = ', response.data);
                setOrganizations(response.data);
            }
            catch (e) {
                console.log(e);
            }
        });
    }
    react_2.useEffect(() => {
        getOrganizations();
        console.log('UploadForm', organizations);
    }, []);
    return (jsx_runtime_1.jsxs("div", { children: [jsx_runtime_1.jsx("h1", { children: "UPLOAD FORM fdg" }, void 0), organizations.map(org => { var _a; return jsx_runtime_1.jsx("div", { children: org.Name }, (_a = org === null || org === void 0 ? void 0 : org.Id) !== null && _a !== void 0 ? _a : '1'); })] }, void 0));
};
exports.default = UploadForm;
