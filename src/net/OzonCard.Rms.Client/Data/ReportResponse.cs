namespace OzonCard.Rms.Client.Data;

public class ReportResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    
}