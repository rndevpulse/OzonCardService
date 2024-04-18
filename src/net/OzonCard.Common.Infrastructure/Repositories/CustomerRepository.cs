using OzonCard.Common.Application.Customers;
using OzonCard.Common.Domain.Customers;
using OzonCard.Common.Infrastructure.Database;
using OzonCard.Common.Infrastructure.Repositories.Abstractions;

namespace OzonCard.Common.Infrastructure.Repositories;

public class CustomerRepository(
    InfrastructureContext context
) : RepositoryBase<Customer>(context), ICustomerRepository
{
    
}