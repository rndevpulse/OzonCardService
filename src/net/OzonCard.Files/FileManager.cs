namespace OzonCard.Files;

public class FileManager : IFileManager
{
    private static readonly string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "FileReports");

    public FileManager()
    {
        if (!Directory.Exists(Path))
            Directory.CreateDirectory(Path);
    }
    public string GetDirectory() { return Path; }
    public async Task<Guid> Save(Stream stream, string name)
    {
        var id = Guid.NewGuid();
        var format = name.Split(".").Last().Trim().ToLower();
        await using var fs = new FileStream(
            System.IO.Path.Combine(Path, string.Concat(id, ".", format).TrimEnd()),
            FileMode.Create);
        await stream.CopyToAsync(fs);
        return id;
    }

    public async Task<bool> RemoveFile(Guid id, string format)
    {
        try
        {
            await Task.Run(() => File.Delete(
                System.IO.Path.Combine(Path, string.Concat(id.ToString(), ".", format).TrimEnd())));

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
                System.IO.Path.Combine(Path, file).TrimEnd()));
            return true;
        }
        catch (Exception)
        { return false; }
    }


    public string GetFile(string name)
    {
        var file = System.IO.Path.Combine(Path, name);
        if (File.Exists(file))
            return file;
        throw new FileNotFoundException();
    }
}