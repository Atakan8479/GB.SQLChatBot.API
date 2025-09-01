//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Pars.Core.Auth;
//using Pars.Core.Cqrs.Commands;
//using Pars.Core.Cqrs.Queries;
//using GB.SQLChatBot.Business.Products.Requests;
//using System.Diagnostics.CodeAnalysis;
//using System.Threading.Tasks;

//namespace GB.SQLChatBot.API.Controllers;

//[ExcludeFromCodeCoverage]
//[Route("api/product")]
//[ApiController]
//[Authorize(AuthenticationSchemes = AuthenticationSchema.All)]
//public class ProductController : ControllerBase
//{
//    private readonly ICommandSender _commandSender;
//    private readonly IQuerySender _querySender;

//    public ProductController(
//        ICommandSender commandSender,
//        IQuerySender querySender)
//    {
//        _commandSender = commandSender;
//        _querySender = querySender;
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> Get(int id)
//        => Ok(await _querySender.QueryAsync(new GetProduct(id)));

//    [HttpGet("filter")]
//    public async Task<IActionResult> Filter([FromQuery] ListOfProduct command)
//        => Ok(await _querySender.QueryAsync(command));

//    [HttpPost]
//    public async Task<IActionResult> ProductCreate([FromBody] CreateProduct command) =>
//        Ok(await _commandSender.SendAsync<CreateProduct, int>(command));

//    [HttpPut]
//    public async Task ProductChange([FromBody] ChangeProduct command)
//        => await _commandSender.SendAsync(command);

//    [HttpDelete("{productId}")]
//    public async Task ProductDelete([FromRoute] int productId)
//        => await _commandSender.SendAsync(new RemoveProduct(productId));

//    [HttpGet("{productId}/comments")]
//    public async Task<IActionResult> Comments([FromRoute] int productId)
//        => Ok(await _querySender.QueryAsync(new ListOfComment(productId)));

//    [HttpPost("comment")]
//    public async Task ProductCommentCreate([FromBody] CreateComment command)
//        => await _commandSender.SendAsync(command);


//    [HttpDelete("{productId}/comment/{commentId}")]
//    public async Task ProductCommentDelete([FromRoute] int productId, [FromRoute] int commentId)
//        => await _commandSender.SendAsync(new RemoveComment(commentId, productId));

//}
