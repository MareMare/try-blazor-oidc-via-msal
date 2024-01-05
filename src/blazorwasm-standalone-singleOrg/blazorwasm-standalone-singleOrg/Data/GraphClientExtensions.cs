// NOTE: https://learn.microsoft.com/ja-jp/aspnet/core/blazor/security/webassembly/graph-api?view=aspnetcore-8.0&pivots=graph-sdk-5
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using IAccessTokenProvider = Microsoft.AspNetCore.Components.WebAssembly.Authentication.IAccessTokenProvider;

/// <summary>
/// Adds services and implements methods to use Microsoft Graph SDK.
/// </summary>
internal static class GraphClientExtensions
{
    public static IServiceCollection AddGraphClient(this IServiceCollection services, string? baseUrl, List<string>? scopes)
    {
        if (string.IsNullOrEmpty(baseUrl) || scopes.IsNullOrEmpty())
        {
            return services;
        }

        services.Configure<RemoteAuthenticationOptions<MsalProviderOptions>>(
            options =>
            {
                scopes?.ForEach(scope =>
                {
                    options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
                });
            });

        services.AddScoped<IAuthenticationProvider, GraphAuthenticationProvider>();

        services.AddScoped(sp =>
        {
            return new GraphServiceClient(
                new HttpClient(),
                sp.GetRequiredService<IAuthenticationProvider>(),
                baseUrl);
        });

        return services;
    }

    /// <summary>
    /// Implements IAuthenticationProvider interface.
    /// Tries to get an access token for Microsoft Graph.
    /// </summary>
    private class GraphAuthenticationProvider : IAuthenticationProvider
    {
        private readonly IConfiguration config;

        public GraphAuthenticationProvider(IAccessTokenProvider tokenProvider, IConfiguration config)
        {
            this.TokenProvider = tokenProvider;
            this.config = config;
        }

        public IAccessTokenProvider TokenProvider { get; }

        public async Task AuthenticateRequestAsync(
            RequestInformation request,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
        {
            var result = await this.TokenProvider.RequestAccessToken(
                new AccessTokenRequestOptions
                {
                    Scopes = this.config.GetSection("MicrosoftGraph:Scopes").Get<string[]>()
                });

            if (result.TryGetToken(out var token))
            {
                request.Headers.Add("Authorization", $"{CoreConstants.Headers.Bearer} {token.Value}");
            }
        }
    }
}
