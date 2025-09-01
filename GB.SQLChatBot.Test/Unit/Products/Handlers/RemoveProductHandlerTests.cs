using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Pars.Core.Exception;
using GB.SQLChatBot.Business.Products.Handlers;
using GB.SQLChatBot.Business.Products.Requests;
using GB.SQLChatBot.Data;
using GB.SQLChatBot.Data.EF;
using Xunit;

namespace GB.SQLChatBot.Test.Unit.Products.Handlers;

public class RemoveProductHandlerTests : Setup
{

    private readonly RemoveProductHandler _handler;
    private readonly Mock<TemplateContext> _mockDbContext;

    public RemoveProductHandlerTests()
    {
        _mockDbContext = new Mock<TemplateContext>(new DbContextOptions<TemplateContext>());
        _handler = new RemoveProductHandler(_mockDbContext.Object);
    }

    [Fact]
    public async Task When_RemoveProductRequestIsValid_Then_ReturnSuccess()
    {
        //ARRANGE
        var request = new RemoveProduct(1);
        _mockDbContext.Setup(k => k.Products.FindAsync(1)).ReturnsAsync(new Product(",", ",", 1)
        {
            Comments = new List<Comment>()
                {
                    new Comment("content", 1,new Product("name", "code",1) { Id = 1 }) { Id = 1 }
                }
        });

        //ACT
        var actual = await _handler.HandleAsync(request);

        //ASSERT
        Assert.NotNull(actual);
    }

    [Fact]
    public async Task When_ProductNotExist_Then_ThrowBusinessException()
    {
        //ARRANGE
        var request = new RemoveProduct(15);
        _mockDbContext.Setup(k => k.Products).ReturnsDbSet(new List<Product>());
        //ACT
        Task Actual() => _handler.HandleAsync(request);

        //ASSERT

        await Assert.ThrowsAsync<ObjectNotFoundException>(Actual);

    }
  
}
