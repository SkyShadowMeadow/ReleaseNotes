
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Services;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ReleaseNotesController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IGitServices _gitServices;
    private readonly AIModelProviderService _aIModelProviderService;
    AIRequestModelProviderService aiRequestModelProvider = new AIRequestModelProviderService();

    public ReleaseNotesController(IHttpClientFactory clientFactory, IGitServices gitServices, AIModelProviderService aIModelProviderService)
    {
        _clientFactory = clientFactory;
        _gitServices = gitServices;
        _aIModelProviderService = aIModelProviderService;
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
            var gitRresponse = await client.GetAsync(tagsUrl);
            string commitMessagesAsString = "";

            if (gitRresponse.IsSuccessStatusCode)
            {
                var tagsJson = await gitRresponse.Content.ReadAsStringAsync();
                var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson);

                if (tags.Count < 2)
                {
                    return BadRequest("The repository does not have at least two tags.");
                }

                var lastTag = tags[0].Name;
                var secondLastTag = tags[1].Name;

                var commitsUrl = $"https://api.github.com/repos/{owner}/{repoName}/compare/{secondLastTag}...{lastTag}";
                gitRresponse = await client.GetAsync(commitsUrl);

                if (gitRresponse.IsSuccessStatusCode)
                {
                    string responseBody = await gitRresponse.Content.ReadAsStringAsync();
                    GitHubApiResponse commitMessages = JsonConvert.DeserializeObject<GitHubApiResponse>(responseBody);
                    commitMessagesAsString = commitMessages.GetConcatenatedResult();
                }
                else
                {
                    return BadRequest("Failed to retrieve commits from GitHub.");
                }

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_aIModelProviderService.ChatGptAIModel.openAIKey}");
                var releaseNotesAiUser = aiRequestModelProvider.ReleaseNotesAiUser("v1.0", "v2.0", commitMessagesAsString);
                var requestBody = new
                {
                    model = _aIModelProviderService.ChatGptAIModel.modelName,
                    messages = new[]
                    {
                    new { role = aiRequestModelProvider.ReleaseNotesAiAssistant.Role,  content = aiRequestModelProvider.ReleaseNotesAiAssistant.Content },
                    new { role = releaseNotesAiUser.Role,  content = releaseNotesAiUser.Content }
                }
                };

                var AIResponse = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

                if (AIResponse.IsSuccessStatusCode)
                {
                    var completionResponse = await AIResponse.Content.ReadFromJsonAsync<CompletionResponse>();
                    return Ok(completionResponse);
                }
                else
                {
                    var errorMessage = await AIResponse.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to complete chat: {errorMessage}");
                }
            }
            else
            {
                return BadRequest("Failed to retrieve tags from GitHub.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    public class CompletionResponse
    {
        public Choice[] choices { get; set; }
    }

    public class Choice
    {
        public ChatMessage message { get; set; }
    }

    public class ChatMessage
    {
        public string content { get; set; }
        public string role { get; set; }
    }
}


