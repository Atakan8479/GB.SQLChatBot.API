using GB.SQLChatBot.Data.DTOs;
using System.Threading.Tasks;

namespace GB.SQLChatBot.Core.Interfaces
{

    public interface IChatbotQueryParserService
    {
        Task<ChatbotResponse> ProcessUserQuery(string userQuery);
    }

    //public interface IChatbotQueryParserService
    //{
    //    Task<ChatbotResponse> ProcessUserQuery(string userQuery);
    //    Task<string> GenerateSqlFromPromptAsync(string prompt);

    //}

    //public class ChatbotResponse
    //{
    //    public bool Success { get; set; }
    //    public string Message { get; set; }
    //    public string Sql { get; set; } // AI tarafından oluşturulan SQL sorgusu
    //    public List<Dictionary<string, object>> Data { get; set; }
    //}
}
