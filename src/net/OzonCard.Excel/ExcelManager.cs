﻿using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.Extensions.Logging;
using OzonCard.Excel.DataSets.Abstract;
using OzonCard.Excel.Models;

namespace OzonCard.Excel;



public class ExcelManager(ILogger<ExcelManager> logger) : IExcelManager
{

    private Customer? ReadRow(IXLRow row, int nameCol, int? tabNumberCol, int? positionCol, int cardCol)
    {
        var card = ReadCardCellValue(row.Cell(cardCol).Value.ToString());
        if (card == "")
            return null;

        return new Customer()
        {
            Name = (row.Cell(nameCol).Value.ToString() ?? "").Replace("  ", " ").Trim(),
            Card = card,
            TabNumber = tabNumberCol == null ? "" : row.Cell((int)tabNumberCol).Value.ToString() ?? "",
            Position = positionCol == null ? "" : row.Cell((int)positionCol).Value.ToString() ?? "",
            Division = ""
        };
    }

    private  Customer? ReadRow(DataRow row, int nameCol, int? tabNumberCol, int? positionCol, int cardCol)
    {
        var card = ReadCardCellValue(row[cardCol].ToString());
        if (card == "")
            return null;

        return new Customer()
        {
            Name = (row[nameCol].ToString() ?? "").Replace("  ", " ").Trim(),
            Card = card,
            TabNumber = tabNumberCol == null ? "" : row[(int)tabNumberCol].ToString() ?? "",
            Position = positionCol == null ? "" : row[(int)positionCol].ToString() ?? "",
            Division = ""
        };
    }

    string ReadCardCellValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        var regex = new Regex(@"\d+");
        var matches = regex.Matches(value);
        string card;
        if (matches.Count > 1)
        {
            card = "000".Remove(0, matches[0].Length) + matches[0];
            card += "00000".Remove(0, matches[1].Length) + matches[1];
            return card;
        }

        card = string.Concat(matches.Select(x => x.Value));
        if (card == "" || card.Length > 8)
            return card;
        card = "00000000".Remove(0, card.Length) + card;
        return card;
    }

    void SelDataColumns(int useColumns, int offset, out int nameCol, out int? tabNumberCol, out int? positionCol,
        out int cardCol)
    {
        nameCol = offset;
        tabNumberCol = null;
        positionCol = null;
        cardCol = 2 + offset;
        switch (useColumns)
        {
            case 3:
                break;
            case 5:
                tabNumberCol = offset;
                nameCol = 1 + offset;
                cardCol = 3 + offset;
                positionCol = 4 + offset;
                break;
            case 9:
                positionCol = 1 + offset;
                nameCol = 2 + offset;
                tabNumberCol = 3 + offset;
                cardCol = 7 + offset;
                break;
            default:
                var ex =
                    $"Колличество столбцов ({useColumns}) не соответствует заданному для парсинга количеству (3:5:9)";
                Console.WriteLine("Error {0}", ex);
                throw new ArgumentOutOfRangeException(ex);
        }
    }

    /// <summary>
    /// Read as ExcelDataReader
    /// </summary>
    /// <returns></returns>
    IEnumerable<Customer> ReadFromXls(string file)
    {
        var clientList = new List<Customer>();
        using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
        {
            var reader = ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration
            {
                UseColumnDataType = false,
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = false
                }
            };
            var dataSet = reader.AsDataSet(conf).Tables[0];
            //нумерация колонок от 0 (A = 0)
            //установка зависимостей используемых столбцов
            SelDataColumns(dataSet.Columns.Count, 0,
                out var nameCol, out var tabNumberCol, out var positionCol, out var cardCol);
            foreach (DataRow row in dataSet.Rows)
            {
                var customer = ReadRow(row, nameCol, tabNumberCol, positionCol, cardCol);
                if (customer == null || clientList.Contains(customer))
                    continue;
                clientList.Add(customer);
            }
        }

        return clientList.ToArray();
    }

    /// <summary>
    /// Read as ClosedXML
    /// </summary>
    /// <returns></returns>
    IEnumerable<Customer> ReadFromXlsx(string file)
    {
        var clientList = new List<Customer>();
        using (var workbook = new XLWorkbook(file))
        {
            var dataSet = workbook.Worksheet(1);
            if (dataSet == null)
                return clientList;
            //нумерация колонок от 1 (A = 1)
            //установка зависимостей используемых столбцов
            SelDataColumns(dataSet.ColumnsUsed().Count(), 1,
                out var nameCol, out var tabNumberCol, out var positionCol, out var cardCol);
            foreach (var row in dataSet.RowsUsed())
            {
                var customer = ReadRow(row, nameCol, tabNumberCol, positionCol, cardCol);
                if (customer == null || clientList.Contains(customer))
                    continue;
                clientList.Add(customer);
            }
        }
        return clientList.ToArray();
    }

    public IEnumerable<Customer> GetCustomers(string file)
    {
        try
        {
            logger.LogInformation("Read file {file}", file);
            return ReadFromXls(file).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail read document '{file}'", file);
            throw new Exception(ex.Message);
        }
    }


    public void CreateWorkbook(string filePath, BaseDataSet baseDataSet, string? title = null)
    {
        try
        {
            var dataset = baseDataSet.GetDataSet();
            if (dataset.Tables.Count == 0)
                throw new ArgumentException("DataSet needs to have at least one DataTable", nameof(dataset));
            var workbook = new XLWorkbook();
            foreach (DataTable dt in dataset.Tables)
            {
                var worksheet = workbook.Worksheets.Add(dt.TableName);

                if (title != null)
                {
                    worksheet.Cell(1, 1).Value = title;
                    worksheet.Range(1, 1, 1, 5).Merge();
                }

                var range = worksheet.Range(2, 1, dt.Rows.Count + 1, dt.Columns.Count);
                var table = range.CreateTable();
                table.Theme = XLTableTheme.TableStyleLight8;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    // Add column header
                    //worksheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);
                    for (int j = 0; j < dt.Rows.Count; j++)
                        //Если нулевые значения, заменяем на пустые строки
                        worksheet.Cell(j + 2, i + 1).Value = dt.Rows[j][i] == DBNull.Value ? "" : dt.Rows[j][i];
                }

                foreach (var totalRow in baseDataSet.TotalsRowByTable(dt.TableName))
                {
                    if (!table.ShowTotalsRow)
                        table.ShowTotalsRow = true;
                    if (!string.IsNullOrEmpty(totalRow.Label))
                    {
                        table.Field(totalRow.Field).TotalsRowLabel = totalRow.Label;
                        continue;
                    }
                    table.Field(totalRow.Field).TotalsRowFunction = totalRow.Function;
                }
                worksheet.Columns().AdjustToContents();
            }
            logger.LogInformation("Save file report to {0}", filePath);
            workbook.SaveAs(filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on save report");
            throw new Exception(" Ошибка при сохранении файла отчета");
        }
    }

}

