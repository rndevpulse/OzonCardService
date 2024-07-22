namespace OzonCard.Common.Worker.Data;

public interface IJobProgress
{
    string TaskId { get; set; }
    Guid Reference { get; set; }
    string Path { get; set; }
}