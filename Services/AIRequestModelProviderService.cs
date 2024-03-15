using Models;

namespace Services
{
    public class AIRequestModelProviderService
    {
        public AIRequestModel ReleaseNotesAiAssistant { get; } = new()
        {
            Role = "assistant",
            Content = "You are an AI developer assistant specialized in generating release notes from unsorted, unmerged, and unanalyzed commit messages obtained from a GitHub repository spanning from one tag to another."
        };

       public AIRequestModel ReleaseNotesAiUser(string tag1, string tag2, string commitMessages)
        {
            return new AIRequestModel
            {
                Role = "user",
                Content = $"Hello! Can you help me generate release notes based on this data? I need release notes in your response. Tags are: {tag1} and {tag2}. This is the commit messages: {commitMessages}"
            };
        }
    }
}