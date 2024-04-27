
import { makeAutoObservable, observable, configure } from 'mobx';
import TaskService from '../services/TaskService';
import {ISavedTask} from "./models/ISavedTask";
import {ITask} from "../api/models/task/ITask";

configure({
    enforceActions: "never",
})


export default class TaskStore {
    timer = 0;
    tasks: ISavedTask[] = JSON.parse(localStorage.getItem('tasks') || '[]') as ISavedTask[]
    
    constructor() {
        makeAutoObservable(this, {}, { autoBind: true });
        setInterval(this.increaseTimer, 1000);
    }
    async increaseTimer() {
        this.timer++;
        const currents = this.tasks
            .filter(task => task.task.status === "Running")
            .map(task=>task.id);
        const response = await TaskService.getTasks(currents)

        if (response.status === 200 && response.data)
            this.tasks = this.tasks.map((t,index) => {
                if (t.task.status !== 'Running') { return t }
                this.setTaskInfo(t.id, index, response.data)
                return t
            })

    }

    async setTaskInfo(taskId: string, index: number, updatedTasks: ITask[]){

        const task = updatedTasks.find(t=>t.id === taskId);
        if (task)
        {
            this.tasks[index].task = task
            this.tasks[index].time++
        }
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }

    async onCancelTask(taskId: string) {
        const response = await TaskService.cancelTask(taskId)
        if (response.status === 200 && response.data)
        {
            const t = this.tasks.filter(t => t.id === taskId)[0]
            t.task = response.data
            this.tasks = this.tasks.filter(t => t.id !== taskId)
            this.tasks.push(t)
            localStorage.setItem('tasks', JSON.stringify(this.tasks))
        }


    }

    onRemoveTask(taskId :string) {
        this.tasks = this.tasks.filter(t => t.id !== taskId)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }
    onAddTask(task: ITask, description: string) {
        const savedTask:ISavedTask = {
            id: task.id,
            description: description,
            time:0,
            task:task
        }
        this.tasks.unshift(savedTask)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }
    
}




 
