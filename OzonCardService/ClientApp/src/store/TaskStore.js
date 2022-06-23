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
var TaskService_1 = require("../services/TaskService");
mobx_1.configure({
    enforceActions: "never",
});
var TaskStore = /** @class */ (function () {
    function TaskStore() {
        this.timer = 0;
        this.tasks = JSON.parse(localStorage.getItem('tasks') || '[]');
        mobx_1.makeAutoObservable(this, {}, { autoBind: true });
        setInterval(this.increaseTimer, 5000);
    }
    TaskStore.prototype.increaseTimer = function () {
        var _this = this;
        this.timer++;
        this.tasks = this.tasks.map(function (t, index) {
            if (t.isCompleted) {
                return t;
            }
            _this.setTaskInfo(t.taskId, index);
            return t;
        });
    };
    TaskStore.prototype.setTaskInfo = function (taskId, index) {
        return __awaiter(this, void 0, void 0, function () {
            var response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, TaskService_1.default.getTaskUpload(taskId)];
                    case 1:
                        response = _a.sent();
                        console.log('setTaskInfo response ', response);
                        if (response.status === 200) {
                            this.tasks[index].taskInfo = response.data;
                            this.tasks[index].isCompleted = response.data.isCompleted;
                        }
                        else {
                            this.tasks[index].isCompleted = true;
                        }
                        localStorage.setItem('tasks', JSON.stringify(this.tasks));
                        return [2 /*return*/];
                }
            });
        });
    };
    TaskStore.prototype.onRemoveTask = function (taskId) {
        this.tasks = this.tasks.filter(function (t) { return t.taskId !== taskId; });
        localStorage.setItem('tasks', JSON.stringify(this.tasks));
    };
    TaskStore.prototype.onAddTask = function (taskId, deskription) {
        var task = {
            taskId: taskId,
            deskription: deskription,
            taskInfo: undefined,
            isCompleted: false,
            created: new Date().toLocaleTimeString() + " " + new Date().toLocaleDateString()
        };
        this.tasks.push(task);
        localStorage.setItem('tasks', JSON.stringify(this.tasks));
    };
    return TaskStore;
}());
exports.default = TaskStore;
//# sourceMappingURL=TaskStore.js.map