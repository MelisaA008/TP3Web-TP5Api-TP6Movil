using System.Net.Http.Json;
using TP5Shared.DTOs;

namespace TP5WebApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AuthResponseDto?> Login(LoginRequest request)
        {
            var response =
                await _http.PostAsJsonAsync(
                    "api/Usuarios/login",
                    request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<AuthResponseDto>();
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var response =
                await _http.PostAsJsonAsync(
                    "api/Usuarios/register",
                    request);

            return response.IsSuccessStatusCode;
        }
    }
}