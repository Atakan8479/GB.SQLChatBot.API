using GB.SQLChatBot.Core.Interfaces;
using GB.SQLChatBot.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SQLChatBot.Business.Services
{
    public class SqlExecutionService : ISqlExecutionService
    {
        private readonly LeaveContext _context;

        public SqlExecutionService(LeaveContext context)
        {
            _context = context;
        }

        public async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string sqlQuery)
        {
            var result = new List<Dictionary<string, object>>();

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sqlQuery;
            command.CommandType = System.Data.CommandType.Text;

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                }

                result.Add(row);
            }

            return result;
        }
    }

}
