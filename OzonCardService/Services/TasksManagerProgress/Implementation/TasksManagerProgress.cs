using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System.Collections.Generic;
using System;

namespace OzonCardService.Services.TasksManagerProgress.Implementation
{
    public class TasksManagerProgress : ITasksManagerProgress
    {
        static Dictionary<Guid, ProgressTask<ProgressInfo<IInfoData>>> DictionaryTasks = new Dictionary<Guid, ProgressTask<ProgressInfo<IInfoData>>>();

        public Guid AddTask(ProgressTask<ProgressInfo<IInfoData>> progress)
        {
            var guid = Guid.NewGuid();
            DictionaryTasks.Add(guid, progress);
            return guid;
        }

        public IInfoData GetStatusTask(Guid taskId)
        {
            ProgressTask<ProgressInfo<IInfoData>> progress;
            DictionaryTasks.TryGetValue(taskId, out progress);
            var status = progress?.Vallue.Status ?? null;
            if (status != null && status.isCompleted)
            {
                DictionaryTasks.Remove(taskId);
            }
            return status ?? default;

        }
    }
}
