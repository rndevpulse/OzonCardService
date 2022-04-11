using OzonCardService.Services.TasksManagerProgress.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace OzonCardService.Services.TasksManagerProgress.Implementation
{
    public class ProgressTask<T> : IProgress<T> where T : class, IProgressInfo
    {
        private volatile T _progressInfo;
        private Task _task { get; set; }
        private readonly Stopwatch _sw;
        private readonly Timer _timer;
        public T Vallue
        {
            get { return _progressInfo; }
        }

        public ProgressTask()
        {
            _sw = new Stopwatch();
            _sw.Start();
            _timer = new Timer(5000);
            _timer.Enabled = true;
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _progressInfo?.isCompleted(false);
            _progressInfo?.TimeCompleted(_sw.Elapsed);
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
                _timer.Stop();
                _timer.Dispose();
            }
            value.TimeCompleted(_sw.Elapsed);
            _progressInfo = value;
        }
    }
}
