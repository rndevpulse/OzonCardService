
import { makeAutoObservable, observable, configure } from 'mobx';
import { useEffect } from 'react';
import { IInfoDataUpload, ITask } from '../models/IInfoDataUpload';
import TaskService from '../services/TaskService';

configure({
    enforceActions: "never",
})


export default class TaskStore {
    timer = 0;
    tasks: ITask[] = JSON.parse(localStorage.getItem('tasks') || '[]') as ITask[]
    
    constructor() {
        makeAutoObservable(this, {}, { autoBind: true });
        setInterval(this.increaseTimer, 5000);
    }
    increaseTimer() {
        this.timer++;
        this.tasks = this.tasks.map((t,index) => {
            if (t.isCompleted) { return t }
            this.setTaskInfo(t.taskId, index)
            return t
        })

    }

    async setTaskInfo(taskId: string, index: number){
        const response = await TaskService.getTaskUpload(taskId)
        //console.log('setTaskInfo response ', response)
        if (response.status === 200) {
            this.tasks[index].taskInfo = response.data
            this.tasks[index].isCompleted = response.data.isCompleted
            this.tasks[index].isCancel = response.data.isCancel
        }
        else {
            this.tasks[index].isCompleted = true
        }
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }

    async onCancelTask(taskId: string) {
        const t = this.tasks.filter(t => t.taskId === taskId)[0]
        t.isCancel = true
        this.tasks = this.tasks.filter(t => t.taskId !== taskId)
        this.tasks.push(t)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
        await TaskService.cancelTask(taskId)
    }

    onRemoveTask(taskId :string) {
        this.tasks = this.tasks.filter(t => t.taskId !== taskId)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }
    onAddTask(taskId: string, deskription: string) {
        const task: ITask = {
            taskId: taskId,
            deskription: deskription,
            taskInfo: undefined,
            isCompleted: false,
            isCancel:false,
            created: `${new Date().toLocaleTimeString()} ${new Date().toLocaleDateString()}`
        }
        this.tasks.unshift(task)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }
    
}




 
