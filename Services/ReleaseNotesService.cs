using Services.Interfaces;

namespace Services
{
    public class ReleaseNotesService
    {
        private IGitService _gitHubService;
        private IOpenAIService _openAIService;

        public ReleaseNotesService(IGitService gitServices, IOpenAIService openAIService)
        {
            _gitHubService = gitServices;
            _openAIService = openAIService;
        }

        public async Task<string> CreateReleaseNotes(string repoUrl,  string newVersionTag = null, string previousVersionTag = null)
        {
            var commitMessages = await _gitHubService.GetCommitMessages(repoUrl, newVersionTag, previousVersionTag);
            var releaseNotes = await _openAIService.ProcessUserPromtAsync(newVersionTag, previousVersionTag, commitMessages);
            return releaseNotes;
        }
    }
}