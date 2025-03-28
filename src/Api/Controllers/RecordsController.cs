using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.Models;
using Application.Models;
using Microsoft.AspNetCore.Http;
using System;
using Application.Queries;

[ApiController]
[Route("api/records")]
public class RecordsController(IMediator mediator) : ControllerBase
{

    [HttpPost("test")]
    public async Task<IActionResult> CreateTestFileRecord()
    {
        var cmd = new CreateFileRecordCommand
        {
            FileName = "test.txt",
            ObjectKey = "test.txt",
            Url = "s3://bucket/test.txt",
            Provider = StorageProvider.Aws
        };

        var id = await mediator.Send(cmd);
        return Ok(new { id });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FileRecordDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecord(Guid id)
    {
        var record = await mediator.Send(new GetRecordQuery(id));

        if (record == null)
            return NotFound();

        return Ok(record);
    }
}
