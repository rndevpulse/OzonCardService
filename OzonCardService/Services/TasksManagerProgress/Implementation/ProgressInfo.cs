using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System;

namespace OzonCardService.Services.TasksManagerProgress.Implementation
{
    public class ProgressInfo : IProgressInfo
    {
        public IInfoData InfoData { get; private set; }

        public ProgressInfo(IInfoData info)
        {
            InfoData = info;
        }

        public IInfoData Status
        {
            get { return InfoData; }
        }

        public void isCompleted(bool complete)
        {
            InfoData.isCompleted = complete;
        }
        public void isCancel(bool cancel)
        {
            InfoData.isCancel = cancel;
        }

        public void TimeCompleted(TimeSpan timeComplete)
        {
            InfoData.TimeCompleted = timeComplete;
        }
    }
}
