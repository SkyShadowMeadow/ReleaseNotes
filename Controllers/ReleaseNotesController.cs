using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotes.Controllers;

[ApiController]
[Route("[controller]")]
public class ReleaseNotesController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public ReleaseNotesController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    [HttpGet(Name = "CreateReleaseNotes")]
    public async Task<IActionResult> CreateReleaseNotes(string repoUrl)
    {
        try
        {
            string[] parts = repoUrl.Split('/');
            if (parts.Length < 4)
            {
                return BadRequest("Invalid repository URL");
            }
            string owner = parts[3];
            string repoName = parts[4];

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
