
import { makeAutoObservable, observable, configure } from 'mobx';
import TaskService from '../services/TaskService';
import {ITask} from "../api/models/task/ITask";

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
            if (t.status == 'Completed') { return t }
            this.setTaskInfo(t.id, index)
            return t
        })

    }

    async setTaskInfo(taskId: string, index: number){
        const response = await TaskService.getTaskUpload(taskId)
        //console.log('setTaskInfo response ', response)
        if (response.status === 200) {
            this.tasks[index].status = response.data.status
            this.tasks[index].completedAt = response.data.completedAt
            this.tasks[index].error = response.data.error
        }
        else {
            this.tasks[index].status = "Completed"
        }
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }

    async onCancelTask(taskId: string) {
        const t = this.tasks.filter(t => t.id === taskId)[0]
        t.status = "Failed"
        this.tasks = this.tasks.filter(t => t.id !== taskId)
        this.tasks.push(t)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
        await TaskService.cancelTask(taskId)
    }

    onRemoveTask(taskId :string) {
        this.tasks = this.tasks.filter(t => t.id !== taskId)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }
    onAddTask(task: ITask, description: string) {
        task.comment = description
        this.tasks.unshift(task)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }
    
}




 
