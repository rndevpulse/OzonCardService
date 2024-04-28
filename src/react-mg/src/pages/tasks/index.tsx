import * as React from 'react'
import { FC, useContext } from 'react';
import { observer } from 'mobx-react-lite';
import {Context} from "../../index";
import {ICustomersTasksProgress, ITask} from "../../models/task/ITask";
import './index.css'
import {ISavedTask} from "../../stores/models/ISavedTask";


const TasksPage: FC = () => {

    function padTo2Digits(num: number) {
        return num.toString().padStart(2, '0');

    }
    function getTime(time: number) : string{
        const date = new Date(time * 1000)
        date.setHours(date.getHours() + date.getTimezoneOffset() / 60);
        return `${padTo2Digits(date.getHours())}:${padTo2Digits(date.getMinutes())}:${padTo2Digits(date.getSeconds())}`;
    }

    const { taskStore } = useContext(Context)
    const taskDescription = (savedTask: ISavedTask) => {
        const progress = savedTask.task.progress as ICustomersTasksProgress;
        if (progress && progress.countAll) {
            return (
                <dd>
                    <ul>
                        <li>Гостей всего: {progress.countAll}</li>
                        <li>Новых: {progress.countNew}</li>
                        <li>Обработано с ошибкой: {progress.countFail}</li>
                        <li>Время выполнения: {getTime(savedTask.time)}</li>
                    </ul>
                    <ul>
                        <li>Изменен баланс у: {progress.countBalance}</li>
                        <li>Присвоена категория: {progress.countCategory}</li>
                        <li>Добавлено в кор.пит: {progress.countProgram}</li>
                        <li>Время создания: {savedTask.task.queuedAt.substring(11, 19)}</li>
                    </ul>
                </dd>
            )
        }
        else {
            return (
                <dd>
                    <ul>
                        <li>Время выполнения: {getTime(savedTask.time)}</li>
                    </ul>
                    <ul>
                        <li>Время создания: {savedTask.task.queuedAt.substring(11, 19)}</li>
                    </ul>
                </dd>
            )
        }
    }
    const taskTitle = (savedTask: ISavedTask) => {
        if (savedTask.task.status === "Running") {
            return (
                <dt>
                    {savedTask.description}
                    {savedTask.task.error}
                    <i className="material-icons red-text"
                       onClick={() => taskStore.onCancelTask(savedTask.id)}>
                        cancel
                    </i>
                </dt>
            )
        }
        return (
            <dt>
                {savedTask.description}
                {savedTask.task.error}
                <i className="material-icons red-text"
                   onClick={() => taskStore.onRemoveTask(savedTask.id)}>
                    delete
                </i>
            </dt>
        )
    }
    if (taskStore.tasks.length === 0) {
        return <h4 className="center">Задач нет</h4>
    }
    
    return (
        <div className="center form-group col-md-12">
            <h1>Мои задачи</h1>
            <div>
                <ul>
                    {taskStore.tasks && taskStore.tasks.map(task => {
                        const classes = ['task']
                        if (task.task.status === "Completed") {
                            classes.push('completed')
                        }
                        if (task.task.status === "Failed" || task.task.status === "Canceled") {
                            classes.push('canceled')
                        }
                        return (
                            <li className={classes.join(' ')} key={task.id}>
                                {taskTitle(task)}
                                {taskDescription(task)}
                            </li>
                        );
                    })
                   }
                </ul>
            </div>


        </div>
    );
};
export default observer(TasksPage);
