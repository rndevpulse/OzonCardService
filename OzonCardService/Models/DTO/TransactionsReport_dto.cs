using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OzonCardService.Models.DTO
{
    public class TransactionsReport_dto
    {
        [Name("Дата")]
        public string Date { get; set; }
        [Name("Время")]
        public string Time { get; set; }
        [Name("Таб. №")]
        public string TabNumber { get; set; }
        [Name("ФИО")]
        public string Name { get; set; }
        [Name("Подразделение")]
        public string Division { get; set; }
        [Name("Категории")]
        public string Categories { get; set; }
        [Name("Прием пищи")]
        public string Eating { get; set; }

        [Ignore]
        public string СardNumbers { get; set; }

    }
    public class TransactionsSummaryReport_dto
    {
        [Name("ФИО")]
        public string Name { get; set; }
        [Name("Количество дней питания")]
        public double CountDay { get; set; }
        [Name("Категории")]
        public string Categories { get; set; }
        [Name("Подразделение")]
        public string Division { get; set; }
    }
    public class TransactionsReport
    {
        public IEnumerable<TransactionsReport_dto> Transactions { get; set; }
        public IEnumerable<TransactionsSummaryReport_dto> TransactionsSummary { get; set; }
        public TransactionsReport()
        {
            Transactions = new List<TransactionsReport_dto>();
            TransactionsSummary = new List<TransactionsSummaryReport_dto>();
        }
        public DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ToDataTable(Transactions.ToList(), "Отчет"));
            ds.Tables.Add(ToDataTable(TransactionsSummary.ToList(), "Сводный"));
            return ds;
        }
        static DataTable ToDataTable<T>(IList<T> list, string nameTable)
        {
            var elementType = typeof(T).GetProperties().Select(x => new KeyValuePair<PropertyInfo, string?>(
                x,
                (x.GetCustomAttributes(true)?.FirstOrDefault() as CsvHelper.Configuration.Attributes.NameAttribute)?.Names?.First()
                )).ToList();
            elementType.RemoveAll(x => x.Value == null);
            DataTable t = new DataTable(nameTable);

            DataRow row = t.NewRow();
            //add a column to table for each public property on T
            foreach (var propInfo in elementType)
            {
                //Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                t.Columns.Add(propInfo.Key.Name, typeof(object));//ColType);
                row[propInfo.Key.Name] = propInfo.Value;
            }
            t.Rows.Add(row);

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                row = t.NewRow();
                foreach (var propInfo in elementType)
                    row[propInfo.Key.Name] = propInfo.Key.GetValue(item, null) ?? DBNull.Value;

                t.Rows.Add(row);
            }
            
            return t;
        }
    }

}
