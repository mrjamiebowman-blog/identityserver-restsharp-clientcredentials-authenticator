using MrJB.IdentityServer.RestSharp.Console.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System.IdentityModel.Tokens.Jwt;

namespace MrJB.IdentityServer.RestSharp.Console.RestSharp;

public class ClientCredentialsAuthenticator : AuthenticatorBase 
{
    // logging
    private readonly Serilog.ILogger _logger;
    
    // config
    readonly ApiClientConfiguration _apiClientConfiguration;

    public ClientCredentialsAuthenticator(Serilog.ILogger logger, ApiClientConfiguration apiClientConfiguration) : base("")
    {
        _logger = logger;
        _apiClientConfiguration = apiClientConfiguration;
    }

    protected override async ValueTask<Parameter> GetAuthenticationParameter(string accessToken)
    {
        // get token
        Token = string.IsNullOrEmpty(Token) ? await GetTokenAsync() : Token;

        // validate token expiration
        var jwt = new JwtSecurityToken(Token);

        if (jwt.ValidTo.AddMinutes(-5) <= DateTime.UtcNow) {
            Token = await GetTokenAsync();
        }

        // append token to header
        return new HeaderParameter(KnownHeaders.Authorization, $"Bearer {Token}");
    }

    public async Task<string> GetTokenAsync()
    {
        try
        {
            // logger
            _logger.Information("Requesting new token from IdentityServer...");

            // restsharp client
            var authClient = new RestClient($"{_apiClientConfiguration.IdentityServerUrl}");

            // auth request
            var request = new RestRequest("connect/token");
            request.AddParameter("client_id", _apiClientConfiguration.ClientId);
            request.AddParameter("client_secret", _apiClientConfiguration.ClientSecret);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", "scope1");

            // response
            var access_token = await authClient.PostAsync<TokenResponse>(request);

            // set token
            Token = access_token?.AccessToken ?? "";
            return Token;
        } catch (Exception ex) {
            _logger.Error("ClientCredentialsAuthenticator.GetTokenAsync() Error: {error}", ex.Message);
            throw ex;
        }
    }
}
