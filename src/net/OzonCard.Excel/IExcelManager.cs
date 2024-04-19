using System.Data;
using OzonCard.Excel.Models;

namespace OzonCard.Excel;

public interface IExcelManager
{
    IEnumerable<Customer> GetCustomers(string file);
    
    void CreateWorkbook(string filePath, DataSet dataset, string? title = null, bool totalsRow = true);
}