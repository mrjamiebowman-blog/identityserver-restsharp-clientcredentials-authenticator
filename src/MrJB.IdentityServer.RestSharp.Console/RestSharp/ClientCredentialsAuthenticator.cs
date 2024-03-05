using MrJB.IdentityServer.RestSharp.Console.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace MrJB.IdentityServer.RestSharp.Console.RestSharp;

public class ClientCredentialsAuthenticator : AuthenticatorBase 
{
    readonly ApiClientConfiguration _apiClientConfiguration;

    public ClientCredentialsAuthenticator(ApiClientConfiguration apiClientConfiguration) : base("")
    {
        _apiClientConfiguration = apiClientConfiguration;
    }

    protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        // get token
        Token = string.IsNullOrEmpty(Token) ? await GetTokenAsync() : Token;

        // validate token

        // append token to header
        return new HeaderParameter(KnownHeaders.Authorization, $"Bearer {Token}");
    }

    public async Task<string> GetTokenAsync()
    {
        // restsharp client
        var authClient = new RestClient($"{_apiClientConfiguration.IdentityServerUrl}");

        // auth request
        var request = new RestRequest("connect/token");
        request.AddParameter("client_id", _apiClientConfiguration.ClientId);
        request.AddParameter("client_secret", _apiClientConfiguration.ClientSecret);
        request.AddParameter("grant_type", "client_credentials");
        request.AddParameter("scope", "api.all");

        // response
        var access_token = authClient.Post<TokenResponse>(request);

        // set token
        Token = access_token.AccessToken;

        return access_token.AccessToken;
    }
}
