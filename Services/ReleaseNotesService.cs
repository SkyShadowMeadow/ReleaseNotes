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
            var gitResponse = await _gitHubService.GetCommitMessages(repoUrl, newVersionTag, previousVersionTag);
            var releaseNotes = await _openAIService.ProcessUserPromtAsync(gitResponse.lasTag, gitResponse.secondLastTag, gitResponse.commitMessages);
            return releaseNotes;
        }
    }
}