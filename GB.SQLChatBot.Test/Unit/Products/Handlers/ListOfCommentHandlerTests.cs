using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pars.Core.Exception;
using GB.SQLChatBot.Business.Products.Handlers;
using GB.SQLChatBot.Business.Products;
using GB.SQLChatBot.Business.Products.Requests;
using GB.SQLChatBot.Data;
using GB.SQLChatBot.Data.EF;
using Xunit;
using Moq.EntityFrameworkCore;

namespace GB.SQLChatBot.Test.Unit.Products.Handlers;

public class ListOfCommentHandlerTests : Setup
{
    private readonly ListOfCommentHandler _handler;
    private readonly Mapper _mapper;
    private readonly Mock<TemplateContext> _mockDbContext;

    public ListOfCommentHandlerTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>()));
        _mockDbContext = new Mock<TemplateContext>(new DbContextOptions<TemplateContext>());
        _handler = new ListOfCommentHandler(_mockDbContext.Object, _mapper);
    }

    [Fact]
    public async Task When_ProductRequestIsValid_Then_ReturnComment()
    {
        //ARRANGE
        var request = new ListOfComment(1);
        var product = new Product("", "", 1);
        product.Comments = new List<Comment>() { new Comment("test", 1, new Product("", "", 1) { Id = 1 }) { Id = 1 } };
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
        var request = new ListOfComment(1);
        var products = new List<Product>() { new Product("", "", 6) { Id = 6 } };

        //ACT
        _mockDbContext.Setup(k => k.Products).ReturnsDbSet(products);
        Task Actual() => _handler.HandleAsync(request);

        //ASSERT
        await Assert.ThrowsAsync<ObjectNotFoundException>(Actual);

    }
}