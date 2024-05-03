using OzonCard.Common.Application.Reports.Data;
using OzonCard.Common.Core;
using OzonCard.Common.Domain.Files;

namespace OzonCard.Common.Application.Reports.Commands;

public class ReportTransactionsCommand : ReportOption, ICommand<SaveFile>;