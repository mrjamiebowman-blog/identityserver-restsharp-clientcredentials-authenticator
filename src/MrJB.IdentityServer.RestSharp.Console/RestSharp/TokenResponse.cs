using System.Text.Json.Serialization;

namespace MrJB.IdentityServer.RestSharp.Console.RestSharp;

public class TokenResponse
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}
