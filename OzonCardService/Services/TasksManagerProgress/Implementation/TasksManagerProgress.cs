using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System.Collections.Generic;
using System;

namespace OzonCardService.Services.TasksManagerProgress.Implementation
{
    public class TasksManagerProgress : ITasksManagerProgress
    {
        static Dictionary<Guid, ProgressTask<ProgressInfo>> DictionaryTasks = new Dictionary<Guid, ProgressTask<ProgressInfo>>();

        public Guid AddTask(ProgressTask<ProgressInfo> progress)
        {
            var guid = Guid.NewGuid();
            DictionaryTasks.Add(guid, progress);
            return guid;
        }

        public IInfoData GetStatusTask(Guid taskId)
        {
            ProgressTask<ProgressInfo> progress;
            DictionaryTasks.TryGetValue(taskId, out progress);
            var status = progress?.Vallue?.Status ?? null;
            if (status != null && (status.isCompleted || status.isCancel))
            {
                DictionaryTasks.Remove(taskId);
            }
            return status ?? default;

        }
        public void CancelTask(Guid taskId)
        {
            ProgressTask<ProgressInfo> progress;
            DictionaryTasks.TryGetValue(taskId, out progress);
            progress.Cancel();
        }
    }
}
