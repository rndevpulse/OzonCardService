import { FC, useContext, useEffect, useState } from 'react';
import * as React from 'react'
import { observer } from 'mobx-react-lite';
import { ITask } from '../models/IInfoDataUpload';
import TaskService from '../services/TaskService';
import { Context } from '..';

const TasksForm: FC = () => {

    const { taskstore } = useContext(Context)
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
        return <p className="center">Задач нет</p>
    }
    return (
        <div>
            <h1>TaskForm</h1>
            <div className="form-group col-md-8">
                <ul>
                    {taskstore.tasks && taskstore.tasks.map(task => {
                        const classes = ['task']
                        if (task.isCompleted) {
                            classes.push('completed')
                        }
                        return (
                            <li className={classes.join(' ')} key={task.taskId}>
                                <label>
                                    {task.deskription}
                                    
                                    
                                    <h6>
                                        countCustomersAll: {task.taskInfo?.countCustomersAll}
                                        countCustomersNew: {task.taskInfo?.countCustomersNew}
                                        countCustomersFail: {task.taskInfo?.countCustomersFail}
                                        countCustomersBalance: {task.taskInfo?.countCustomersBalance}
                                        countCustomersCategory: {task.taskInfo?.countCustomersCategory}
                                        countCustomersCorporateNutritions: {task.taskInfo?.countCustomersCorporateNutritions}
                                        timeCompleted: {task.taskInfo?.timeCompleted.substring(0,8)}
                                    </h6>
                                    
                                    <i className="material-icons red-text"
                                        onClick={() => taskstore.onRemoveTask(task.taskId)}>
                                        delete
                                    </i>
                                </label>
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
