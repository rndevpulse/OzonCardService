using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Application.Files.Commands;
using OzonCard.Common.Application.Files.Queries;
using OzonCard.Customer.Api.Models.Files;
using OzonCard.Customer.Api.Services.FileManager;
using OzonCard.Identity.Domain;

namespace OzonCard.Customer.Api.Controllers;

[Authorize(UserRole.Basic)]
public class FileController(IFileManager fileManager) : ApiController
{

    [HttpPost]
    public async Task<object> Create(IFormFile file, CancellationToken ct = default)
    {
        var id = await fileManager.Save(file);
        var document = await Commands.Send(new SaveFileCommand(
            id,
            file.FileName,
            UserClaimSid
        ), ct);
        return Mapper.Map<FileModel>(document);
    }


    [HttpDelete]
    public async Task Remove(string url, CancellationToken ct = default)
    {
        await Commands.Send(new RemoveFileCommand(url), ct);
        await fileManager.RemoveFile(url);
    }

    [HttpGet]
    public async Task<IEnumerable<FileModel>> Index(CancellationToken ct = default)
    {
        var documents = await Queries.Send(
            new GetFilesQuery(UserClaimSid),
            ct);
        return Mapper.Map<IEnumerable<FileModel>>(documents);
    }

    [HttpGet("{url}")]
    public ActionResult<IFormFile> Download(string url) =>
        PhysicalFile(
            fileManager.GetFile(url),
            "multipart/form-data");

}