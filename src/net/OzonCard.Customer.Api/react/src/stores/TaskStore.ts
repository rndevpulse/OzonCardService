
import { makeAutoObservable, observable, configure } from 'mobx';
import TaskService from '../services/TaskService';
import {ITask} from "../models/task/ITask";
import LoginStore from "./LoginStore";

configure({
    enforceActions: "never",
})


export default class TaskStore {
    // timer = 0;
    tasks: ITask[] = this.tryGetSavedTasks();




    constructor() {
        makeAutoObservable(this, {}, { autoBind: true });
        setInterval(this.increaseTimer, 5000);
    }

    tryGetSavedTasks():ITask[]{
        try {
            return JSON.parse(localStorage.getItem('tasks') || '[]') as ITask[];

        }
        catch(error) {
            console.log(error)
            localStorage.clear()
            return [];
        }
    }

    static continueStatuses = ['Enqueued', 'Processing', 'Scheduled'];
    async increaseTimer() {
        // console.log(`TaskStore increaseTimer login status: ${LoginStore.isAuthenticated}`)
        if (LoginStore.isAuthenticated === false)
            return

        // this.timer++;
        //берем задачи, которые нужно отследить
        const currents = this.tasks
            .filter(task => TaskStore.continueStatuses.includes(task.status ))
            .map(task=>task.id);
        // if (currents.length === 0)
        //     return
        //получаем список с нужными и прочими задачами пользователя
        const response = await TaskService.getTasks(currents)
        if (!response)
        {
            return
        }
        if (response.status === 200 && response.data.length === 0)
        {
        }
        //мы все же получили нормальный ответ....
        if (response.status === 200 && response.data)
        {
            const processed:string[] = []
            let oldTasks = this.tasks;
            this.tasks =  response.data.map(response=>{
                let local = this.tasks.find(t=>t.id === response.id)
                if (local)
                {
                    processed.push(local.id)
                    if (response.title === undefined || response.title === "")
                        response.title = local.title
                    return response
                }
                return response
            })
            this.tasks.push(...oldTasks.filter(x=>!processed.includes(x.id)))
            this.tasks = this.tasks.sort((x,y,)=>x.queuedAt > y.queuedAt ? -1 : 1)

            localStorage.setItem('tasks', JSON.stringify(this.tasks))
        }

    }


    async onCancelTask(taskId: string) {
        const response = await TaskService.cancelTask(taskId)
        if (response.status === 200 && response.data)
        {
            const localTask = this.tasks.filter(t => t.id === taskId)[0]
            if (response.data && localTask.title !== response.data?.title)
                response.data.title = localTask.title;

            this.tasks = this.tasks.filter(t => t.id !== taskId)
            this.tasks.push(response.data)
            localStorage.setItem('tasks', JSON.stringify(this.tasks))
        }


    }

    async onRemoveTask(taskId :string) {
        // console.log("onRemoveTask", taskId)
        const response = await TaskService.removeTask(taskId)
        this.tasks = this.tasks.filter(t => t.id !== taskId)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
    }

    onAddTask(task: ITask, title: string = "") {
        console.log(`add task: ${task.title}`);

        if (task.title === undefined || task.title === "")
            task.title = title
        this.tasks.unshift(task)
        localStorage.setItem('tasks', JSON.stringify(this.tasks))
        // console.log(this.tasks)
    }

    clearTasks(): void {
        localStorage.setItem('tasks', '[]')
    }
    
}




 
