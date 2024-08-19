using System.IO.Compression;
using Microsoft.Extensions.Configuration;

namespace OzonCard.Files;

public class FileManager : IFileManager
{
    private readonly string _path;
    private readonly string _temp;

    public FileManager(IConfiguration configuration)
    {
        _path = Path.Combine(
            configuration["content"] ?? "content",
            "fileReports"
        );
        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
        
        _temp = Path.Combine(
            configuration["content"] ?? "content",
            "temp"
        );
        if (!Directory.Exists(_temp))
            Directory.CreateDirectory(_temp);
    }
    public string GetDirectory() { return _path; }
    public string GetTempDirectory() { return _temp; }
    public Task<Guid> SaveAsBatch(string folder)
    {
        var id = Guid.NewGuid();
        var dirInfo = new DirectoryInfo(folder);
        if (!dirInfo.Exists)
            throw new Exception("Directory not exist");
        
        ZipFile.CreateFromDirectory(folder, Path.Combine(_path, $"{id}.zip"));
        dirInfo.Delete(true);
        
        return Task.FromResult(id);
    }


    public async Task<Guid> Save(Stream stream, string name)
    {
        var id = Guid.NewGuid();
        var format = name.Split(".").Last().Trim().ToLower();
        await using var fs = new FileStream(
            Path.Combine(_path, string.Concat(id, ".", format).TrimEnd()),
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