using Microsoft.Extensions.Configuration;

namespace GB.SQLChatBot.Test.Unit;

public class Setup
{
    //Configurations should not be mocked.
    public IConfiguration GetConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            
            .AddJsonFile("appsettings.Integration.json")
            .Build();
        return configuration;
    }
}