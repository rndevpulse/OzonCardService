using AutoMapper;
using OzonCard.Common.Application.Customers.Data;
using OzonCard.Customer.Api.Models.Customers;

namespace OzonCard.Customer.Api.Mappings;

public class CustomerMappings : Profile
{
    protected CustomerMappings()
    {
        CreateMap<CustomerSearch, CustomerModel>();
    }
}