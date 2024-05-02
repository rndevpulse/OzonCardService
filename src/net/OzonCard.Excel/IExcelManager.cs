using OzonCard.Excel.DataSets.Abstract;
using OzonCard.Excel.Models;

namespace OzonCard.Excel;

public interface IExcelManager
{
    IEnumerable<Customer> GetCustomers(string file);
    
    void CreateWorkbook(string filePath, BaseDataSet baseDataSet, string? title = null);
}