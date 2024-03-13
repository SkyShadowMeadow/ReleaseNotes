using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ReleaseNotesController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IGitServices _gitServices;

    public ReleaseNotesController(IHttpClientFactory clientFactory, IGitServices gitServices)
    {
        _clientFactory = clientFactory;
        _gitServices = gitServices;
    }


    [HttpGet(Name = "CreateReleaseNotes")]
    public async Task<IActionResult> CreateReleaseNotes(string repoUrl)
    {
        try
        {
            (string owner, string repoName) = _gitServices.ParseRepositoryUrl(repoUrl);
            var apiUrl = $"https://api.github.com/repos/{owner}/{repoName}/tags";

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ReleaseNotesApp");
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch tags from GitHub API");
            }
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"An error occurred while fetching data from GitHub API: {ex.Message}");
        }
    }
}
