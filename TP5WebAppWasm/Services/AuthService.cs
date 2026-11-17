using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TP5Shared.DTOs;
using TP5WebAppWasm.Auth;

namespace TP5WebAppWasm.Services;

public class AuthService
{
    private readonly HttpClient _http;

    private readonly AuthenticationStateProvider
        _authStateProvider;

    private readonly ILocalStorageService
        _localStorage;

    public AuthService(
        HttpClient http,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    /// <summary>
    /// Login contra la API.
    /// </summary>
    public async Task<bool> Login(
        LoginRequest dto)
    {

        Console.WriteLine($"BASE URL = {_http.BaseAddress}");
        Console.WriteLine("INTENTANDO LOGIN...");


        var response =
            await _http.PostAsJsonAsync(
                "api/Usuarios/login",
                dto);

        if (!response.IsSuccessStatusCode)
            return false;

        var result =
            await response.Content
                .ReadFromJsonAsync<AuthResponseDto>();

        if (result == null)
            return false;

        // Guardar JWT.
        await _localStorage
            .SetItemAsync(
                "authToken",
                result.Token);

        // Actualizar estado.
        ((CustomAuthStateProvider)
            _authStateProvider)
            .NotifyUserAuthentication(
                result.Token);

        return true;
    }

    /// <summary>
    /// Registro de usuario.
    /// </summary>
    public async Task<bool> Register(
        RegisterRequest dto)
    {
        var response =
            await _http.PostAsJsonAsync(
                "api/Usuarios/register",
                dto);

        return response
            .IsSuccessStatusCode;
    }

    /// <summary>
    /// Logout.
    /// </summary>
    public async Task Logout()
    {
        await _localStorage
            .RemoveItemAsync(
                "authToken");

        ((CustomAuthStateProvider)
            _authStateProvider)
            .NotifyUserLogout();
    }

    /// <summary>
    /// Obtener JWT actual.
    /// </summary>
    public async Task<string?> GetToken()
    {
        return await _localStorage
            .GetItemAsync<string>(
                "authToken");
    }

    /// <summary>
    /// Indica si existe token.
    /// </summary>
    public async Task<bool>
        IsAuthenticated()
    {
        var token =
            await _localStorage
                .GetItemAsync<string>(
                    "authToken");

        return
            !string.IsNullOrWhiteSpace(
                token);
    }
}