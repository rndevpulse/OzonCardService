using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OzonCard.Common.Application.Files.Commands;
using OzonCard.Common.Application.Files.Queries;
using OzonCard.Customer.Api.Models.Files;
using OzonCard.Files;

namespace OzonCard.Customer.Api.Controllers;

public class FileController(IFileManager fileManager) : ApiController
{

    [HttpPost]
    public async Task<FileModel> Create(IFormFile file, CancellationToken ct = default)
    {
        await using var rs = file.OpenReadStream();
        var id = await fileManager.Save(rs, file.FileName);
        var document = await Commands.Send(new SaveFileCommand(
            id,
            file.FileName,
            UserClaimSid
        ), ct);
        return Mapper.Map<FileModel>(document);
    }


    [HttpPost("[action]/{url}")]
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