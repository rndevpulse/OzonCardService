using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OzonCardService.Services.TasksManagerProgress.Implementation
{
    public class ProgressTask<T> : IProgress<T> where T : class, IProgressInfo<IInfoData>
    {
        private volatile T _progressInfo;
        private Task _task { get; set; }
        private readonly Stopwatch _sw;

        public T Vallue
        {
            get { return _progressInfo; }
        }

        public ProgressTask()
        {
            _sw = new Stopwatch();
            _sw.Start();
        }
        public void SetTask(Task task)
        {
            _task = task;
            _task.ContinueWith(t =>
            {
                _progressInfo.isCompleted(true);
                Report(_progressInfo);
            });
        }

        public void Report(T value)
        {
            if (value.Status.isCompleted == true)
            {
                _sw.Stop();
            }
            value.TimeCompleted(_sw.Elapsed);
            _progressInfo = value;
        }
    }
}
