CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(150) NOT NULL,
    [Contacto] nvarchar(100) NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Proveedores] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(150) NOT NULL,
    [Contacto] nvarchar(100) NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Rol] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Ventas] (
    [Id] int NOT NULL IDENTITY,
    [ClienteId] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Ventas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Ventas_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [Productos] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(150) NOT NULL,
    [Stock] int NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [ImagenPath] nvarchar(max) NULL,
    [CategoriaId] int NOT NULL,
    [ProveedorId] int NOT NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Productos_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Productos_Proveedores_ProveedorId] FOREIGN KEY ([ProveedorId]) REFERENCES [Proveedores] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [DetalleVentas] (
    [Id] int NOT NULL IDENTITY,
    [VentaId] int NOT NULL,
    [ProductoId] int NOT NULL,
    [Cantidad] int NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_DetalleVentas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DetalleVentas_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_DetalleVentas_Ventas_VentaId] FOREIGN KEY ([VentaId]) REFERENCES [Ventas] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [IngresosProductos] (
    [Id] int NOT NULL IDENTITY,
    [ProductoId] int NOT NULL,
    [ProveedorId] int NOT NULL,
    [Cantidad] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    CONSTRAINT [PK_IngresosProductos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IngresosProductos_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_IngresosProductos_Proveedores_ProveedorId] FOREIGN KEY ([ProveedorId]) REFERENCES [Proveedores] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [SalidasProductos] (
    [Id] int NOT NULL IDENTITY,
    [ProductoId] int NOT NULL,
    [ClienteId] int NOT NULL,
    [Cantidad] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    CONSTRAINT [PK_SalidasProductos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalidasProductos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SalidasProductos_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]) ON DELETE CASCADE
);
GO


CREATE INDEX [IX_DetalleVentas_ProductoId] ON [DetalleVentas] ([ProductoId]);
GO


CREATE INDEX [IX_DetalleVentas_VentaId] ON [DetalleVentas] ([VentaId]);
GO


CREATE INDEX [IX_IngresosProductos_ProductoId] ON [IngresosProductos] ([ProductoId]);
GO


CREATE INDEX [IX_IngresosProductos_ProveedorId] ON [IngresosProductos] ([ProveedorId]);
GO


CREATE INDEX [IX_Productos_CategoriaId] ON [Productos] ([CategoriaId]);
GO


CREATE INDEX [IX_Productos_ProveedorId] ON [Productos] ([ProveedorId]);
GO


CREATE INDEX [IX_SalidasProductos_ClienteId] ON [SalidasProductos] ([ClienteId]);
GO


CREATE INDEX [IX_SalidasProductos_ProductoId] ON [SalidasProductos] ([ProductoId]);
GO


CREATE UNIQUE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);
GO


CREATE UNIQUE INDEX [IX_Usuarios_Username] ON [Usuarios] ([Username]);
GO


CREATE INDEX [IX_Ventas_ClienteId] ON [Ventas] ([ClienteId]);
GO


