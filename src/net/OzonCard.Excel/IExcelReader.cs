using OzonCard.Excel.Models;

namespace OzonCard.Excel;

public interface IExcelReader
{
    IEnumerable<Customer> GetCustomers(string file);
    void CreateWorkbook<T>(string filePath, IList<T> list, string? title = null, bool totalsRow = true);
}