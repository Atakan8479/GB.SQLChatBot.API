using Moq.AutoMock;
using GB.SQLChatBot.Business.Products.Events;
using GB.SQLChatBot.Business.Products.Events.Handlers;
using Xunit;

namespace GB.SQLChatBot.Test.Unit.Products.Events.Handlers;

public class ProductUpdatedEventHandlerTests : Setup
{
    private readonly ProductUpdatedEventHandler _productUpdatedEvent;
    private AutoMocker _mocker;
    public ProductUpdatedEventHandlerTests()
    {
        _mocker = new AutoMocker();
        _productUpdatedEvent = _mocker.CreateInstance<ProductUpdatedEventHandler>();
    }
    [Fact]
    public async Task When_ProductIsValid_Then_ReturnEmptyObject()
    {
        //ARRANGE
        var @event = new ProductUpdatedEvent();

        //ACT
        var expected = await _productUpdatedEvent.HandleAsync(@event);

        //ASSERT
        Assert.Null(expected);
    }
}
