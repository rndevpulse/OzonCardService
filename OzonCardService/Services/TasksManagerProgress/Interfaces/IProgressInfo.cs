using System;

namespace OzonCardService.Services.TasksManagerProgress.Interfaces
{
    public interface IProgressInfo
    {
        IInfoData Status { get; }
        void isCompleted(bool complete);
        void TimeCompleted(TimeSpan timeComplete);
    }
}
