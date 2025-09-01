using Xunit;

namespace GB.SQLChatBot.Test.Integration;

[CollectionDefinition(nameof(HostContext))]
public class HostContext : ICollectionFixture<TestHost>
{
}