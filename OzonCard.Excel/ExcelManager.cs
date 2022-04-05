using ExcelDataReader;
using ExcelLibrary.SpreadSheet;
using Serilog;
using System.Data;

namespace OzonCard.Excel
{
    public class ExcelManager
    {
        private readonly ILogger log = Log.ForContext(typeof(ExcelManager));


        string originalFileName = string.Empty;
        const int Columns = 3;
        int offset;
        public ExcelManager(string file)
        {
            originalFileName = file;
            offset = 0;
        }


        public IEnumerable<ShortCustomerInfo_excel> GetClients()
        {
            try
            {
                var clientList = new List<ShortCustomerInfo_excel>();
                log.Debug($"Открытие файла {originalFileName} для получения гостей");
                using (var stream = File.Open(originalFileName, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

                    var conf = new ExcelDataSetConfiguration
                    {
                        UseColumnDataType = false,
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false

                        }
                    };

                    var dataSet = reader.AsDataSet(conf).Tables[0];
                    switch (dataSet.Columns.Count)
                    {
                        case Columns:
                            break;
                        case Columns + 1:
                            offset = 1;
                            break;
                        case Columns + 2:
                            offset = 1;
                            break;
                        default:
                            var ex = $"Колличество столбцов ({dataSet.Columns.Count}) не соответствует заданному для парсинга количеству ({Columns})";
                            log.Error(ex);
                            throw new ArgumentOutOfRangeException(ex);
                    }

                    //var count = dataSet.Rows.Count;
                    foreach (DataRow row in dataSet.Rows)
                    {
                        string card = string.Empty;
                        if (row[offset + 2].ToString().Contains("/"))
                        {
                            string[] num = row[offset + 2].ToString().Replace(" ", "").Split('/');
                            num[0] = "000".Remove(0, num[0].Count()) + num[0];
                            num[1] = "00000".Remove(0, num[1].Count()) + num[1];
                            card = num[0] + num[1];
                        }
                        else
                        {
                            try
                            {
                                if (row[offset + 2].ToString().Trim() == "")
                                    throw new Exception();
                                card = "00000000".Remove(0, row[offset + 2].ToString().Count()) + row[offset + 2].ToString();
                            }
                            catch (Exception)
                            {
                                card = "";
                            }

                        }
                        if (card == "" || clientList
                            .Select(data => data.Card)
                            .Contains(card))
                            continue;
                        if (offset == 1)
                            clientList.Add(new ShortCustomerInfo_excel()
                            {
                                Name = row[offset].ToString().Replace("  ", " ").Trim(),
                                Card = card,
                                TabNumber = row[0].ToString(),
                                Position = row[dataSet.Columns.Count - 1].ToString()
                            });
                        else
                            clientList.Add(new ShortCustomerInfo_excel()
                            {
                                Name = row[0].ToString(),
                                Card = card
                            });
                    }

                    log.Information($"Обработка завершена, строк в документе обработано: {dataSet.Rows.Count}");
                    reader.Close();
                    //clientList.RemoveAt(0);

                }

                log.Information($"Загрузка гостей завершена, гостей для дальнейшей обработки: {clientList.Count}");
                return clientList.ToArray();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public void CreateReportWithTabNumber(IEnumerable<ShortCustomerInfo_excel> Customers, string saveFileName)
        {
            log.Information($"Открытие файла {originalFileName} для получения отчета");
            using (var stream = File.Open(originalFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

                var conf = new ExcelDataSetConfiguration
                {
                    UseColumnDataType = true,
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true

                    }
                };

                var dataSet = reader.AsDataSet(conf).Tables[0];
                foreach (DataRow row in dataSet.Rows)
                {
                    var s = Customers
                        .Where(data => data.Name.Contains(row[0].ToString().Replace("  ", " ").Trim()))
                        .ToArray()
                        .Select(data => data.TabNumber).First();
                    if (row[3].ToString().Length > 0)
                        row[row.ItemArray.Count() - 1] = s;
                }
                CreateWorkbook(saveFileName, dataSet.DataSet);
                log.Information($"Обработка завершена, результат сохранен в {saveFileName}");
                reader.Close();
            }

        }


        //////// экспорт в эксель


        //export Excel from DataSet
        public static void CreateWorkbook<T>(String filePath, IList<T> list, string title = null)
        {
            var dataset = ToDataSet<T>(list);
            CreateWorkbook(filePath, dataset, title);   
        }

        public static void CreateWorkbook(String filePath, DataSet dataset, string title = null)
        {
            if (dataset.Tables.Count == 0)
                throw new ArgumentException("DataSet needs to have at least one DataTable", "dataset");

            Workbook workbook = new Workbook();
            foreach (DataTable dt in dataset.Tables)
            {
                Worksheet worksheet = new Worksheet(dt.TableName);
                if (title != null)
                {
                    worksheet.Cells[0, 0] = new Cell(title);
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    // Add column header
                    //worksheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);

                    // Populate row data
                    for (int j = 0; j < dt.Rows.Count; j++)
                        //Если нулевые значения, заменяем на пустые строки
                        worksheet.Cells[j + 1, i] = new Cell(dt.Rows[j][i] == DBNull.Value ? "" : dt.Rows[j][i]);
                }
                workbook.Worksheets.Add(worksheet);
            }
            workbook.Save(filePath);
        }

        static DataSet ToDataSet<T>(IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            DataRow row = t.NewRow();
            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                //Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, typeof(object));//ColType);
                row[propInfo.Name] = ((CsvHelper.Configuration.Attributes.NameAttribute)propInfo.GetCustomAttributes(true).First())
                    .Names
                    .FirstOrDefault()
                    .ToString();

            }
            t.Rows.Add(row);

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }
            var n = 13;
            if (list.Count < n)
                for (var i = 0; i < n; i++)
                    t.Rows.Add(t.NewRow());
            return ds;
        }


        //********************************
        //DataGridView to DataTable
        //public static DataTable ToDataTable(DataTable table, string tableName)
        //{

        //    DataGridView dgv = dataGridView;
        //    DataTable table = new DataTable(tableName);
        //    int iCol = 0;

        //    for (iCol = 0; iCol & lt; dgv.Columns.Count; iCol++)
        //    {
        //        table.Columns.Add(dgv.Columns[iCol].Name);
        //    }

        //    foreach (DataGridViewRow row in dgv.Rows)
        //    {

        //        DataRow datarw = table.NewRow();

        //        for (iCol = 0; iCol & lt; dgv.Columns.Count; iCol++)
        //        {
        //            datarw[iCol] = row.Cells[iCol].Value;
        //        }

        //        table.Rows.Add(datarw);
        //    }

        //    return table;
        //}
    }
}
