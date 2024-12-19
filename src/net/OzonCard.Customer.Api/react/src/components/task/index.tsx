import {ICategoriesTasksProgress, ICustomersTasksProgress, IReportsTasksProgress, ITask} from "../../models/task";
import * as React from "react";
import FileService from "../../services/FileServise";

interface ITaskProps {
    saved: ITask,
    onCancel: (id:string) => void
    onRemove: (id:string) => void
}

export function Task({saved, onCancel, onRemove}: ITaskProps) {
    const classes = ['task']
    if (saved.status === "Succeeded") {
        classes.push('completed')
    }
    if (saved.status === "Failed" || saved.status === "Deleted") {
        classes.push('canceled')
    }

    return (
        <li className={classes.join(' ')} key={saved.id}>
            {taskTitle({saved, onCancel, onRemove})}
            {saved.error && taskError(saved)}
            {switchTaskDescription(saved)}
        </li>
    )
}

function padTo2Digits(num: number) {
    return num.toString().padStart(2, '0');

}
function getTimeWork(task: ITask) : string{
    // const date = new Date(time * 1000)
    // date.setHours(date.getHours() + date.getTimezoneOffset() / 60);
    // return `${padTo2Digits(date.getHours())}:${padTo2Digits(date.getMinutes())}:${padTo2Digits(date.getSeconds())}`;

    if (!task.processedAt) return "";

    let processedAt = new Date(task.processedAt);
    let closedAt = task.completedAt !== undefined
        ? new Date(task.completedAt)
        : new Date();
    let result = new Date(closedAt.getTime() - processedAt.getTime());
    return  `${padTo2Digits(result.getUTCHours() + (result.getDate()-1)*24)}`
        +`:${padTo2Digits(result.getUTCMinutes())}`
        +`:${padTo2Digits(result.getUTCSeconds())}`;

    // return  result.toLocaleTimeString();
}
function getLocalTime(time:Date):string{
    return new Date(time).toLocaleTimeString()
}

const switchTaskDescription = (savedTask: ITask) =>{

    switch (savedTask.progress?.Type) {
        case undefined: return taskDefaultDescription(savedTask);
        case "CustomersTaskProgress":
            return taskCustomerDescription(savedTask)
        case "ReportsTaskProgress":
            return taskReportDescription(savedTask)
        case "CategoriesTaskProgress":
            return taskCategoriesDescription(savedTask)
    }

}

const taskCategoriesDescription = (savedTask: ITask) => {
    const status = savedTask.progress as ICategoriesTasksProgress;

    return (
        <dd>
            <div className={"description-simple"}>{status.Log}</div>
            <ul>
                <li>Обработано: {status.Processed}</li>
                <li>Гостей всего: {status.All}</li>
                {savedTask.status === 'Processing' && <li>Время выполнения: {getTimeWork(savedTask)}</li>}
                <li>Время создания: {getLocalTime(savedTask.queuedAt)}</li>
            </ul>
            {savedTask.result
                && savedTask.result?.Id
                && savedTask.result?.Format
                && savedTask.result?.Name
                && onViewSaveButton(
                    `${savedTask.result.Id}.${savedTask.result?.Format}`,
                    `${savedTask.result.Name}.${savedTask.result?.Format}`
                )
            }
        </dd>
)
}

const taskReportDescription = (savedTask: ITask) => {
    const status = savedTask.progress as IReportsTasksProgress;
    return (
        <dd>
            <div className={"description-simple"}>

                <ul>
                    <li>Процесс выполнения: {status.Progress}% {status.Description}</li>
                    <li>Время выполнения: {getTimeWork(savedTask)}</li>
                    <li>Время создания: {getLocalTime(savedTask.queuedAt)}</li>
                </ul>

            </div>
            {savedTask.result
                && savedTask.result?.Id
                && savedTask.result?.Format
                && savedTask.result?.Name
                && onViewSaveButton(
                    `${savedTask.result.Id}.${savedTask.result?.Format}`,
                    `${savedTask.result.Name}.${savedTask.result?.Format}`
                )
            }
        </dd>

    )
}
const onViewSaveButton = (link:string, name:string)=>{
    // console.log("try save report from task",link)
    return (
        <i className="material-icons blue-text"
           onClick={(e) => onSaveFile(link, name)}
        >
            file_download
        </i>
    )
}
const taskDefaultDescription = (savedTask: ITask) => {
    return (
        <dd>
            <ul>
                {savedTask.status === "Processing"  &&  <li>Время выполнения: {getTimeWork(savedTask)}</li>}
            </ul>
            <ul>
                <li>Время создания: {getLocalTime(savedTask.queuedAt)}</li>
            </ul>
        </dd>
    )
}
const taskCustomerDescription = (savedTask: ITask) => {
    // console.log(savedTask.progress);
    const status = savedTask.progress as ICustomersTasksProgress;
    return (
        <dd>
            <ul>
                <li>Гостей всего: {status.CountAll}</li>
                <li>Новых: {status.CountNew}</li>
                <li>Обработано с ошибкой: {status.CountFail}</li>
                <li>Время выполнения: {getTimeWork(savedTask)}</li>
            </ul>
            <ul>
                <li>Изменен баланс у: {status.CountBalance}</li>
                <li>Присвоена категория: {status.CountCategory}</li>
                <li>Добавлено в кор.пит: {status.CountProgram}</li>
                <li>Время создания: {getLocalTime(savedTask.queuedAt)}</li>
            </ul>
        </dd>
    )
}
const taskTitle = (props: ITaskProps) => {
    if (props.saved.status === "Processing" || props.saved.status === "Scheduled") {
        return (
            <dt>
                {props.saved.title}
                <i className="material-icons red-text"
                   onClick={() => props.onCancel(props.saved.id)}>
                    cancel
                </i>
            </dt>
        )
    }
    return (
        <dt>
            {props.saved.title}
            <i className="material-icons red-text"
               onClick={() => props.onRemove(props.saved.id)}>
                delete
            </i>
        </dt>
    )
}
const taskError = (saved: ITask) => {
    return(
        <div className={"task-error"}>
            Ошибка: {saved.error}
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
