namespace Extentions;
public static class StringExtensions
{
    public static (string owner, string repoName) GetOwnerAndNameFromRepoURL(this string repoUrl)
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