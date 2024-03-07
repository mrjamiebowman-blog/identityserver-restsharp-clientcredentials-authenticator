using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MrJB.IdentityServer.RestSharp.Console.Configuration;
using MrJB.IdentityServer.RestSharp.Console.Models;
using MrJB.IdentityServer.RestSharp.Console.Models.Queries;
using MrJB.IdentityServer.RestSharp.Console.RestSharp;
using RestSharp;

namespace MrJB.IdentityServer.RestSharp.Console.Services;

public class ApiClientService : IApiClientService, IDisposable
{
    // logging
    private readonly Serilog.ILogger _logger;

    // services
    public RestClient RestClient { get; set; }

    // configuration
    private readonly ApiClientConfiguration _apiClientConfiguration;

    public ApiClientService(Serilog.ILogger logger, ApiClientConfiguration apiClientConfiguration)
    {
        // logging
        _logger = logger;

        // config
        _apiClientConfiguration = apiClientConfiguration;

        // restsharp
        var options = new RestClientOptions(apiClientConfiguration.ApiUrl) {
            Authenticator = new ClientCredentialsAuthenticator(_logger, apiClientConfiguration)
        };

        // set rest client
        RestClient = new RestClient(options);
    }

    /// <summary>
    ///  Get all users
    ///  Returns: 200, 400, 500
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Customer?> GetCustomerAsync(GetCustomerQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            // auth request
            var request = new RestRequest("customers");

            // response
            var data = await RestClient.GetAsync<Customer>(request,
                                                           cancellationToken: cancellationToken);

            return data;
        }
        catch (Exception ex)
        {
            _logger.Error("ApiClientService.GetCustomerAsync() Error: {error}", ex.Message);
            throw ex;
        }
    }

    public void Dispose()
    {
        RestClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
