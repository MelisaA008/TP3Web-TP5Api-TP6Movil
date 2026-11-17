using System.Net.Http.Headers;

namespace TP6AndroidHybrid.Auth;

public class JwtInterceptorHandler : DelegatingHandler
{
    private readonly TokenStorage _storage;

    public JwtInterceptorHandler(TokenStorage storage) => _storage = storage;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _storage.GetToken();

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}