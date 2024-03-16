using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> ProcessUserPromtAsync(string tag1, string tag2, string input);
    }
}