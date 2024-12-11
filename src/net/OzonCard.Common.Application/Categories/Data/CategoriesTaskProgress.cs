using OzonCard.Common.Worker.Data;

namespace OzonCard.Common.Application.Categories.Data;

public record CategoriesTaskProgress : NamedProgress
{
    public string Log { get; set; } = "";
    public int All { get; set; } = 0;
    public int Processed { get; set; } = 0;
  
    public void AddLog(string log) => Log += $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] " + log + "\n";
}