using System;

namespace Services
{
    public class GitServices : IGitServices
    {
        public (string owner, string repoName) ParseRepositoryUrl(string repoUrl)
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