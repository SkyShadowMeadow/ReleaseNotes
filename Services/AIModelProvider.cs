using Models;

namespace Services
{
    public class AIModelProvider
    {
        public AIModel ChatGptAIModel { get; }

        public AIModelProvider(IConfiguration configuration)
        {
            ChatGptAIModel = new AIModel(configuration["OpenAI:ApiKey"], configuration["OpenAI:ApiUrl"], configuration["OpenAI:ApiModelName"]);
        }
    }
}