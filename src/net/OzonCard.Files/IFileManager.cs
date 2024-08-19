

namespace OzonCard.Files;

public interface IFileManager
{
    Task<Guid> Save(Stream stream, string name);
    bool RemoveFile(Guid id, string format);
    bool RemoveFile(string file);
    string GetFile(string name);
    string GetDirectory();
    string GetTempDirectory();
    Task<Guid> SaveAsBath(string folder);
}