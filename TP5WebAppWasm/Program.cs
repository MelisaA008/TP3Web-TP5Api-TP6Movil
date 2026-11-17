using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TP5WebAppWasm;
using TP5WebAppWasm.Auth;
using TP5WebAppWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

//
// LocalStorage.
//
builder.Services.AddBlazoredLocalStorage();

//
// Autorización.
//
builder.Services.AddAuthorizationCore();

// AuthenticationStateProvider.

builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthStateProvider>();

// Interceptor JWT.

builder.Services.AddTransient<JwtInterceptorHandler>();

// Cliente HTTP configurado para la API.

builder.Services.AddHttpClient("WebApi",client =>{client.BaseAddress = new Uri
    ("https://localhost:5001/");}).AddHttpMessageHandler<JwtInterceptorHandler>();

// HttpClient principal.

builder.Services.AddScoped(sp =>sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("WebApi"));

// Servicios.

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<IngresoService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<ReporteService>();
builder.Services.AddScoped<SalidaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<VentaService>();

await builder.Build().RunAsync();
