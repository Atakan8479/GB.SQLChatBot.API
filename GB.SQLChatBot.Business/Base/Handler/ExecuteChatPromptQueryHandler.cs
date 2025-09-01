using GB.SQLChatBot.Business.Base.Query;
using GB.SQLChatBot.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GB.SQLChatBot.Business.Base.Handler
{
    public class ExecuteChatPromptQueryHandler : IRequestHandler<ExecuteChatPromptQuery, List<Dictionary<string, object>>>
    {
        private readonly IChatbotQueryParserService _queryParser;
        private readonly ISqlExecutionService _sqlExecution;

        public ExecuteChatPromptQueryHandler(
            IChatbotQueryParserService queryParser,
            ISqlExecutionService sqlExecution)
        {
            _queryParser = queryParser;
            _sqlExecution = sqlExecution;
        }

        public async Task<List<Dictionary<string, object>>> Handle(ExecuteChatPromptQuery request, CancellationToken cancellationToken)
        {
            var response = await _queryParser.ProcessUserQuery(request.Prompt);

            if (!response.Success || string.IsNullOrWhiteSpace(response.Sql))
                throw new Exception("Yapay zeka geçerli bir SQL üretemedi.");

            var result = await _sqlExecution.ExecuteQueryAsync(response.Sql);

            return result;
        }
    }

}
