using Microsoft.Extensions.Configuration;

namespace OzonCard.Files;

public class FileManager : IFileManager
{
    private readonly string _path;

    public FileManager(IConfiguration configuration)
    {
        _path = System.IO.Path.Combine(
            configuration["content"] ?? "content",
            "fileReports"
        );
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
    }
    public string GetDirectory() { return _path; }
    public async Task<Guid> Save(Stream stream, string name)
    {
        var id = Guid.NewGuid();
        var format = name.Split(".").Last().Trim().ToLower();
        await using var fs = new FileStream(
            System.IO.Path.Combine(_path, string.Concat(id, ".", format).TrimEnd()),
            FileMode.Create);
        await stream.CopyToAsync(fs);
        return id;
    }

    public bool RemoveFile(Guid id, string format)
    {
        try
        {
            File.Delete(System.IO.Path.Combine(_path, string.Concat(id.ToString(), ".", format).TrimEnd()));
            return true;
        }
        catch (Exception)
        { return false; }
    }
    public bool RemoveFile(string file)
    {
        try
        {
           File.Delete(System.IO.Path.Combine(_path, file).TrimEnd());
           return true;
        }
        catch (Exception)
        { return false; }
    }


    public string GetFile(string name)
    {
        var file = System.IO.Path.Combine(_path, name);
        if (File.Exists(file))
            return file;
        throw new FileNotFoundException();
    }
}