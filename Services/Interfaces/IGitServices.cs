namespace Services.Interfaces
{
    public interface IGitService
    {
        Task<string> GetCommitMessages(string repoUrl, string newVersionTag, string previousVersionTag);
    }
}