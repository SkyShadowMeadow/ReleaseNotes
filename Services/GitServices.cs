using Models;
using Newtonsoft.Json;
using Services.Exceptions;
using Services.Interfaces;

namespace Services
{
    public class GitService : IGitService
    {
        public static string GitHubUrl = "https://api.github.com/repos/";
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient _client;
        private string commitMessagesAsString = "";

        public GitService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "ReleaseNotesApp");
        
        }

        public async Task<GitResponse> GetCommitMessages(string repoUrl, string newVersionTag, string previousVersionTag)
        {
            (string owner, string repoName) = GetOwnerAndNameFromRepoURL(repoUrl);

            var tags = await GetTagsAsync(repoUrl);
            var lastTag = string.IsNullOrEmpty(newVersionTag) ? tags[0].Name : newVersionTag;
            var secondLastTag = string.IsNullOrEmpty(previousVersionTag) ? tags[1].Name : previousVersionTag;

            var commitsUrl = $"{GitHubUrl}{owner}/{repoName}/compare/{secondLastTag}...{lastTag}";
            var gitRresponse = await _client.GetAsync(commitsUrl);
          
            if (gitRresponse.IsSuccessStatusCode)
            {
                string responseBody = await gitRresponse.Content.ReadAsStringAsync();
                GitHubApiResponse commitMessages = JsonConvert.DeserializeObject<GitHubApiResponse>(responseBody);
                GitResponse gitResponse = new GitResponse();
                gitResponse.commitMessages = commitMessages.GetConcatenatedResult();
                gitResponse.lasTag = lastTag;
                gitResponse.secondLastTag = secondLastTag;
                return gitResponse;
            }
            else
            {
                return await Task.FromException<GitResponse>(new GitException("Failed to retrieve commits from GitHub."));
            }
        }

        private async Task<List<Tag>> GetTagsAsync(string repoUrl)
        {
            (string owner, string repoName) = GetOwnerAndNameFromRepoURL(repoUrl);
            var tagsUrl = $"{GitHubUrl}{owner}/{repoName}/tags";
            var gitRresponse = await _client.GetAsync(tagsUrl);
            var tagsJson = await gitRresponse.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson)
                       ?? throw new Exception("Error while parsing tags.");

            if (tags.Count < 2)
            {
                return await Task.FromException<List<Tag>>(new GitException("The repository does not have at least two tags."));
            }
            else
            {
                return tags;
            }
        }

        private (string owner, string repoName) GetOwnerAndNameFromRepoURL(string repoUrl)
        {
            string[] parts = repoUrl.Split('/');
            if (parts.Length < 4)
            {
                throw new ArgumentException("Invalid repository URL");
            }
            string owner = parts[3];
            string repoName = parts[4];
            return (owner, repoName);
        }
    }
}