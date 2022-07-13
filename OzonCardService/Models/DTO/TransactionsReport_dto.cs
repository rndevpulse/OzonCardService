using CsvHelper.Configuration.Attributes;

namespace OzonCardService.Models.DTO
{
    public class TransactionsReport_dto
    {
        [Name("Дата")]
        public string Date { get; set; }
        [Name("Время")]
        public string Time { get; set; }


        [Name("Точка доступа")]
        public string PointControl { get { return "Столовая"; } }
        [Name("Направление")]
        public string Direction { get { return "Касса"; } }
        [Name("Событие")]
        public string Event { get { return "Оплата"; } }
        [Name("Таб. №")]
        public string TabNumber { get; set; }
        [Name("Имя")]
        public string Name { get; set; }
        [Ignore]
        public string СardNumbers { get; set; }

    }
    public class TransactionsSummaryReport_dto
    {
    }


}
