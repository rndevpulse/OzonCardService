namespace OzonCard.Tools.Categories;

public interface IAddon
{
    IEnumerable<IAddon> Load(string line);
} 


public class Reader
{
    public static IEnumerable<T> Load<T>(string fileName) where T : IAddon, new()
    {
        var query = LineReader(fileName)
            .Where(l=>l.Length>0)
            .SelectMany(CreateAddon<T>);
        return query.Cast<T>().ToArray();
    }

    private static IEnumerable<string> LineReader(string fileName)
    {
        using var file = File.OpenText(fileName);
        while (file.ReadLine() is { } line)
            yield return line.Trim();
    }
    private static IEnumerable<IAddon> CreateAddon<T>(string line) where T : IAddon, new()
    {
        var addon = new T();
        return addon.Load(line);
    }
    
}