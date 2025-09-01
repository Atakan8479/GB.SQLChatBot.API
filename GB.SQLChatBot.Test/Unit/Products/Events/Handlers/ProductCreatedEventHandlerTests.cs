using Moq.AutoMock;
using GB.SQLChatBot.Business.Products.Events;
using GB.SQLChatBot.Business.Products.Events.Handlers;
using Xunit;

namespace GB.SQLChatBot.Test.Unit.Products.Events.Handlers;

public class ProductCreatedEventHandlerTests : Setup
{
    private readonly ProductCreatedEventHandler _productCreatedEvent;
    private AutoMocker _mocker;
    public ProductCreatedEventHandlerTests()
    {
        _mocker = new AutoMocker();
        _productCreatedEvent = _mocker.CreateInstance<ProductCreatedEventHandler>();
    }
    [Fact]
    public async Task When_ProductIsValid_Then_ReturnEmptyObject()
    {
        //ARRANGE
        var @event = new ProductCreatedEvent();

        //ACT
        var expected = await _productCreatedEvent.HandleAsync(@event);

        //ASSERT

        Assert.Null(expected);
    }
}
