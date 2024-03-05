using MrJB.IdentityServer.RestSharp.Console.Configuration;

namespace MrJB.IdentityServer.RestSharp.Console.Services;

public class ApiClientService : IApiClientService
{
    // logging

    // services

    // configuration
    private readonly ApiClientConfiguration _apiClientConfiguration;

    public ApiClientService(ApiClientConfiguration apiClientConfiguration)
    {
        _apiClientConfiguration = apiClientConfiguration;
    }
}
