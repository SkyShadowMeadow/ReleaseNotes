using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ReleaseNotesController : ControllerBase
{
    private readonly ReleaseNotesService _releaseNotesService;

    public ReleaseNotesController(ReleaseNotesService releaseNotesService)
    {
        _releaseNotesService = releaseNotesService;
    }


    [HttpGet(Name = "CreateReleaseNotes")]
    public async Task<IActionResult> CreateReleaseNotes(string repoUrl,  string newVersionTag = null, string previousVersionTag = null)
    {
        try
        {
            var releaseNotes = await _releaseNotesService.CreateReleaseNotes(repoUrl, newVersionTag, previousVersionTag);
            return Ok(releaseNotes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}


