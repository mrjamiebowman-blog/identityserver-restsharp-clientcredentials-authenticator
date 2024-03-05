namespace MrJB.IdentityServer.RestSharp.Console.Configuration;

public class ApiClientConfiguration
{
    public const string Position = "ApiClient";

    public string IdentityServerUrl { get; set; } = "http://localhost:5000";

    public string ClientId { get; set; } = "m2m.client";

    public string ClientSecret { get; set; } = "511536EF-F270-4058-80CA-1C89C192F69A";

    /// <summary>
    ///  This API does not exist.
    /// </summary>
    public string ApiUrl { get; set; } = "http://localhost:5244/";
}
