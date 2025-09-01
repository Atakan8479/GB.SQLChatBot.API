using MediatR;
using GB.SQLChatBot.Data.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using GB.SQLChatBot.Core.Queries;
using System.Data;

namespace GB.SQLChatBot.Business.Leave.Handlers
{
    public class ExecuteRawSqlQueryHandler : IRequestHandler<ExecuteRawSqlQuery, List<Dictionary<string, object>>>
    {
        private readonly LeaveContext _context;

        public ExecuteRawSqlQueryHandler(LeaveContext context)
        {
            _context = context;
        }

        public async Task<List<Dictionary<string, object>>> Handle(ExecuteRawSqlQuery request, CancellationToken cancellationToken)
        {
            var results = new List<Dictionary<string, object>>();

            try
            {
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync(cancellationToken);

                using var command = connection.CreateCommand();
                command.CommandText = request.SqlQuery;
                command.CommandType = CommandType.Text;

                using var reader = await command.ExecuteReaderAsync(cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var value = await reader.IsDBNullAsync(i, cancellationToken) ? null : reader.GetValue(i);
                        row[columnName] = value;
                    }

                    results.Add(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL çalıştırma hatası: {ex.Message}");
            }

            return results;
        }
    }
}