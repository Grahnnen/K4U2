using ContentAPI.Options;
using Microsoft.Extensions.Options;

namespace ContentApi.Clients;

public class ServiceBApiKeyHandler : DelegatingHandler
{
    private readonly IOptions<ServiceBOptions> _options;

    public ServiceBApiKeyHandler(IOptions<ServiceBOptions> options)
    {
        _options = options;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Internal-Api-Key", _options.Value.ApiKey);
        return base.SendAsync(request, cancellationToken);
    }
}