import * as React from 'react'
import { FC, useContext } from 'react';
import { observer } from 'mobx-react-lite';
import { Context } from '..';
import { ITask } from '../models/IInfoDataUpload';

const TasksForm: FC = () => {

    const { taskstore } = useContext(Context)
    const taskDescription = (task: ITask) => {
        if (task.taskInfo && task.taskInfo.countCustomersAll) {
            return (
                <dd>
                    <ul>
                        <li>Гостей всего: {task.taskInfo?.countCustomersAll}</li>
                        <li>Новых: {task.taskInfo?.countCustomersNew}</li>
                        <li>Обработано с ошибкой: {task.taskInfo?.countCustomersFail}</li>
                        <li>Время выполнения: {task.taskInfo?.timeCompleted.substring(0, 8)}</li>
                    </ul>
                    <ul>
                        <li>Изменен баланс у: {task.taskInfo?.countCustomersBalance}</li>
                        <li>Присвоена категория: {task.taskInfo?.countCustomersCategory}</li>
                        <li>Добавлено в кор.пит: {task.taskInfo?.countCustomersCorporateNutritions}</li>
                        <li>Время создания: {task.created}</li>
                    </ul>
                </dd>
            )
        }
        else {
            return (
                <dd>
                    <ul>
                        <li>Время выполнения: {task.taskInfo?.timeCompleted.substring(0, 8)}</li>
                    </ul>
                    <ul>
                        <li>Время создания: {task.created}</li>
                    </ul>
                </dd>
            )
        }
    }
    const taskTitle = (task: ITask) => {
        if (!task.isCompleted) {
            return (
                <dt>
                {task.deskription}
                <i className="material-icons red-text"
                    onClick={() => taskstore.onCancelTask(task.taskId)}>
                    cancel
                </i>
                </dt>
            )
        }
        else {
            return (
                <dt>
                    {task.deskription}
                    <i className="material-icons red-text"
                        onClick={() => taskstore.onRemoveTask(task.taskId)}>
                        delete
                    </i>
                </dt>
            )
        }
    }
    if (taskstore.tasks.length === 0) {
        return <h4 className="center">Задач нет</h4>
    }
    
    return (
        <div>
            <h1 className="center form-group col-md-12">Мои задачи</h1>
            <div className="center form-group col-md-12">
                <ul>
                    {taskstore.tasks && taskstore.tasks.map(task => {
                        const classes = ['task']
                        if (task.isCompleted && !task.taskInfo.isCancel) {
                            classes.push('completed')
                        }
                        if (task.taskInfo.isCancel) {
                            classes.push('canceled')
                        }
                        return (
                            <li className={classes.join(' ')} key={task.taskId}>
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
export default observer(TasksForm);
