//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Net;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Pars.Core.Monitoring.OpenTelemetry.Metrics;
//using GB.SQLChatBot.Business;
//using GB.SQLChatBot.Data.EF;

//namespace GB.SQLChatBot.API.Controllers;

//[ExcludeFromCodeCoverage]
//[Route("/startup/")]
//[ApiController]
//public class StartUpController : ControllerBase
//{
//    private readonly ILogger<StartUpController> _logger;
//    private readonly TemplateContext _templateContext;
//    private readonly IParsMetrics _metrics;
//    public StartUpController(TemplateContext templateContext, IParsMetrics metrics, ILogger<StartUpController> logger)
//    {
//        _templateContext = templateContext;
//        _metrics = metrics;
//        _logger = logger;
//    }

//    [HttpGet("init")]
//    public async Task<IActionResult> StartUp()
//    {
//        try
//        {
//            var product = await _templateContext.Products.FirstOrDefaultAsync(x => x.Id == 1);
//            _metrics.Increment(MetricsRegistry.StartUpCounter);
//        }
//        catch (Exception e)
//        {
//           _logger.LogError("startup exception", e);
//        }

//        return new ContentResult
//        {
//            ContentType = "text/html; charset=utf-8",
//            StatusCode = (int)HttpStatusCode.OK
//        };
//    }
//}
