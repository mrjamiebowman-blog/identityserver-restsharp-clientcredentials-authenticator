﻿using MrJB.IdentityServer.RestSharp.Console.Models;
using MrJB.IdentityServer.RestSharp.Console.Models.Queries;

namespace MrJB.IdentityServer.RestSharp.Console.Services;

public interface IApiClientService
{
    Task<Customer?> GetCustomerAsync(GetCustomerQuery query, CancellationToken cancellationToken = default);
}
