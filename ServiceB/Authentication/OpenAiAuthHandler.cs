using LLMProxy.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace LLMProxy.Authentication
{
    public class OpenAiAuthHandler : DelegatingHandler
    {
        private readonly IOptions<OpenAiOptions> _options;

        public OpenAiAuthHandler(IOptions<OpenAiOptions> options)
        {
            _options = options;
        }

        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.Value.ApiKey))
            {
                throw new InvalidOperationException("OpenAI:ApiKey saknas i ServiceB.");
            }

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.Value.ApiKey);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
