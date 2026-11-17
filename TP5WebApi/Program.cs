using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using QuestPDF.Infrastructure;
using Serilog;
using System.Text;
using TP5Shared.Models;
using TP5WebApi.Repositories;
using TP5WebApi.Services;

// Licencia gratuita de QuestPDF (reportes PDF)
QuestPDF.Settings.License = LicenseType.Community;

//serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

//serilog
builder.Host.UseSerilog(
    (context, services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

// -------------------- Servicios --------------------
// Servicios de negocio
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<IngresoService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<VentaService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<SalidaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ReportePdfService>();
builder.Services.AddScoped<EmailService>();

// Controladores
builder.Services.AddControllers();

// DbContext con SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<IngresoProductoRepository>();
builder.Services.AddScoped<IVentasRepository, VentasRepository>();
builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<ProveedorRepository>();
builder.Services.AddScoped<SalidaProductoRepository>();
builder.Services.AddScoped<UsuarioRepository>();

// CORS permisivo
builder.Services.AddCors(options =>
{
    options.AddPolicy("Permisivo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger / OpenAPI con botón Authorize para el token JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TP5 Web API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Pegá solo el token, sin la palabra Bearer"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Autorización
builder.Services.AddAuthorization();

// -------------------- Pipeline --------------------

var app = builder.Build();

// Manejo global de errores: registra en el log cualquier excepción no controlada
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var ex = feature?.Error;

        if (ex != null)
        {
            Log.Error(ex, "Error no controlado en {Method} {Path}", context.Request.Method, context.Request.Path);
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        object respuesta = app.Environment.IsDevelopment() && ex != null
            ? new { error = "Error interno", detalle = ex.ToString() }
            : new { error = "Ocurrió un error interno. Quedó registrado en el log del sistema." };

        await context.Response.WriteAsJsonAsync(respuesta);
    });
});

//serilog
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} respondió {StatusCode} en {Elapsed:0.0000} ms";
});

// Swagger disponible también en el hosting
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles(); //imagenes

app.UseCors("Permisivo");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}