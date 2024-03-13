namespace Services
{
    public interface IGitServices
    {
        (string owner, string repoName) ParseRepositoryUrl(string repoUrl);
        Task<List<string>> GetCommitMessagesAsync(string responseBody);
    }
}