using System;

namespace OzonCardService.Services.TasksManagerProgress.Interfaces
{
    public interface IInfoData
    {
        bool isCompleted { get; set; }
        TimeSpan TimeCompleted { get; set; }
    }
}
