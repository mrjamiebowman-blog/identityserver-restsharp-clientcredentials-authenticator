namespace MrJB.IdentityServer.RestSharp.Console.Configuration;

public class ApiClientConfiguration
{
    public const string Position = "ApiClient";

    public string IdentityServerUrl { get; set; } = "https://localhost:5000";

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    /// <summary>
    ///  This API does not exist.
    /// </summary>
    public string ApiUrl { get; set; } = "https:/test.mrjamiebowman.com/api/";
}
