"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_1 = require("react");
var mobx_react_lite_1 = require("mobx-react-lite");
var __1 = require("..");
var TasksForm = function () {
    var taskstore = react_1.useContext(__1.Context).taskstore;
    var taskDescription = function (task) {
        var _a, _b, _c, _d, _e, _f, _g, _h;
        if (task.taskInfo && task.taskInfo.countCustomersAll) {
            return (React.createElement("dd", null,
                React.createElement("ul", null,
                    React.createElement("li", null,
                        "\u0413\u043E\u0441\u0442\u0435\u0439 \u0432\u0441\u0435\u0433\u043E: ", (_a = task.taskInfo) === null || _a === void 0 ? void 0 :
                        _a.countCustomersAll),
                    React.createElement("li", null,
                        "\u041D\u043E\u0432\u044B\u0445: ", (_b = task.taskInfo) === null || _b === void 0 ? void 0 :
                        _b.countCustomersNew),
                    React.createElement("li", null,
                        "\u041E\u0431\u0440\u0430\u0431\u043E\u0442\u0430\u043D\u043E \u0441 \u043E\u0448\u0438\u0431\u043A\u043E\u0439: ", (_c = task.taskInfo) === null || _c === void 0 ? void 0 :
                        _c.countCustomersFail),
                    React.createElement("li", null,
                        "\u0412\u0440\u0435\u043C\u044F \u0432\u044B\u043F\u043E\u043B\u043D\u0435\u043D\u0438\u044F: ", (_d = task.taskInfo) === null || _d === void 0 ? void 0 :
                        _d.timeCompleted.substring(0, 8))),
                React.createElement("ul", null,
                    React.createElement("li", null,
                        "\u0418\u0437\u043C\u0435\u043D\u0435\u043D \u0431\u0430\u043B\u0430\u043D\u0441 \u0443: ", (_e = task.taskInfo) === null || _e === void 0 ? void 0 :
                        _e.countCustomersBalance),
                    React.createElement("li", null,
                        "\u041F\u0440\u0438\u0441\u0432\u043E\u0435\u043D\u0430 \u043A\u0430\u0442\u0435\u0433\u043E\u0440\u0438\u044F: ", (_f = task.taskInfo) === null || _f === void 0 ? void 0 :
                        _f.countCustomersCategory),
                    React.createElement("li", null,
                        "\u0414\u043E\u0431\u0430\u0432\u043B\u0435\u043D\u043E \u0432 \u043A\u043E\u0440.\u043F\u0438\u0442: ", (_g = task.taskInfo) === null || _g === void 0 ? void 0 :
                        _g.countCustomersCorporateNutritions),
                    React.createElement("li", null,
                        "\u0412\u0440\u0435\u043C\u044F \u0441\u043E\u0437\u0434\u0430\u043D\u0438\u044F: ",
                        task.created))));
        }
        else {
            return (React.createElement("dd", null,
                React.createElement("ul", null,
                    React.createElement("li", null,
                        "\u0412\u0440\u0435\u043C\u044F \u0432\u044B\u043F\u043E\u043B\u043D\u0435\u043D\u0438\u044F: ", (_h = task.taskInfo) === null || _h === void 0 ? void 0 :
                        _h.timeCompleted.substring(0, 8))),
                React.createElement("ul", null,
                    React.createElement("li", null,
                        "\u0412\u0440\u0435\u043C\u044F \u0441\u043E\u0437\u0434\u0430\u043D\u0438\u044F: ",
                        task.created))));
        }
    };
    var taskTitle = function (task) {
        if (!task.isCompleted) {
            return (React.createElement("dt", null,
                task.deskription,
                React.createElement("i", { className: "material-icons red-text", onClick: function () { return taskstore.onCancelTask(task.taskId); } }, "cancel")));
        }
        else {
            return (React.createElement("dt", null,
                task.deskription,
                React.createElement("i", { className: "material-icons red-text", onClick: function () { return taskstore.onRemoveTask(task.taskId); } }, "delete")));
        }
    };
    if (taskstore.tasks.length === 0) {
        return React.createElement("h4", { className: "center" }, "\u0417\u0430\u0434\u0430\u0447 \u043D\u0435\u0442");
    }
    return (React.createElement("div", null,
        React.createElement("h1", { className: "center form-group col-md-12" }, "\u041C\u043E\u0438 \u0437\u0430\u0434\u0430\u0447\u0438"),
        React.createElement("div", { className: "center form-group col-md-12" },
            React.createElement("ul", null, taskstore.tasks && taskstore.tasks.map(function (task) {
                var _a, _b;
                var classes = ['task'];
                if (task.isCompleted && ((_a = task.taskInfo) === null || _a === void 0 ? void 0 : _a.isCancel) === false) {
                    classes.push('completed');
                }
                if (task.isCancel || ((_b = task.taskInfo) === null || _b === void 0 ? void 0 : _b.isCancel) === true) {
                    classes.push('canceled');
                }
                return (React.createElement("li", { className: classes.join(' '), key: task.taskId },
                    taskTitle(task),
                    taskDescription(task)));
            })))));
};
exports.default = mobx_react_lite_1.observer(TasksForm);
//# sourceMappingURL=TasksForm.js.map