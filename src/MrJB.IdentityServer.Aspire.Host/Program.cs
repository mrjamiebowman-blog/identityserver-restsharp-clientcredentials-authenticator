var builder = DistributedApplication.CreateBuilder(args);

// components

// services
builder.AddProject<Projects.MrJB_IdentityServer>("identity-server");
builder.AddProject<Projects.MrJB_IdentityServer_Api>("demo-api");
builder.AddProject<Projects.MrJB_IdentityServer_RestSharp_Console>("console-app");

builder.Build().Run();
