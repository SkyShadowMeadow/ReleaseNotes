using Models;

namespace Services
{
    public class AIRequestModelProvider
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
                Content = $@"Hello! Can you help me to generate release notes based on this data? 
                            I need release notes in your response. Tags are: {tag1} and {tag2}. 
                            This is the commit messages: {commitMessages}. 
                            I want you to draw an overall conclusion in the beginning for a non-technical person.
                            I want you to analyze messages and find common areas. 
                            I want release notes to start with: Release notes of a new version {tag1}. 
                            Then I want you to cluster those messages in features, fixes, and improvements. 
                            Do not focus on merges. I want this notes to be understandable for a person outside the team, try to be less specific about certain variables. Use more descriptive approach"
            };
        }
    }
}