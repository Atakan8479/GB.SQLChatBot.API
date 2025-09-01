// GB.SQLChatBot.Business/Queries/ExecuteRawSqlQuery.cs
using MediatR;
using System.Collections.Generic;
using GB.SQLChatBot.Data.DTOs;

namespace GB.SQLChatBot.Core.Queries
{
    public class ExecuteRawSqlQuery : IRequest<List<Dictionary<string, object>>>
    {
        public string SqlQuery { get; set; }
    }
}