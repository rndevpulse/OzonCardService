using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;
using OzonCardService.Services.Interfaces;
using System.Threading.Tasks;

namespace OzonCardService.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
		IIdentityRepository _repository;

		public IdentityService(IIdentityRepository repository) => _repository = repository;

		public async Task<User> GetUser(string userName)
		{
			return await _repository.GetUser(userName);
		}
	}
}
