using Pars.Core.Utils;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace GB.SQLChatBot.Test.Integration.Scenarios.Products.Scenario_1;

[Collection(nameof(HostContext))]
public class WhenProductRequestIsNullThenReturnEmptyResponse
{
    protected readonly HttpClient _client;

    public WhenProductRequestIsNullThenReturnEmptyResponse(TestHost host)
    {
        _client = host.GetClient().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task Run()
    {
        //ARRANGE
        var inputJson = CommonExtensions.LoadJson("Integration\\Scenarios\\Products\\Scenario_1\\input.json");
        var outputJson = CommonExtensions.LoadJson("Integration\\Scenarios\\Products\\Scenario_1\\output.json");

        StringContent stringContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

        //ACT
        var response =
            await _client.PostAsync(
                $"api/product" , stringContent);

        var expected = JsonConvert.SerializeObject(response.StatusCode);

        //ASSERT
        Assert.Equivalent(expected, outputJson);
    }

}