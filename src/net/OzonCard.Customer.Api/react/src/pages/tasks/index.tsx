import * as React from 'react'
import { FC, useContext } from 'react';
import { observer } from 'mobx-react-lite';
import {Context} from "../../index";
import './index.css'
import {Task} from "../../components/task";
import {Tab, TabList, TabPanel, Tabs} from "react-tabs";
import {ISavedTask} from "../../stores/models/ISavedTask";



const TasksPage: FC = () => {
    const { taskStore } = useContext(Context)

    const current = ["Enqueued","Processing"]
    const closed = ["Deleted","Failed","Succeeded"]


    function getTasks(tasks: ISavedTask[]){
        if (tasks.length === 0) {
            return <h4 className="center">Задач нет</h4>
        }
        return (
            <ul>
                {tasks.map(task =>
                    <Task
                        saved={task}
                        onRemove={taskStore.onRemoveTask}
                        onCancel={taskStore.onCancelTask}
                        key={task.id}
                    />
                )}
            </ul>
        )
    }

    return (
        <div className="center form-group col-md-12">
            <h1>Мои задачи</h1>
            <Tabs>
                <TabList>
                    <Tab>Текущие</Tab>
                    <Tab>Отложенные</Tab>
                    {/*<Tab>Выполненные</Tab>*/}
                </TabList>
                <TabPanel>
                    {getTasks(taskStore.tasks.filter(x=>x.task.status !== "Scheduled"))}
                </TabPanel>
                <TabPanel>
                    {getTasks(taskStore.tasks.filter(x=>x.task.status === "Scheduled"))}
                </TabPanel>

            </Tabs>
        </div>
    );
};
export default observer(TasksPage);
