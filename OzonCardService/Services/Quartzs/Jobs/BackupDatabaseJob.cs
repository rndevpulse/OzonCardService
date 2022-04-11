using OzonCardService.Services.Quartzs.Workers;
using Quartz;
using System.IO;
using System.Threading.Tasks;

namespace OzonCardService.Services.Quartzs.Jobs
{
    public class BackupDatabaseJob : IJob
    {
        private readonly IServiceDatabase _serviceDatabase;

        public BackupDatabaseJob(IServiceDatabase serviceDatabase)
        {
            _serviceDatabase = serviceDatabase;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var path = Directory.GetCurrentDirectory();
            ;
            await _serviceDatabase.CreateBackup(Path.Combine(path, "Backup"), 6);
            await _serviceDatabase.RemoveOldFile(Path.Combine(path, "FileReports"), 7);
        }

    }
}
