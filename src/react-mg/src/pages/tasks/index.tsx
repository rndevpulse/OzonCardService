import * as React from 'react'
import { FC, useContext } from 'react';
import { observer } from 'mobx-react-lite';
import {Context} from "../../index";
import {ICustomersTasksProgress, ITask} from "../../api/models/task/ITask";
import './index.css'
import {ISavedTask} from "../../stores/models/ISavedTask";


const TasksPage: FC = () => {

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
                        <li>Время выполнения: {new Date(savedTask.time * 1000).toTimeString()}</li>
                    </ul>
                    <ul>
                        <li>Изменен баланс у: {progress.countBalance}</li>
                        <li>Присвоена категория: {progress.countCategory}</li>
                        <li>Добавлено в кор.пит: {progress.countProgram}</li>
                        <li>Время создания: {savedTask.task.queuedAt}</li>
                    </ul>
                </dd>
            )
        }
        else {
            return (
                <dd>
                    <ul>
                        <li>Время выполнения: {new Date(savedTask.time * 1000).toTimeString()}</li>
                    </ul>
                    <ul>
                        <li>Время создания: {savedTask.task.queuedAt}</li>
                    </ul>
                </dd>
            )
        }
    }
    const taskTitle = (savedTask: ISavedTask) => {
        if (savedTask.task.status === "Completed") {
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
        } else {
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
    }
    if (taskStore.tasks.length === 0) {
        return <h4 className="center">Задач нет</h4>
    }
    
    return (
        <div>
            <h1 className="center form-group col-md-12">Мои задачи</h1>
            <div className="center form-group col-md-12">
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
