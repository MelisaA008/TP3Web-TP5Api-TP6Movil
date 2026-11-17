namespace TP6AndroidHybrid.Auth;

public class TokenStorage
{
    private const string Clave = "authToken";

    public async Task<string?> GetToken()
    {
        try { return await SecureStorage.Default.GetAsync(Clave); }
        catch { return null; }
    }

    public Task SetToken(string token) => SecureStorage.Default.SetAsync(Clave, token);

    public void Clear() => SecureStorage.Default.Remove(Clave);
}