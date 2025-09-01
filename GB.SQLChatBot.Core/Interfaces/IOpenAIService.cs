
namespace GB.SQLChatBot.Core.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> GetSqlQueryFromUserQuery(string userQuery);
    }
}