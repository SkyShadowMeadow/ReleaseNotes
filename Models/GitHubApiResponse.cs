using System.Text;

namespace Models
{
    public class GitHubApiResponse
    {
        public List<CommitInfo> Commits { get; set; }

        public string GetConcatenatedResult()
        {
            StringBuilder sb = new StringBuilder();
            foreach (CommitInfo item in Commits)
            {
                sb.AppendLine(item.Commit.Message);
            }
            return sb.ToString();
        }
    }

    public class Commit
    {
        public string Message { get; set; }
    }

    public class CommitInfo
    {
        public Commit Commit { get; set; }
    }
}