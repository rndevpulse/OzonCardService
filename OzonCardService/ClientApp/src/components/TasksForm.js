"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var react_1 = require("react");
var React = require("react");
var mobx_react_lite_1 = require("mobx-react-lite");
var __1 = require("..");
var TasksForm = function () {
    var taskstore = react_1.useContext(__1.Context).taskstore;
    //const [tasks, setTasks] = useState<ITask[]>([])
    //useEffect(() => {
    //    const list = JSON.parse(localStorage.getItem('tasks') || '[]') as ITask[]
    //    setTasks(list)
    //    console.log('useEffect []')
    //},[])
    //useEffect(() => {
    //    localStorage.setItem('tasks', JSON.stringify(tasks))
    //    console.log('useEffect [tasks]')
    //}, [tasks])
    //async function requestTask(task: ITask) {
    //    try {
    //        if (task && task?.isCompleted) {
    //            return;
    //        }
    //        const response = await TaskService.getTaskUpload(task.taskId);
    //        setTasks(prev => prev.map(t => {
    //            if (t.taskId === task.taskId) {
    //                t.isCompleted = response.data.isCompleted
    //                t.taskInfo = response.data
    //            }
    //            return t
    //        }))
    //        console.log('requestTask: ', JSON.stringify(task));
    //    }
    //    catch (e) {
    //        console.log(e);
    //    }
    //}
    //const onRemoveTask = (taskId: string) =>{
    //    setTasks(prev=> prev.filter(t=>t.taskId !== taskId))
    //    console.log('removeTask', JSON.stringify(tasks))
    //}
    if (taskstore.tasks.length === 0) {
        return React.createElement("p", { className: "center" }, "\u0417\u0430\u0434\u0430\u0447 \u043D\u0435\u0442");
    }
    return (React.createElement("div", null,
        React.createElement("h1", null, "TaskForm"),
        React.createElement("div", { className: "form-group col-md-8" },
            React.createElement("ul", null, taskstore.tasks && taskstore.tasks.map(function (task) {
                var _a, _b, _c, _d, _e, _f, _g;
                var classes = ['task'];
                if (task.isCompleted) {
                    classes.push('completed');
                }
                return (React.createElement("li", { className: classes.join(' '), key: task.taskId },
                    React.createElement("label", null,
                        task.deskription,
                        React.createElement("h6", null,
                            "countCustomersAll: ", (_a = task.taskInfo) === null || _a === void 0 ? void 0 :
                            _a.countCustomersAll,
                            "countCustomersNew: ", (_b = task.taskInfo) === null || _b === void 0 ? void 0 :
                            _b.countCustomersNew,
                            "countCustomersFail: ", (_c = task.taskInfo) === null || _c === void 0 ? void 0 :
                            _c.countCustomersFail,
                            "countCustomersBalance: ", (_d = task.taskInfo) === null || _d === void 0 ? void 0 :
                            _d.countCustomersBalance,
                            "countCustomersCategory: ", (_e = task.taskInfo) === null || _e === void 0 ? void 0 :
                            _e.countCustomersCategory,
                            "countCustomersCorporateNutritions: ", (_f = task.taskInfo) === null || _f === void 0 ? void 0 :
                            _f.countCustomersCorporateNutritions,
                            "timeCompleted: ", (_g = task.taskInfo) === null || _g === void 0 ? void 0 :
                            _g.timeCompleted.substring(0, 8)),
                        React.createElement("i", { className: "material-icons red-text", onClick: function () { return taskstore.onRemoveTask(task.taskId); } }, "delete"))));
            })))));
};
exports.default = mobx_react_lite_1.observer(TasksForm);
//# sourceMappingURL=TasksForm.js.map