using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GB.SQLChatBot.Core.Interfaces;
using GB.SQLChatBot.Business.Leave.Requests;
using GB.SQLChatBot.Business.Base.Query;
using MediatR;
using GB.SQLChatBot.Core.Queries;
using GB.SQLChatBot.Core;
using System.Linq;
using GB.SQLChatBot.Business.Services;

namespace GB.SQLChatBot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotQueryParserService _chatbotQueryParserService;
        private readonly IOpenAIService _openAIService;
        private readonly IMediator _mediator;

        public ChatbotController(IChatbotQueryParserService chatbotQueryParserService, OpenAIService openAIService, IMediator mediator)
        {
            _chatbotQueryParserService = chatbotQueryParserService;
            _openAIService = openAIService;
            _mediator = mediator;
        }

        //[HttpPost("query")]
        //public async Task<IActionResult> Query([FromBody] ChatBotRequest request)
        //{
        //    if (string.IsNullOrWhiteSpace(request?.Prompt))
        //        return BadRequest("Prompt boş olamaz.");

        //    var response = await _chatbotService.ProcessUserQuery(request.Prompt);

        //    return Ok(response);
        //}

        [HttpPost("execute")]
        public async Task<IActionResult> ExecutePrompt([FromBody] string prompt)
        {
            var result = await _mediator.Send(new ExecuteChatPromptQuery { Prompt = prompt });
            return Ok(result);
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatBotRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt boş olamaz.");

            ChatbotResponse response = await _chatbotQueryParserService.ProcessUserQuery(request.Prompt);

            return Ok(response);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var response = await _chatbotQueryParserService.ProcessUserQuery("Son 5 izin kaydını getir");

            return Ok(response);
        }

        [HttpGet("test2")]
        public async Task<IActionResult> TestQuery()
        {
            var sql = "SELECT TOP 5 PersonRef, Duration FROM tb_PersonAnnualLeaveAction ORDER BY CreatedDate DESC";

            var query = new ExecuteRawSqlQuery { SqlQuery = sql };
            var data = await _mediator.Send(query);

            var response = new ChatbotResponse
            {
                Success = data.Any(),
                Sql = sql,
                Message = data.Any() ? "Veriler başarıyla alındı." : "Hiçbir sonuç döndürülmedi.",
                Data = data
            };

            return Ok(response);
        }

    }
}
