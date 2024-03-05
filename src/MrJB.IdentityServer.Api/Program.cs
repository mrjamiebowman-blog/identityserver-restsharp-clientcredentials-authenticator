using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MrJB.IdentityServer.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(o => { });

// cors
builder.Services.AddCors();

// authentication
builder.Services.AddAuthentication()
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,

                        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        {
                            var jwt = new JsonWebToken(token);
                            return jwt;
                        }
                    };

                    o.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnAuthenticationFailed = (c) =>
                        {
                            var errorMessage = c.Exception.Message;
                            return Task.CompletedTask;
                        }
                    };
                });

// authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("protected_api", policy =>
        policy
            .RequireClaim("scope", "api1"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/customers", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new Customer
        (
            //DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //Random.Shared.Next(-20, 55),
            //summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetCustomers")
.RequireAuthorization("protected_api")
.WithOpenApi();

app.Run();
