using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using TP6AndroidHybrid.Auth;
using TP6AndroidHybrid.Config;
using TP6AndroidHybrid.Services;

namespace TP6AndroidHybrid;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddAuthorizationCore();
        builder.Services.AddSingleton<TokenStorage>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

        builder.Services.AddScoped(sp => new HttpClient(
            new JwtInterceptorHandler(sp.GetRequiredService<TokenStorage>())
            {
                InnerHandler = new HttpClientHandler()
            })
        {
            BaseAddress = new Uri(ApiConfig.BaseUrl)
        });

        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<ProductoService>();
        builder.Services.AddScoped<CategoriaService>();
        builder.Services.AddScoped<ClienteService>();
        builder.Services.AddScoped<ProveedorService>();
        builder.Services.AddScoped<IngresoService>();
        builder.Services.AddScoped<VentaService>();
        builder.Services.AddScoped<ReporteService>();

        return builder.Build();
    }
}