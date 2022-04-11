using OzonCardService.Services.TasksManagerProgress.Implementation;
using System;

namespace OzonCardService.Services.TasksManagerProgress.Interfaces
{
    public interface ITasksManagerProgress
    {
        Guid AddTask(ProgressTask<ProgressInfo> progress);
        IInfoData GetStatusTask(Guid taskId);
    }
}
