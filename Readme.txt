# TP5 - Sistema de Gestión Comercial

## Descripción

Sistema desarrollado en .NET 10 basado en arquitectura en capas.

Permite administrar:

- Productos
- Categorías
- Proveedores
- Clientes
- Usuarios
- Ingresos de mercadería
- Salidas / ventas
- Control de stock
- Autenticación JWT
- Gestión de roles
- Reportes PDF
- Carga de imágenes

La solución se encuentra dividida en Backend Web API, Biblioteca Compartida, Frontend Blazor WebAssembly Standalone y Frontend Blazor Hybrid  

---

# Estructura de la Solución

```text
SOLUCIÓN
│
├── TP5WebApi
│
├── TP5Shared
│
└── TP5WebAppWasm
│
└── TP6AndroidHybrid
```
# Tecnologías
.NET 10 y ASP.NET Core Web API
Entity Framework Core y SQL Server
JWT y BCrypt
Blazor WebAssembly Standalone
.NET MAUI Blazor Hybrid (Android)
Serilog
QuestPDF y MailKit
Swagger
Bootstrap

---

# Proyecto TP5WebApi

Backend REST desarrollado en ASP.NET Core Web API.

Responsabilidades:

- Exponer endpoints REST
- Validaciones de negocio
- Acceso a datos
- Autenticación JWT
- Autorización por Roles
- Manejo de imágenes
- Generación de reportes

```
TP5WebApi
│
├── Controllers
│   ├── UsuariosController.cs
│   ├── ProductosController.cs
│   ├── CategoriasController.cs
│   ├── ClientesController.cs
│   ├── ProveedoresController.cs
│   ├── IngresosController.cs
│   ├── SalidasController.cs
│   └── ReportesController.cs
│
├── Services
│   ├── JwtService.cs
│   ├── ProductoService.cs
│   ├── CategoriaService.cs
│   ├── ClienteService.cs
│   ├── ProveedorService.cs
│   ├── IngresoService.cs
│   ├── SalidaService.cs
│   └── UsuarioService.cs
│
├── Repositories
│
├── Models
│
├── Data
│   └── DataContext.cs
│
├── Migrations
│
├── wwwroot
│   └── uploads
│
├── Program.cs
│
└── appsettings.json
```

La arquitectura utilizada separa Controllers, Services, Repositories y acceso a datos mediante Entity Framework Core. 

---

# Proyecto TP5Shared

Biblioteca compartida entre API y Frontend.

Contiene DTOs y modelos reutilizables.

```text
TP5Shared
│
├── DTOs
│   ├── AuthResponseDto.cs
│   ├── LoginRequest.cs
│   ├── RegisterRequest.cs
│   ├── ProductoDto.cs
│   ├── CategoriaDto.cs
│   ├── ClienteDto.cs
│   ├── ProveedorDto.cs
│   ├── IngresoDto.cs
│   ├── SalidaDto.cs
│   ├── ParametrosPaginacionDto.cs
│   └── RespuestaPaginadaDto.cs
│
└── Models
```

Los DTOs permiten compartir contratos entre Backend y Frontend sin duplicar código. 

---

# Proyecto TP5WebAppWasm

Frontend desarrollado con Blazor WebAssembly Standalone.

Responsabilidades:

- Interfaz de usuario
- Consumo de API
- Login
- Gestión de JWT
- Control de acceso
- Reportes
- Carga de imágenes

```
TP5WebAppWasm
│
├── Auth
│   ├── CustomAuthStateProvider.cs
│   └── JwtInterceptorHandler.cs
│
├── Services
│   ├── AuthService.cs
│   ├── ProductoService.cs
│   ├── CategoriaService.cs
│   ├── ClienteService.cs
│   ├── ProveedorService.cs
│   ├── IngresoService.cs
│   ├── SalidaService.cs
│   └── UsuarioService.cs
│
├── Pages
│   ├── Index.razor
│   ├── Login.razor
│   ├── Register.razor
│   ├── Productos.razor
│   ├── Categorias.razor
│   ├── Clientes.razor
│   ├── Proveedores.razor
│   ├── Ingresos.razor
│   ├── Salidas.razor
│   ├── Reportes.razor
│   └── SubirImagen.razor
│
├── Shared
│   ├── MainLayout.razor
│   └── NavMenu.razor
│
├── wwwroot
│   ├── css
│   ├── images
│   └── index.html
│
├── App.razor
├── Program.cs
└── _Imports.razor
```

# Funcionalidades Implementadas

## Seguridad

- Login de usuarios
- Registro de usuarios
- JWT
- Roles
- Autorización por páginas

## Gestión

- Productos
- Categorías
- Clientes
- Proveedores
- Ingresos
- Salidas

## Extras

- Paginación
- Filtros
- Carga de imágenes
- Reportes PDF
- Auditoría de errores


---

# Flujo General

```text
TP5WebAppWasm
        │
        ▼
     HttpClient
        │
        ▼
     TP5WebApi
        │
        ▼
      Services
        │
        ▼
    Repositories
        │
        ▼
    SQL Server
```
TP6AndroidHybrid

Frontend móvil en .NET MAUI Blazor Hybrid para Android. Ofrece un catálogo de productos con carrito de compras.

text
TP6AndroidHybrid
│
├── Config
│   └── ApiConfig.cs               dirección de la API
│
├── Auth
│   ├── TokenStorage.cs            guarda el JWT en SecureStorage
│   ├── CustomAuthStateProvider.cs
│   └── JwtInterceptorHandler.cs
│
├── Services
│
├── Components
│   ├── Routes.razor
│   ├── Layout
│   └── Pages
│       ├── Login.razor, Register.razor
│       ├── Catalogo.razor         catálogo, carrito y pago simulado
│       ├── Ingresos.razor         entradas de mercadería (Admin)
│       ├── Venta.razor
│       └── Reportes.razor
│
├── Platforms/Android
└── MauiProgram.cs

Funcionalidad
Catálogo con búsqueda, filtro por categoría e indicador de stock disponible.
Carrito de compras con control de cantidad según stock.
Compra con pago simulado y ticket en PDF.
Registro de entradas de mercadería para el administrador.
Reportes PDF que se abren en el visor del dispositivo y envío por correo.
Control de acceso por rol.
