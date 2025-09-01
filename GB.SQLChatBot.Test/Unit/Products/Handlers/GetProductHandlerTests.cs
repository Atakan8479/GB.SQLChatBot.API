using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Pars.Core.Exception;
using GB.SQLChatBot.Business.Products;
using GB.SQLChatBot.Business.Products.Handlers;
using GB.SQLChatBot.Business.Products.Requests;
using GB.SQLChatBot.Data;
using GB.SQLChatBot.Data.EF;
using Xunit;

namespace GB.SQLChatBot.Test.Unit.Products.Handlers;

public class GetProductHandlerTests : Setup
{
    private GetProductHandler _handler;
    private readonly Mapper _mapper;
    private readonly Mock<TemplateContext> _mockDbContext;

    public GetProductHandlerTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>()));
        _mockDbContext = new Mock<TemplateContext>(new DbContextOptions<TemplateContext>());
        _handler = new GetProductHandler(_mockDbContext.Object, _mapper);
    }

    [Fact]
    public async Task When_ProductRequestIsValid_Then_ReturnObject()
    {
        //ARRANGE
        var request = new GetProduct(1);
        var products = new List<Product>()
        {
            new("name", "code", 1) { Id = 1 },
            new("name", "code", 2) { Id = 2 },
            new("name", "code", 3) { Id = 3 }
        };

        _mockDbContext.Setup(k => k.Products.FindAsync(1)).ReturnsAsync(new Product(",", ",", 1));


        //ACT
        var actual = await _handler.HandleAsync(request);

        //ASSERT
        Assert.NotNull(actual.Data);
    }
    [Fact]
    public async Task When_ProductNotExist_Then_ThrowBusinessException()
    {
        //ARRANGE
        var request = new GetProduct(1);

        //ACT
        var products = new List<Product>() { new Product("", "", 6) { Id = 6 } };

        _mockDbContext.Setup(k => k.Products).ReturnsDbSet(products);

        //ASSERT
        Task Actual() => _handler.HandleAsync(request);

        await Assert.ThrowsAsync<ObjectNotFoundException>(Actual);

    }
}
