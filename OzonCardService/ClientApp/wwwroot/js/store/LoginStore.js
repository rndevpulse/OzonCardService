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
const axios_1 = __importDefault(require("axios"));
const mobx_1 = require("mobx");
const AuthService_1 = __importDefault(require("../services/AuthService"));
class LoginStore {
    constructor() {
        this.email = '';
        this.rules = [];
        this.isAuth = false;
        this.isLoading = false;
        mobx_1.makeAutoObservable(this);
    }
    setIsAuth(bool) {
        this.isAuth = bool;
    }
    setMail(mail) {
        this.email = mail;
    }
    setLoading(bool) {
        this.isLoading = bool;
    }
    setRules(rules) {
        this.rules = rules;
    }
    login(email, password) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const response = yield AuthService_1.default.login(email, password);
                localStorage.setItem('token', response.data.token);
                this.setIsAuth(true);
                this.setMail(response.data.email);
                this.setRules(response.data.rules);
                console.log(response);
            }
            catch (e) {
                console.log(e);
            }
        });
    }
    logout() {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const responce = yield AuthService_1.default.logout(undefined);
                localStorage.removeItem('token');
                this.setIsAuth(false);
                this.setMail("");
                console.log(responce);
            }
            catch (e) {
                console.log(e);
            }
        });
    }
    checkAuth() {
        return __awaiter(this, void 0, void 0, function* () {
            this.isLoading = true;
            try {
                const response = yield axios_1.default.post('https://localhost:5401/api/auth/refresh', { withCredentials: true });
                localStorage.setItem('token', response.data.token);
                this.setIsAuth(true);
                this.setMail(response.data.email);
                this.setRules(response.data.rules);
                console.log(response);
            }
            catch (e) {
                console.log(e);
            }
            finally {
                this.isLoading = false;
            }
        });
    }
}
exports.default = LoginStore;
