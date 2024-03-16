namespace Models
{
    public class AIModel
    {
        public string openAIKey {get;}
        public string openAIUrl {get;}
        public string modelName {get;}

        public AIModel(string openAIKey,string openAIURL, string modelName)
        {
            this.openAIKey = openAIKey;
            this.openAIUrl = openAIURL;
            this.modelName = modelName;
        }
    }
}