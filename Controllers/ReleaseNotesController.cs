using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.Controllers;

[ApiController]
[Route("[controller]")]
public class ReleaseNotesController  : ControllerBase
{
    public ReleaseNotesController () 
    {}


    [HttpGet(Name = "CreateReleaseNotes")]
    public string CreateReleaseNotes()
    {
        return "test";
    }
}
