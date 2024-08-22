import {ICustomersTasksProgress, IReportsTasksProgress, ITask} from "../../models/task";
import {ISavedTask} from "../../stores/models/ISavedTask";
import * as React from "react";
import FileService from "../../services/FileServise";

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
            {saved.task.error && taskError(saved)}
            {switchTaskDescription(saved)}
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
    let t = new Date(time);
    return new Date(t.getTime() - (t.getTimezoneOffset() * 60000)).toLocaleTimeString()
}

const switchTaskDescription = (savedTask: ISavedTask) =>{

    switch (savedTask.task.progress?.Type) {
        case undefined: return taskDefaultDescription(savedTask);
        case "CustomersTaskProgress":
            return taskCustomerDescription(savedTask)
        case "ReportsTaskProgress":
            return taskReportDescription(savedTask)
    }

}


const taskReportDescription = (savedTask: ISavedTask) => {
    const status = savedTask.task.progress as IReportsTasksProgress;
    return (
        <dd>
            <div className={"description-simple"}>

                <ul>
                    <li>Процесс выполнения: {status.Progress}% {status.Description}</li>
                    <li>Время выполнения: {getTime(savedTask.time)}</li>
                    <li>Время создания: {getLocalTime(savedTask.task.queuedAt)}</li>
                </ul>

            </div>
            {savedTask.task.result
                && savedTask.task.result?.Id
                && savedTask.task.result?.Format
                && savedTask.task.result?.Name
                && onViewSaveButton(
                    `${savedTask.task.result.Id}.${savedTask.task.result?.Format}`,
                    `${savedTask.task.result.Name}.${savedTask.task.result?.Format}`
                )
            }
        </dd>

    )
}
const onViewSaveButton = (link:string, name:string)=>{
    console.log("try save report from task",link)
    return (
        <i className="material-icons blue-text"
           onClick={(e) => onSaveFile(link, name)}
        >
            file_download
        </i>
    )
}
const taskDefaultDescription = (savedTask: ISavedTask) => {
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
const taskCustomerDescription = (savedTask: ISavedTask) => {
    // console.log(savedTask.task.progress);
    const status = savedTask.task.progress as ICustomersTasksProgress;
    return (
        <dd>
            <ul>
                <li>Гостей всего: {status.CountAll}</li>
                <li>Новых: {status.CountNew}</li>
                <li>Обработано с ошибкой: {status.CountFail}</li>
                <li>Время выполнения: {getTime(savedTask.time)}</li>
            </ul>
            <ul>
                <li>Изменен баланс у: {status.CountBalance}</li>
                <li>Присвоена категория: {status.CountCategory}</li>
                <li>Добавлено в кор.пит: {status.CountProgram}</li>
                <li>Время создания: {getLocalTime(savedTask.task.queuedAt)}</li>
            </ul>
        </dd>
    )
}
const taskTitle = (props: ITaskProps) => {
    if (props.saved.task.status === "Running") {
        return (
            <dt>
                {props.saved.description}
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
            <i className="material-icons red-text"
               onClick={() => props.onRemove(props.saved.id)}>
                delete
            </i>
        </dt>
    )
}
const taskError = (saved: ISavedTask) => {
    return(
        <div className={"task-error"}>
            Ошибка: {saved.task.error}
        </div>
    )
}



async function onSaveFile(url: string, name: string) {
    //console.log('downloadHandler', url, name)
    FileService.downloadFile(url)
        .then(response => {
            const type = response.headers['content-type']
            const blob = new Blob([response.data], { type: type })
            const _url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = _url
            link.setAttribute('download', name);
            document.body.appendChild(link)
            link.click()
            link.remove()
        })
}
