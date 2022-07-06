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
        private System.Threading.CancellationTokenSource _cancellationToken { get; set; }
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


        public void SetTask(Task task, System.Threading.CancellationTokenSource cancellationToken)
        {
            _task = task;
            _cancellationToken = cancellationToken;
            _task.ContinueWith(t =>
            {
                _progressInfo.isCompleted(true);
                Report(_progressInfo);
            });
            
        }
        public void Cancel()
        {
            _cancellationToken.Cancel();
            _progressInfo.isCancel(true);
        }
        public void Report(T value)
        {
            if (value.Status.isCompleted == true || value.Status.isCancel == true)
            {
                _sw.Stop();
                _timer.Stop();
                _timer.Dispose();
                _cancellationToken.Dispose();
            }
            value.TimeCompleted(_sw.Elapsed);
            _progressInfo = value;
        }
    }
}
