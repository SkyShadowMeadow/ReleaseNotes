using Microsoft.AspNetCore.Mvc;
using Services;
using Newtonsoft.Json;
using Models;
using System.Diagnostics;

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

            var tagsUrl = $"https://api.github.com/repos/{owner}/{repoName}/tags";
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ReleaseNotesApp");
            var response = await client.GetAsync(tagsUrl);

            if (response.IsSuccessStatusCode)
            {
                var tagsJson = await response.Content.ReadAsStringAsync();
                var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson);

                if (tags.Count < 2)
                {
                    return BadRequest("The repository does not have at least two tags.");
                }

                var lastTag = tags[0].Name;
                var secondLastTag = tags[1].Name;

                var commitsUrl = $"https://api.github.com/repos/{owner}/{repoName}/compare/{secondLastTag}...{lastTag}";
                response = await client.GetAsync(commitsUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var commitMessages = _gitServices.GetCommitMessagesAsync(responseBody);
                    return Ok(commitMessages);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Failed to fetch commits from GitHub API");
                }
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
