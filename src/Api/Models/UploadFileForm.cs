
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Models;
public class UploadFileForm
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; }

    [FromForm(Name = "metadata")]
    public string MetadataJson { get; set; }
}
