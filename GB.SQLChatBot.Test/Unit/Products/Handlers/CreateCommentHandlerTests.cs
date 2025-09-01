using System.Linq.Expressions;
using System.Security.Claims;
using Moq;
using Moq.AutoMock;
using Pars.Core.Auth.OpenId;
using Pars.Core.Data;
using Pars.Core.Exception;
using Pars.Core.Uow;
using GB.SQLChatBot.Business.Products.Handlers;
using GB.SQLChatBot.Business.Products.Requests;
using GB.SQLChatBot.Data;
using Xunit;

namespace GB.SQLChatBot.Test.Unit.Products.Handlers;

public class CreateCommentHandlerTests : Setup
{
    private readonly CreateCommentHandler _handler;
    private readonly AutoMocker _mocker;
    public CreateCommentHandlerTests()
    {
        _mocker = new AutoMocker();

        var configuration = base.GetConfiguration();

        _mocker.Use(configuration);

        _handler = _mocker.CreateInstance<CreateCommentHandler>();

        _mocker.GetMock<IPrincipalProvider>().SetupGet(x => x.User).Returns(new ParsUser(new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(new List<Claim> { new Claim("personref", "1") }) })));
    }

    [Theory]
    [InlineData("content-1", 1)]
    [InlineData("content-2", 2)]
    [InlineData("content-3", 3)]
    [InlineData("content-4", 4)]
    [InlineData("content-5", 5)]
    public async Task When_CommentRequestIsValid_Then_ReturnEmptyObject(string code, int productId)
    {
        //ARRANGE
        var request = new CreateComment()
        {
            Content = code,
            ProductId = productId
        };

        var productRepositoryMock = new Mock<IRepository<Product, int>>();
        productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product,bool>>>())).ReturnsAsync(new Product("name", code, productId));
        _mocker.GetMock<IUnitOfWork>().Setup(k => k.GetRepository<Product, int>())
            .Returns(productRepositoryMock.Object);


        //ACT
        var actual = await _handler.HandleAsync(request);

        //ASSERT
        Assert.NotNull(actual.Result);
    }
    [Fact]
    public async Task When_ProductNotExist_Then_ThrowBusinessException()
    {
        //ARRANGE
        var request = new CreateComment()
        {
            Content = "code",
            ProductId = 1
        };

        var productRepositoryMock = new Mock<IRepository<Product, int>>();

        _mocker.GetMock<IUnitOfWork>().Setup(k => k.GetRepository<Product, int>())
            .Returns(productRepositoryMock.Object);


        //ASSERT
        await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
        {
            await _handler.HandleAsync(request);
        });

    }
    
}