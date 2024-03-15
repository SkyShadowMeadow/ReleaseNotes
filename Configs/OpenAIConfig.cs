using System;

namespace Configs
{
    public class OpenAIConfig
    {
        private readonly IConfiguration _configuration;

        public OpenAIConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string ApiKey => _configuration["OpenAI:ApiKey"];
    }
}