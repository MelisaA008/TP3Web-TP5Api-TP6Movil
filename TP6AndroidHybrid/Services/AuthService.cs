using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using TP5Shared.DTOs;
using TP6AndroidHybrid.Auth;

namespace TP6AndroidHybrid.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly TokenStorage _storage;

    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, TokenStorage storage)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _storage = storage;
    }

    public async Task<bool> Login(LoginRequest dto)
    {
        var response = await _http.PostAsJsonAsync("api/Usuarios/login", dto);
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (result == null) return false;

        await _storage.SetToken(result.Token);
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
        return true;
    }

    public async Task<bool> Register(RegisterRequest dto)
    {
        var response = await _http.PostAsJsonAsync("api/Usuarios/register", dto);
        return response.IsSuccessStatusCode;
    }

    public Task Logout()
    {
        _storage.Clear();
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        return Task.CompletedTask;
    }
}