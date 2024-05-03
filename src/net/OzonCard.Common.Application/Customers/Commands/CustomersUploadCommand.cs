using OzonCard.Common.Application.Customers.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Application.Customers.Commands;

public class CustomersUploadCommand : CustomersUpload, ICommand<IEnumerable<Customer>>;
