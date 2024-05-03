using OzonCardService.Services.Quartzs.Workers;
using Quartz;
using System.Threading.Tasks;

namespace OzonCardService.Services.Quartzs.Jobs
{
    public class ServiceEventJob : IJob
    {
        private readonly IServiceEvent _service;

        public ServiceEventJob(IServiceEvent service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.UpdateEvents();
        }

    }
}
