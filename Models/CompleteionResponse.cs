namespace Models
{
    public class CompletionResponse
    {
        public Choice[] choices { get; set; }
    }

    public class Choice
    {
        public ChatMessage message { get; set; }
    }

    public class ChatMessage
    {
        public string content { get; set; }
        public string role { get; set; }
    }
}