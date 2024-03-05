using MrJB.IdentityServer.RestSharp.Console.Configuration;
using MrJB.IdentityServer.RestSharp.Console.Services;
using Serilog;

// logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

// configuration
ApiClientConfiguration apiClientConfiguration = new ApiClientConfiguration();

// services
var apiClientService = new ApiClientService(apiClientConfiguration);

Log.Information("Press [T] to Test.");
ConsoleKeyInfo keyInfo;

do
{
    // read key
    keyInfo = Console.ReadKey();

    if (keyInfo.Key == ConsoleKey.T) {
        Log.Information("Running Auth Test...");

        // make api call

        break;
    }
} while (true);
