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
var mobx_1 = require("mobx");
var OrganizationServise_1 = require("../services/OrganizationServise");
var OrganizationStore = /** @class */ (function () {
    function OrganizationStore() {
        this.organizations = [];
        this.isLoading = false;
        mobx_1.makeAutoObservable(this);
    }
    OrganizationStore.prototype.setLoading = function (bool) {
        this.isLoading = bool;
    };
    OrganizationStore.prototype.requestOrganizations = function () {
        return __awaiter(this, void 0, void 0, function () {
            var response, e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.setLoading(true);
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, 4, 5]);
                        return [4 /*yield*/, OrganizationServise_1.default.getMyOrganizations()];
                    case 2:
                        response = _a.sent();
                        this.organizations = response.data;
                        console.log(response);
                        return [3 /*break*/, 5];
                    case 3:
                        e_1 = _a.sent();
                        console.log(e_1);
                        return [3 /*break*/, 5];
                    case 4:
                        this.setLoading(false);
                        return [7 /*endfinally*/];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    OrganizationStore.prototype.updateOrganization = function (organizationId) {
        return __awaiter(this, void 0, void 0, function () {
            var response_1, e_2;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.setLoading(true);
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, 4, 5]);
                        return [4 /*yield*/, OrganizationServise_1.default.updateOrganization(organizationId)];
                    case 2:
                        response_1 = _a.sent();
                        this.organizations = this.organizations.filter(function (f) { return f.id !== response_1.data.id; });
                        this.organizations.push(response_1.data);
                        console.log(response_1);
                        return [3 /*break*/, 5];
                    case 3:
                        e_2 = _a.sent();
                        console.log(e_2);
                        return [3 /*break*/, 5];
                    case 4:
                        this.setLoading(false);
                        return [7 /*endfinally*/];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    OrganizationStore.prototype.createOrganization = function (email, password) {
        return __awaiter(this, void 0, void 0, function () {
            var response, e_3;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.setLoading(true);
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, 4, 5]);
                        return [4 /*yield*/, OrganizationServise_1.default.createOrganization(email, password)];
                    case 2:
                        response = _a.sent();
                        this.organizations.push(response.data);
                        console.log(response);
                        return [3 /*break*/, 5];
                    case 3:
                        e_3 = _a.sent();
                        console.log(e_3);
                        return [3 /*break*/, 5];
                    case 4:
                        this.setLoading(false);
                        return [7 /*endfinally*/];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    return OrganizationStore;
}());
exports.default = OrganizationStore;
//# sourceMappingURL=OrganizationStore.js.map