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

La solución se encuentra dividida en Backend Web API, Biblioteca Compartida y Frontend Blazor WebAssembly Standalone. 

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
```

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

```text
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

```text
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

La guía recomienda organizar la aplicación en Pages, Services, Shared y Auth para separar responsabilidades. 

---

# Tecnologías Utilizadas

- .NET 10
- ASP.NET Core Web API
- Blazor WebAssembly
- SQL Server
- Entity Framework Core
- JWT Authentication
- Bootstrap
- Swagger
- LocalStorage
- HttpClient
- QuestPDF



---

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

