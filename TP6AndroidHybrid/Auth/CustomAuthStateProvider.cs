using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace TP6AndroidHybrid.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly TokenStorage _storage;
    private readonly AuthenticationState _anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public CustomAuthStateProvider(TokenStorage storage) => _storage = storage;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _storage.GetToken();
        if (string.IsNullOrWhiteSpace(token)) return _anonymous;

        var claims = ParseClaimsFromJwt(token).ToList();
        var exp = claims.FirstOrDefault(c => c.Type == "exp");

        if (exp != null &&
            DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.Value)) <= DateTimeOffset.UtcNow)
        {
            _storage.Clear();
            return _anonymous;
        }

        return new AuthenticationState(new ClaimsPrincipal(Crear(claims)));
    }

    public void NotifyUserAuthentication(string token) =>
        NotifyAuthenticationStateChanged(Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(Crear(ParseClaimsFromJwt(token))))));

    public void NotifyUserLogout() =>
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));

    // Indica qué claim es el nombre, para que Identity.Name funcione en el header.
    private static ClaimsIdentity Crear(IEnumerable<Claim> claims) =>
        new(claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1].Replace('-', '+').Replace('_', '/');
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }

        var datos = JsonSerializer.Deserialize<Dictionary<string, object>>(
            Convert.FromBase64String(payload));

        var claims = new List<Claim>();
        if (datos == null) return claims;

        foreach (var kvp in datos)
        {
            var tipo = kvp.Key == "role" ? ClaimTypes.Role : kvp.Key;
            claims.Add(new Claim(tipo, kvp.Value.ToString()!));
        }
        return claims;
    }
}