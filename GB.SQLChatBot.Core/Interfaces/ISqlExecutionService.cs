
namespace GB.SQLChatBot.Core.Interfaces
{
    public interface ISqlExecutionService
    {
        Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string sqlQuery);
    }
}
