using MrJB.IdentityServer.RestSharp.Console.Configuration;
using MrJB.IdentityServer.RestSharp.Console.Models;
using MrJB.IdentityServer.RestSharp.Console.Models.Queries;
using RestSharp;

namespace MrJB.IdentityServer.RestSharp.Console.Services;

public class ApiClientService : IApiClientService, IDisposable
{
    // logging

    // services
    public RestClient RestClient { get; set; }

    // configuration
    private readonly ApiClientConfiguration _apiClientConfiguration;

    public ApiClientService(ApiClientConfiguration apiClientConfiguration)
    {
        _apiClientConfiguration = apiClientConfiguration;
    }

    /// <summary>
    ///  Get all users
    ///  Returns: 200, 400, 500
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IList<Customer>> GetCustomerAsync(GetCustomerQuery query, CancellationToken cancellationToken = default)
    {
        // users
        var data = new List<Customer>();

        try
        {
            // auth request
            var request = new RestRequest("customers");

            // parameters
            //if (query.StartDate != null)
            //{
            //    request.AddParameter("startDate", query.StartDate.ToString());
            //}
            //if (query.EndDate != null)
            //{
            //    request.AddParameter("endDate", query.EndDate.ToString());
            //}

            // response
            data = await RestClient.GetAsync<List<Customer>>(request);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return data;
    }

    public void Dispose()
    {
        RestClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
