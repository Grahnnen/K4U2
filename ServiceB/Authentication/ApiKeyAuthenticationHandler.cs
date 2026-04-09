using LLMProxy.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace LLMProxy.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IOptionsMonitor<InternalApiKeyOptions> _apiKeyOptions;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IOptionsMonitor<InternalApiKeyOptions> apiKeyOptions)
            : base(options, logger, encoder)
        {
            _apiKeyOptions = apiKeyOptions;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-Internal-Api-Key", out var suppliedKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing API key."));
            }

            var expectedKey = _apiKeyOptions.CurrentValue.Value;

            if (string.IsNullOrWhiteSpace(expectedKey) || suppliedKey != expectedKey)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
            }

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "ContentApi")
        };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}