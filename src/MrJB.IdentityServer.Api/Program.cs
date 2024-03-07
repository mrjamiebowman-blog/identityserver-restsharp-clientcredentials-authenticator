using Bogus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MrJB.IdentityServer.Api.Models;
using Serilog;

// logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

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
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    //o.Authority = $"http://localhost:5000";
                    o.Audience = $"http://localhost:5000/resources";

                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidateLifetime = false,
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret"))

                        RequireSignedTokens = false,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,

                        SignatureValidator = delegate (string token, TokenValidationParameters parameters) {
                            var jwt = new JsonWebToken(token);
                            return jwt;
                        }
                    };

                    o.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnTokenValidated = (c) =>
                        {
                            Log.Information("Token Validated: {token}", c.SecurityToken.ToString());
                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = (c) =>
                        {
                            
                            var errorMessage = c.Exception.Message;
                            Log.Error("UnAuthorized: {error}", errorMessage);
                            return Task.CompletedTask;
                        }
                    };
                });

// authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("protected_api", policy =>
        policy
            .RequireClaim("scope", "scope1"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/up", () => {
    return "UP";
});

app.MapGet("/customers", () => {
    var customerFaker = new Faker<Customer>()
        .RuleFor(x => x.CustomerName, (f, u) => f.Name.FullName())
        .RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.CustomerName))       
    ;

    var customer = customerFaker.Generate();
    return customer;
})
.WithName("GetCustomers")
.RequireAuthorization("protected_api")
.WithOpenApi();

app.Run();
