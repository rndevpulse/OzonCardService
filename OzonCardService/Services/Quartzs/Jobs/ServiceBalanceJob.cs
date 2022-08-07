using OzonCardService.Services.Quartzs.Workers;
using Quartz;
using System.Threading.Tasks;

namespace OzonCardService.Services.Quartzs.Jobs
{
    public class ServiceBalanceJob : IJob
    {
        private readonly IServiceBalance _service;

        public ServiceBalanceJob(IServiceBalance service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.UpdateAllBalanceCustomers();
        }

    }
}
