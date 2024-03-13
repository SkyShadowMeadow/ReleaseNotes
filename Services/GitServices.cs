using System;
using Models;
using Newtonsoft.Json;

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

        public async Task<List<string>> GetCommitMessagesAsync(string responseBody)
        {
            dynamic data = JsonConvert.DeserializeObject(responseBody);

            List<string> commitMessages = new List<string>();
            foreach (var commit in data.commits)
            {
                commitMessages.Add(commit["commit"]["message"].ToString());
            }
            return commitMessages;
        }
    }
}