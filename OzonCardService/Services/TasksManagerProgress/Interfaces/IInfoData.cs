using System;

namespace OzonCardService.Services.TasksManagerProgress.Interfaces
{
    public interface IInfoData
    {
        bool isCompleted { get; set; }
        TimeSpan TimeCompleted { get; set; }
    }

    public class InfoData : IInfoData
    {
        public bool isCompleted { get; set; }
        public TimeSpan TimeCompleted { get; set; }
    }
}
