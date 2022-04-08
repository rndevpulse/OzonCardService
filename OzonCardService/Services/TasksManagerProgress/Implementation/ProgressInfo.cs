using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System;

namespace OzonCardService.Services.TasksManagerProgress.Implementation
{
    public class ProgressInfo<T> : IProgressInfo<T> where T : class, IInfoData
    {
        public T InfoData { get; private set; }

        public ProgressInfo(T info)
        {
            InfoData = info;
        }

        public T Status
        {
            get { return InfoData; }
        }

        public void isCompleted(bool complete)
        {
            InfoData.isCompleted = complete;
        }

        public void TimeCompleted(TimeSpan timeComplete)
        {
            InfoData.TimeCompleted = timeComplete;
        }
    }
}
