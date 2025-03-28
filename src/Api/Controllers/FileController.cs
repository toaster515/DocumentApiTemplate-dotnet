using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using System.Text.Json;
using Application.Commands;
using Api.Models;
using Application.Queries;
using System.Text.Json.Serialization;

namespace Api.Controllers;

[ApiController]
[Route("api/files")]
public class FileController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CreateFileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CreateFileDto>> Upload([FromForm] UploadFileForm form)
    {
        var file = form.File;
        var metadata = form.MetadataJson;
        Console.WriteLine($"File: {file?.FileName}, Metadata: {metadata}");

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        CreateFileRequest? request;
        try
        {
            request = JsonSerializer.Deserialize<CreateFileRequest>(metadata, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            });
        }
        catch (JsonException)
        {
            return BadRequest("Invalid metadata format.");
        }

        if (request == null)
            return BadRequest("Metadata is required.");

        using var stream = file.OpenReadStream();

        var cmd = new UploadFileCommand
        {
            FileStream = stream,
            FileName = request.FileName,
            ContentType = file.ContentType,
            Provider = request.Provider,
            UploadedBy = request.UploadedBy
        };

        var result = await mediator.Send(cmd);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Download(Guid id)
    {
        var result = await mediator.Send(new GetFileQuery { Id = id });

        var (stream, contentType, fileName) = result;

        return File(stream, contentType, fileName);
    }

}