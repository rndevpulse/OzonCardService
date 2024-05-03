import * as React from 'react'
import { FC, useContext } from 'react';
import { observer } from 'mobx-react-lite';
import {Context} from "../../index";
import './index.css'
import {Task} from "../../components/task";


const TasksPage: FC = () => {



    const { taskStore } = useContext(Context)


    if (taskStore.tasks.length === 0) {
        return <h4 className="center">Задач нет</h4>
    }

    return (
        <div className="center form-group col-md-12">
            <h1>Мои задачи</h1>
            <ul>
                {taskStore.tasks && taskStore.tasks.map(task =>
                    <Task
                        saved={task}
                        onRemove={taskStore.onRemoveTask}
                        onCancel={taskStore.onCancelTask}
                        key={task.id}
                    />
                )}
            </ul>
        </div>
    );
};
export default observer(TasksPage);
