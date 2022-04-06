using IdentityServer4.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(new IdentityResource[]
    {
        new IdentityResources.OpenId(),
    })
    .AddInMemoryApiScopes(new[]
    {
        new ApiScope("main", "MainScope")
    })
    .AddInMemoryClients(new[]
    {
        new Client
        {
            ClientId = "webapi",
            ClientSecrets = { new Secret("key") },

            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "main" },

            RedirectUris = new List<string> { "http://localhost:5000/swagger/oauth2-redirect.html" },
            AllowedCorsOrigins = new List<string> { "http://localhost:5000" },
            AllowPlainTextPkce = true
        }
    })
    .AddDeveloperSigningCredential();

var app = builder.Build();
app.UseIdentityServer();
app.Run();
