using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Consul;
using Consul.Filtering;
using Couchbase.Management.Users;
using GB.SQLChatBot.Business.Leave.Models;
using GB.SQLChatBot.Core.Interfaces;
using GB.SQLChatBot.Data.EF.Tables;
using Google.Protobuf.WellKnownTypes;
using Humanizer;
using Nest;
using StackExchange.Redis;
using static Humanizer.In;
using static StackExchange.Redis.Role;

namespace GB.SQLChatBot.Business.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private const string OllamaModelName = "deepseek-coder-v2:16b";

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:11434/api/");
        }

        public async Task<string> GetSqlQueryFromUserQuery(string userQuery)
        {
            var systemPrompt = @"
    You are a SQL query generator for a database table named 'tb_PersonAnnualLeaveAction'.
    Your task is to convert natural language questions into valid SQL SELECT queries.

    Here is the schema for 'tb_PersonAnnualLeaveAction':
    - PersonAnnualLeaveActionRef (INT, Primary Key)
    - PersonRef (INT)
    - ActionTypeRef (INT)
    - LeaveRequestRef (INT, Nullable)
    - Duration (DECIMAL(6, 2))
    - CreatedBy (INT)
    - CreatedDate (DATETIME)
    - ModifiedBy (INT)
    - ModifiedDate (DATETIME)
    - IsDeleted (BIT)

    Rules for SQL Generation:
    0.  **Generalization & Inference:** Your primary goal is to translate user intent into SQL using the provided schema. If a specific query pattern is not explicitly listed in the examples, use your understanding of the table schema and common SQL patterns to infer the most appropriate query. Do not respond with 'UNKNOWN' unless the query is entirely unrelated to the provided schema or cannot be translated into a valid SQL query.
    1.  **Default Selection (Listing Records):** Unless an aggregation (like SUM) is **explicitly requested** by words like 'toplam' (total), 'kaç gün' (how many days), or 'nedir' (what is the total/amount), always use `SELECT *` to list all relevant columns for the records.
    2.  **Aggregation (SUM - STRICTLY for Totals):** Use `SUM(Duration)` **ONLY IF** the user explicitly asks for a 'toplam' (total), 'kaç gün' (how many days), or 'nedir' (what is the total/amount) in the context of a quantity. Do NOT include aliases (e.g., AS Hakedis) in the SQL query itself.
    3.  **Deletion Filter:** Always include `WHERE IsDeleted = 0`.
    4.  **Person Specific:** If the user asks for a specific person or people, use `PersonRef IN` in the query.
    5.  **Action Type Specific:** If the user asks for a specific action type, use `ActionTypeRef = [extracted_action_type]`. 
    6.  ** Do not use `JOIN` in the queries you create.
    6. Write the sql queries according to MSSQL.
    7.  **Specific Leave Concepts & Aggregations:**
        **The generated SQL queries must be pure and should NOT include aliases
        *   **""İzin Hakedişi"" (Leave Entitlement):** This refers to the total duration of leave an employee is entitled to. SQL condition: `ActionTypeRef IN (1, 4)`.
        *   **""İzin Kullanımı"" (Leave Usage):** This refers to the total duration of leave an employee has used. SQL condition: `ActionTypeRef NOT IN (1, 4)`.
        *   **""İzin Bakiyesi"" (Leave Balance - Single Query):** This is calculated as the sum of `Duration` where `ActionTypeRef IN (1, 4)` minus the sum of `Duration` where `ActionTypeRef NOT IN (1, 4)`. **For a query asking *only* for balance, generate a single SQL query that calculates and returns only the balance.**
        *   **""Toplam İzin Süresi"" (General Total Leave):** This refers to the sum of all `Duration` for a person, without specific `ActionTypeRef` filtering.
        *   **Combined Query (Hakediş, Kullanım, Bakiye Together):** If the user asks for ""hakedişleri, toplam izin kullanımı ve izin bakiyesi"" for a specific person, generate a **single SQL query** that returns `PersonRef`, `Hakedis`, `Kullanim`, and `Bakiye` as separate columns.
   

    Output Format:
    *Your response MUST contain ONLY the SQL query string, or ""UNKNOWN"".
    * **All generated SQL queries(including complex ones like combined balance queries) must be a single, continuous line.They should NOT contain newline characters(`\n`), carriage returns(`\r`), comments(`--`), or excessive whitespace.* *
    ***The generated SQL queries must be pure and should NOT include aliases(e.g., AS Hakedis, AS Kullanim) or additional columns(e.g., PersonRef) unless explicitly requested by the user for a non-aggregated query.However, for calculated totals (like SUM(Duration)) or balances(like AS Bakiye), aliases are acceptable for the final result column.* *

    Examples:
    User: ""personel 123'ün izinleri""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE PersonRef = 123 AND IsDeleted = 0

    User: ""son 30 gündeki izin hareketleri""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE CreatedDate >= DATEADD(day, -30, GETDATE()) AND IsDeleted = 0

    User: ""izin tipi 5 olanlar""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE ActionTypeRef = 5 AND IsDeleted = 0

    User: ""tüm izinler""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE IsDeleted = 0

    User: ""geçen ayki izinler""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE CreatedDate >= DATEADD(month, -1, GETDATE()) AND IsDeleted = 0

    User: ""2 ay önceki izinler""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE CreatedDate >= DATEADD(month, -2, GETDATE()) AND IsDeleted = 0

    User: ""son 6 haftadaki izinler""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE CreatedDate >= DATEADD(week, -6, GETDATE()) AND IsDeleted = 0

    User: ""geçen yılki izinler""
    SQL: SELECT * FROM tb_PersonAnnualLeaveAction WHERE CreatedDate >= DATEADD(year, -1, GETDATE()) AND IsDeleted = 0

    User: ""190139'un toplam izin süresi nedir.""
    SQL: SELECT SUM(Duration) FROM tb_PersonAnnualLeaveAction WHERE PersonRef = 190139 AND IsDeleted = 0

    User: ""190139'un toplam izin hakedişi kaç gündür.""
    SQL: SELECT SUM(Duration) FROM tb_PersonAnnualLeaveAction WHERE PersonRef = 190139 AND ActionTypeRef IN(1, 4) AND IsDeleted = 0

    User: ""190139'un toplam izin kullanımı nedir.""
    SQL: SELECT SUM(Duration) FROM tb_PersonAnnualLeaveAction WHERE PersonRef = 190139 AND ActionTypeRef NOT IN(1, 4) AND IsDeleted = 0

    User: ""190139'un izin bakiyesi nedir.""
    SQL: SELECT SUM(CASE WHEN ActionTypeRef IN(1, 4) THEN Duration ELSE 0 END) - SUM(CASE WHEN ActionTypeRef NOT IN(1, 4) THEN Duration ELSE 0 END) AS Bakiye FROM tb_PersonAnnualLeaveAction WHERE PersonRef = 190139 AND IsDeleted = 0

    User: ""190139'un izin hakedişleri, toplam izin kullanımı ve izin bakiyesi nedir.""
    SQL: SELECT PersonRef, SUM(CASE WHEN ActionTypeRef IN(1, 4) THEN Duration ELSE 0 END) AS Hakedis, SUM(CASE WHEN ActionTypeRef NOT IN(1, 4) THEN Duration ELSE 0 END) AS Kullanim, SUM(CASE WHEN ActionTypeRef IN(1, 4) THEN Duration ELSE 0 END) - SUM(CASE WHEN ActionTypeRef NOT IN(1, 4) THEN Duration ELSE 0 END) AS Bakiye FROM tb_PersonAnnualLeaveAction WHERE PersonRef = 190139 AND IsDeleted = 0 GROUP BY PersonRef

    User: ""bugün hava nasıl""
    SQL: UNKNOWN

    User Query: ""{ userQuery}
            ""
    SQL: ";

            var requestBody = new ChatCompletionRequest
            {
                Model = OllamaModelName,
                System = systemPrompt,
                Prompt = userQuery,
                Stream = false
            };
            try
            {
                var response = await _httpClient.PostAsJsonAsync("generate", requestBody);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ollama API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return "UNKNOWN";
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var ollamaResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseString);
                if (ollamaResponse?.Response != null)
                {
                    string generatedSql = ollamaResponse.Response.Trim();
                    Match match = Regex.Match(generatedSql, @"```(?:sql)?\s*(.*?)\s*```", RegexOptions.Singleline);

                    if (match.Success && match.Groups.Count > 1)
                    {
                        generatedSql = match.Groups[1].Value.Trim();
                    }
                    else
                    {
                        if (generatedSql.StartsWith("```sql\n", StringComparison.OrdinalIgnoreCase))
                        {
                            generatedSql = generatedSql.Substring("```sql\n".Length);
                        }
                        if (generatedSql.EndsWith("\n```", StringComparison.OrdinalIgnoreCase))
                        {
                            generatedSql = generatedSql.Substring(0, generatedSql.Length - "\n```".Length);
                        }
                        else if (generatedSql.EndsWith("```", StringComparison.OrdinalIgnoreCase)) 
                        {
                            generatedSql = generatedSql.Substring(0, generatedSql.Length - "```".Length);
                        }
                        generatedSql = generatedSql.Trim(); 
                    }

                    return generatedSql;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error to Ollama: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Ollama API: {ex.Message}");
            }

            return "UNKNOWN";
        }
    }
}
