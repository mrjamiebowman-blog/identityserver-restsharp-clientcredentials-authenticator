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

// cancel after 10 mins
cts.CancelAfter(600); 

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

    // wait 3 seconds
    await Task.Delay(3000);
} while (!cts.IsCancellationRequested);
