using GB.SQLChatBot.Core.Interfaces;
using GB.SQLChatBot.Data.DTOs;
using GB.SQLChatBot.Core.Queries;
using MediatR;
using System.Threading.Tasks;
using System.Linq;
using GB.SQLChatBot.Core;

namespace GB.SQLChatBot.Business.Services
{
    public class ChatbotQueryParserService : IChatbotQueryParserService
    {
        private readonly IMediator _mediator;
        private readonly IOpenAIService _openAIService;

        public ChatbotQueryParserService(IMediator mediator, IOpenAIService openAIService)
        {
            _mediator = mediator;
            _openAIService = openAIService;
        }

        public async Task<ChatbotResponse> ProcessUserQuery(string userQuery)
        {
            userQuery = userQuery.ToLowerInvariant().Trim();

            // 1. Adım: OpenAI ile SQL üret
            var generatedSql = await _openAIService.GetSqlQueryFromUserQuery(userQuery);

            if (string.IsNullOrWhiteSpace(generatedSql) || generatedSql == "UNKNOWN")
            {
                return new ChatbotResponse
                {
                    Success = false,
                    Message = "Üzgünüm, bu sorgu için geçerli bir SQL oluşturamadım.",
                    Sql = generatedSql,
                    Data = null
                };
            }

            // 2. Adım: SQL çalıştır ve sonuçları al
            var data = await _mediator.Send(new ExecuteRawSqlQuery { SqlQuery = generatedSql });

            return new ChatbotResponse
            {
                Success = data.Any(),
                Message = data.Any() ? "Sorgu başarılı, veriler aşağıda." : "Sorgu çalıştı ancak eşleşen veri bulunamadı.",
                Sql = generatedSql,
                Data = data
            };
        }
    }
}