using OzonCardService.Models.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OzonCardService.Helpers
{

    public interface IProgressInfo
    {
        InfoDataUpload_dto Status { get; }
        void isCompleted(bool complete);
        void TimeCompleted(TimeSpan timeComplete);
    }

    public class ProgressInfo : IProgressInfo
    {
        public InfoDataUpload_dto InfoData { get; private set; }

        public ProgressInfo(InfoDataUpload_dto info)
        {
            InfoData = info;
        }
        

        public InfoDataUpload_dto Status
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
    public class Progress<T> : IProgress<T> where T : class, IProgressInfo
    {
        private volatile T _progressInfo;
        private Task _task { get; set; }
        private readonly Stopwatch _sw;

        public T Vallue
        {
            get { return _progressInfo; }
        }

        public Progress()
        {
            _sw = new Stopwatch();
            _sw.Start();
        }
        public void SetTask(Task task)
        {
            _task = task;
            _task.ContinueWith(t=> 
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

    public interface ITasksManagerProgress
    {
        Guid AddTask(Progress<ProgressInfo> progress);
        InfoDataUpload_dto GetStatusTask(Guid taskId);
    }
    public class TasksManagerProgress : ITasksManagerProgress
    {
        static Dictionary<Guid, Progress<ProgressInfo>> DictionaryTasks = new Dictionary<Guid, Progress<ProgressInfo>>();

        public Guid AddTask(Progress<ProgressInfo> progress)
        {
            var guid = Guid.NewGuid();
            DictionaryTasks.Add(guid, progress);
            return guid;
        }

        public InfoDataUpload_dto GetStatusTask(Guid taskId)
        {
            Progress<ProgressInfo> progress;
            DictionaryTasks.TryGetValue(taskId, out progress);
            var status = progress?.Vallue.Status ?? null;
            if (status != null && status.isCompleted)
            {
                DictionaryTasks.Remove(taskId);
            }
            return status ?? new InfoDataUpload_dto();

        }
    }
}
