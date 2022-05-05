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
var react_2 = require("react");
var FileServise_1 = require("../services/FileServise");
var FilesForm = function () {
    var _a = react_1.useState([]), files = _a[0], setFiles = _a[1];
    function getFiles() {
        return __awaiter(this, void 0, void 0, function () {
            var response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, FileServise_1.default.getMyFiles()];
                    case 1:
                        response = _a.sent();
                        console.log(response.data);
                        setFiles(response.data);
                        return [2 /*return*/];
                }
            });
        });
    }
    function onRemoveFile(url) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        console.log('onRemoveFile: ', url);
                        return [4 /*yield*/, FileServise_1.default.removeFile(url)];
                    case 1:
                        _a.sent();
                        return [4 /*yield*/, getFiles()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    }
    function downloadHandler(e, url, name) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                console.log('downloadHandler', url, name);
                e.stopPropagation();
                FileServise_1.default.downloadFile(url)
                    .then(function (response) {
                    var type = response.headers['content-type'];
                    var blob = new Blob([response.data], { type: type });
                    var _url = window.URL.createObjectURL(blob);
                    var link = document.createElement('a');
                    link.href = _url;
                    link.setAttribute('download', name);
                    document.body.appendChild(link);
                    link.click();
                    link.remove();
                });
                return [2 /*return*/];
            });
        });
    }
    react_2.useEffect(function () {
        console.log('useEffect');
        getFiles();
    }, []);
    React.createElement("h1", null, "\u041C\u043E\u0438 \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u044B");
    if (files.length === 0) {
        return React.createElement("h4", { className: "center" }, "\u0424\u0430\u0439\u043B\u043E\u0432 \u043D\u0435\u0442");
    }
    return (React.createElement("div", null,
        React.createElement("h1", null, "\u041C\u043E\u0438 \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u044B"),
        React.createElement("div", { className: "form-group col-md-12" },
            React.createElement("ul", null, files && files.map(function (file) {
                return (React.createElement("li", { className: "file", key: file.url },
                    React.createElement("label", null,
                        React.createElement("span", { onClick: function (e) { return downloadHandler(e, file.url, "" + file.name); } },
                            file.name,
                            " | ",
                            new Date(file.created).toDateString()),
                        React.createElement("span", null,
                            React.createElement("i", { className: "material-icons blue-text", onClick: function (e) { return downloadHandler(e, file.url, "" + file.name); } }, "file_download"),
                            React.createElement("i", { className: "material-icons red-text", onClick: function () { return onRemoveFile(file.url); } }, "delete")))));
            })))));
};
exports.default = FilesForm;
//# sourceMappingURL=FilesForm.js.map