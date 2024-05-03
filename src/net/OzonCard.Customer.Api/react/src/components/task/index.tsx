import {ICustomersTasksProgress, ITask} from "../../models/task";
import {ISavedTask} from "../../stores/models/ISavedTask";
import * as React from "react";

interface ITaskProps {
    saved: ISavedTask,
    onCancel: (id:string) => void
    onRemove: (id:string) => void
}

export function Task({saved, onCancel, onRemove}: ITaskProps) {
    const classes = ['task']
    if (saved.task.status === "Completed") {
        classes.push('completed')
    }
    if (saved.task.status === "Failed" || saved.task.status === "Canceled") {
        classes.push('canceled')
    }

    return (
        <li className={classes.join(' ')} key={saved.id}>
            {taskTitle({saved, onCancel, onRemove})}
            {taskDescription(saved)}
        </li>
    )
}

function padTo2Digits(num: number) {
    return num.toString().padStart(2, '0');

}
function getTime(time: number) : string{
    const date = new Date(time * 1000)
    date.setHours(date.getHours() + date.getTimezoneOffset() / 60);
    return `${padTo2Digits(date.getHours())}:${padTo2Digits(date.getMinutes())}:${padTo2Digits(date.getSeconds())}`;
}
function getLocalTime(time:string):string{
    const date = new Date(time)
    return date.toLocaleTimeString();
}

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
                    <li>Время создания: {getLocalTime(savedTask.task.queuedAt)}</li>
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
                    <li>Время создания: {getLocalTime(savedTask.task.queuedAt)}</li>
                </ul>
            </dd>
        )
    }
}
const taskTitle = (props: ITaskProps) => {
    if (props.saved.task.status === "Running") {
        return (
            <dt>
                {props.saved.description}
                {props.saved.task.error}
                <i className="material-icons red-text"
                   onClick={() => props.onCancel(props.saved.id)}>
                    cancel
                </i>
            </dt>
        )
    }
    return (
        <dt>
            {props.saved.description}
            {props.saved.task.error}
            <i className="material-icons red-text"
               onClick={() => props.onRemove(props.saved.id)}>
                delete
            </i>
        </dt>
    )
}