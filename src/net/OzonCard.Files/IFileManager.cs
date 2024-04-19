

namespace OzonCard.Files;

public interface IFileManager
{
    Task<Guid> Save(Stream stream, string name);
    Task<bool> RemoveFile(Guid id, string format);
    Task<bool> RemoveFile(string file);
    string GetFile(string name);
}