
import { makeAutoObservable, observable, configure } from 'mobx';
import TaskService from '../services/TaskService';
import {ISavedTask} from "./models/ISavedTask";
import {ITask} from "../models/task/ITask";

configure({
    enforceActions: "never",
})


export default class TaskStore {
    timer = 0;
    tasks: ISavedTask[] = JSON.parse(localStorage.getItem('tasks') || '[]') as ISavedTask[]
    
    constructor() {
        makeAutoObservable(this, {}, { autoBind: true });
        setInterval(this.increaseTimer, 2000);
    }
    async increaseTimer() {
        this.timer++;
        const currents = this.tasks
            .filter(task => task.task.status == "Running")
            .map(task=>task.id);
        if (currents.length === 0)
            return
        const response = await TaskService.getTasks(currents)
        if (!response)
        {
            return
        }
        if (response.status === 200 && response.data.length === 0)
        {
        }
        if (response.status === 200 && response.data)
        {
            // console.log("running tasks info", response.data)

            this.tasks = this.tasks.map((t,index) => {
                if (t.task.status !== 'Running') { return t }
                this.setTaskInfo(t.id, index, response.data)
                return t
            })
        }


    }

    async setTaskInfo(taskId: string, index: number, updatedTasks: ITask[]){
        // console.log("setTaskInfo", taskId)

        const task = updatedTasks.find(t=>t.id === taskId);
        if (task)
        {
            this.tasks[index].task = task
            this.tasks[index].time += 2
        }
        else
            this.tasks[index].task.status = "Failed"
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
        console.log("onRemoveTask", taskId)
        this.tasks = this.tasks.filter(t => t.id !== taskId)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }
    onAddTask(task: ITask, description: string) {
        const savedTask:ISavedTask = {
            id: task.id,
            description: description,
            time:1,
            task:task
        }
        this.tasks.unshift(savedTask)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
        console.log(this.tasks)
    }
    
}




 
