using Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services
{
    public class ChatGPTReleaseNotesService : IOpenAIService
    {
        private static string openAIURL = "https://api.openai.com/v1/chat/completions";
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient _client;
        private readonly AIModelProviderService _aIModelProviderService;
        private AIRequestModelProviderService _aiRequestModelProvider = new AIRequestModelProviderService();

        public ChatGPTReleaseNotesService(IHttpClientFactory clientFactory, AIModelProviderService aIModelProviderService)
        {
            _clientFactory = clientFactory;
            _aIModelProviderService = aIModelProviderService;
        }

        public async Task<string> ProcessUserPromtAsync(string tag1, string tag2, string input)
        {
            CreateHttpClient();
            var releaseNotesAiUser = _aiRequestModelProvider.ReleaseNotesAiUser(tag1, tag2, input);
            var requestBody = new
            {
                model = _aIModelProviderService.ChatGptAIModel.modelName,
                messages = new[]
                {
                    new { role = _aiRequestModelProvider.ReleaseNotesAiAssistant.Role,  content = _aiRequestModelProvider.ReleaseNotesAiAssistant.Content },
                    new { role = releaseNotesAiUser.Role,  content = releaseNotesAiUser.Content }
                }
            };

            var AIResponse = await _client.PostAsJsonAsync(openAIURL, requestBody);

            if (AIResponse.IsSuccessStatusCode)
            {
                var completionResponse = await AIResponse.Content.ReadFromJsonAsync<CompletionResponse>();
                return completionResponse.choices[0].message.content;
            }
            else
            {
                var errorMessage = await AIResponse.Content.ReadAsStringAsync();
                throw new OpenAIException($"Failed to complete chat: {errorMessage}");
            }
        }

        private void CreateHttpClient()
        {
            _client = _clientFactory.CreateClient();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_aIModelProviderService.ChatGptAIModel.openAIKey}");
        }
    }
}
