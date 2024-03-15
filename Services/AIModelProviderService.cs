using System;
using Models;

namespace Services
{
    public class AIModelProviderService
    {
        private readonly string _openAIKey;

        public AIModel ChatGptAIModel { get; }

        public AIModelProviderService(IConfiguration configuration)
        {
            _openAIKey = configuration["OpenAI:ApiKey"];
            ChatGptAIModel = new AIModel(_openAIKey, "https://api.openai.com/v1", "gpt-3.5-turbo");
        }
    }
}