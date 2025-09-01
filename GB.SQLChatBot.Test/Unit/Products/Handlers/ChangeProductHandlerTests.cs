using System.Security.Claims;
using Moq;
using Moq.AutoMock;
using Pars.Core.Auth.OpenId;
using Pars.Core.Bus.MessageBox;
using Pars.Core.Data;
using Pars.Core.Exception;
using Pars.Core.Uow;
using GB.SQLChatBot.Business.Products.Events;
using GB.SQLChatBot.Business.Products.Handlers;
using GB.SQLChatBot.Business.Products.Requests;
using GB.SQLChatBot.Data;
using Xunit;

namespace GB.SQLChatBot.Test.Unit.Products.Handlers;

public class ChangeProductHandlerTests : Setup
{
    private readonly ChangeProductHandler _handler;
    private readonly AutoMocker _mocker;
    public ChangeProductHandlerTests()
    {
        _mocker = new AutoMocker();

        var configuration = base.GetConfiguration();

        _mocker.Use(configuration);

        _handler = _mocker.CreateInstance<ChangeProductHandler>();

        _mocker.GetMock<IPrincipalProvider>().SetupGet(x => x.User).Returns(new ParsUser(new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(new List<Claim> { new Claim("personref", "1") }) })));


    }

    [Theory]
    [InlineData("product-name-1", "product-code-1")]
    [InlineData("product-name-2", "product-code-2")]
    [InlineData("product-name-3", "product-code-3")]
    [InlineData("product-name-4", "product-code-4")]
    [InlineData("product-name-5", "product-code-5")]
    public async Task When_ProductRequestIsValid_Then_ReturnEmptyObject(string code, string name)
    {
        //ARRANGE
        var request = new ChangeProduct()
        {
            Code = code,
            Name = name
        };

        var productRepositoryMock = new Mock<IRepository<Product, int>>();
        productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(new Product(name, code, 1));
        _mocker.GetMock<IUnitOfWork>().Setup(k => k.GetRepository<Product, int>())
            .Returns(productRepositoryMock.Object);


        //ACT
        var actual = await _handler.HandleAsync(request);

       
        //ASSERT
        Assert.Empty(actual.Result.ToString() ?? string.Empty);
    }
    [Fact]
    public async Task When_ProductNotExist_Then_ThrowBusinessException()
    {
        //ARRANGE
        var request = new ChangeProduct()
        {
            Code = "code",
            Name = "name"
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
    [Fact]
    public async Task When_ProductRequestIsValid_Then_SendToOutbox()
    {
        //ARRANGE
        var request = new ChangeProduct()
        {
            Code = "Product-Code",
            Name = "NewProduct-Name"
        };

        var productRepositoryMock = new Mock<IRepository<Product, int>>();
            
        productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(new Product("Product-Code", "NewProduct-Name", 1));
        _mocker.GetMock<IUnitOfWork>().Setup(k => k.GetRepository<Product, int>())
            .Returns(productRepositoryMock.Object);

        //ACT
        var actual = await _handler.HandleAsync(request);

        //ASSERT
        _mocker.GetMock<IMessageOutbox>().Verify(k=>k.SendAsync(It.IsAny<ProductUpdatedEvent>(),null),Times.Once);
    }
}