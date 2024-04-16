using Microsoft.AspNetCore.Http;

namespace OzonCard.Customer.Api.Services.FileManager;

public class FileManager : IFileManager
{
    static string path = Path.Combine(Directory.GetCurrentDirectory(), "FileReports");

    public FileManager()
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    public string GetDirectory() { return path; }
    public async Task<Guid> Save(IFormFile file)
    {
        var name = Guid.NewGuid();
        var format = file.FileName.Split(".").Last().Trim().ToLower();
        using (var fileStream = new FileStream(
                   Path.Combine(path, string.Concat(name.ToString(), ".", format).TrimEnd()),
                   FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return name;
    }

    public async Task<bool> RemoveFile(Guid id, string format)
    {
        try
        {
            await Task.Run(() => File.Delete(
                Path.Combine(path, string.Concat(id.ToString(), ".", format).TrimEnd())));

            return true;
        }
        catch (Exception)
        { return false; }
    }
    public async Task<bool> RemoveFile(string file)
    {
        try
        {
            await Task.Run(() => File.Delete(
                Path.Combine(path, file).TrimEnd()));
            return true;
        }
        catch (Exception)
        { return false; }
    }


    public string GetFile(string name)
    {
        var file = Path.Combine(path, name);
        if (File.Exists(file))
            return file;
        else throw new FileNotFoundException();
    }
}