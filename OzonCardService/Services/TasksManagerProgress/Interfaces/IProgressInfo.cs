using System;

namespace OzonCardService.Services.TasksManagerProgress.Interfaces
{
    public interface IProgressInfo<T> where T : class, IInfoData
    {
        T Status { get; }
        void isCompleted(bool complete);
        void TimeCompleted(TimeSpan timeComplete);
    }
}
