using Microsoft.AspNetCore.Http;

namespace OzonCard.Customer.Api.Services.FileManager;

public interface IFileManager
{
    Task<Guid> Save(IFormFile file);
    Task<bool> RemoveFile(Guid id, string format);
    Task<bool> RemoveFile(string file);
    string GetFile(string name);
}