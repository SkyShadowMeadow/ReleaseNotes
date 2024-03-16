using Models;

namespace Services.Interfaces
{
    public interface IGitService
    {
        Task<GitResponse> GetCommitMessages(string repoUrl, string newVersionTag, string previousVersionTag);
    }
}