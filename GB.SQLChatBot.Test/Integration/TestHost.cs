using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pars.Core.TestContainer;
using GB.SQLChatBot.API;
using GB.SQLChatBot.Data.EF;
using GB.SQLChatBot.Test.Integration.Models;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Xunit;

namespace GB.SQLChatBot.Test.Integration;

public class TestHost : IAsyncLifetime
{
    public StsTokenRequest? StsTokenRequest;
    public WebApplicationFactory<Program>? app;
    public async ValueTask<HttpClient> GetClient()
    {
        var opts = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        };

        var client = app!.CreateClient(opts);

        var stsResponse = await GetAccessTokenFromSts();

        if (stsResponse != null && !string.IsNullOrEmpty(stsResponse.AccessToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", stsResponse.AccessToken);
        }
        else
        {
            throw new InvalidOperationException("Unauthorized access!");
        }

        return client;
    }

    public async Task InitializeAsync()
    {
        await TestContainerCreator.InitializeContainers(
            new List<ContainerType>()
            {
                ContainerType.Redis,
                ContainerType.MsSql,
               // ContainerType.Couchbase,
               // ContainerType.MsSql

            });

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Integration.json")
            .Build();

        app = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseConfiguration(configuration);

                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<Setup>();
                });
            });

        StsTokenRequest = configuration.GetSection("StsTokenRequest").Get<StsTokenRequest>();

        //MongoDb
        //var setup = app?.Services.GetService<Setup>();
        //var db = app?.Services.GetService<IMongoDatabase>();

        //if (db != null && setup != null)
        //{
        //    setup.Populate(db);
        //}

       // MSSql
        var scope = app?.Services.CreateScope();
        var setup = scope?.ServiceProvider.GetService<Setup>();
        var db = scope?.ServiceProvider.GetService<TemplateContext>();

        if (db != null && setup != null)
        {
            setup.Populate(db);
        }
    }

    public async Task DisposeAsync()
    {
        await TestContainerCreator.ShutdownAll();
    }


    public async Task<StsTokenResponse?> GetAccessTokenFromSts()
    {
        if (StsTokenRequest == null ||
            StsTokenRequest.ClientId == null ||
            StsTokenRequest.UserName == null ||
            StsTokenRequest.Password == null ||
            StsTokenRequest.GrantType == null ||
            StsTokenRequest.ClientSecret == null ||
            StsTokenRequest.Scope == null)
        {
            throw new InvalidOperationException("Unauthorized access!");
        }

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, StsTokenRequest.BaseAddress);
        var collection = new List<KeyValuePair<string, string>>
        {
            new("client_id", StsTokenRequest.ClientId),
            new("username", StsTokenRequest.UserName),
            new("password", StsTokenRequest.Password),
            new("grant_type", StsTokenRequest.GrantType),
            new("client_secret", StsTokenRequest.ClientSecret),
            new("scope", StsTokenRequest.Scope)
        };
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;

        using var httpResponse = await client.SendAsync(request);
        if (httpResponse.IsSuccessStatusCode && httpResponse.StatusCode == HttpStatusCode.OK)
        {
            var response = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<StsTokenResponse>(response);
        }

        return default;
    }
}