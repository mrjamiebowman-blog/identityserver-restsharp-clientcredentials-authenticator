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
var apiClientService = new ApiClientService(Log.Logger, apiClientConfiguration);

// cancellation token
CancellationTokenSource cts = new CancellationTokenSource();

// count
var count = 0;

do
{
    try
    {
        // make request and output data.
        var query = new MrJB.IdentityServer.RestSharp.Console.Models.Queries.GetCustomerQuery();
        var customer = await apiClientService.GetCustomerAsync(query, cts.Token);

        // output 
        Log.Information("Customer: {customer}, E-mail: {email}.", customer.CustomerName, customer.Email);
    } catch (Exception ex) {
        Log.Error("Error: {error}", ex.Message);
    }

    // increment count
    count++;

    // wait 1 second
    await Task.Delay(1000);
    
    if (count == 10) {
        // exit after 10
        cts.Cancel();
    }
} while (!cts.IsCancellationRequested);
