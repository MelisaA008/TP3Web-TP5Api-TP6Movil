using Microsoft.EntityFrameworkCore;
using TP5Shared.Models;

namespace TP5WebApi.Data
{
    /// <summary>
    /// Contexto principal de Entity Framework.
    /// Configura tablas, relaciones e índices.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        #region Tablas

        /// <summary>
        /// Usuarios del sistema.
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Productos.
        /// </summary>
        public DbSet<Producto> Productos { get; set; }

        /// <summary>
        /// Categorías de productos.
        /// </summary>
        public DbSet<Categoria> Categorias { get; set; }

        /// <summary>
        /// Proveedores.
        /// </summary>
        public DbSet<Proveedor> Proveedores { get; set; }

        /// <summary>
        /// Clientes.
        /// </summary>
        public DbSet<Cliente> Clientes { get; set; }

        /// <summary>
        /// Movimientos de ingreso de stock.
        /// </summary>
        public DbSet<IngresoProducto> IngresosProductos { get; set; }

        /// <summary>
        /// Movimientos de salida de stock.
        /// </summary>
        public DbSet<SalidaProducto> SalidasProductos { get; set; }

        /// <summary>
        /// Cabecera de ventas.
        /// </summary>
        public DbSet<Venta> Ventas { get; set; }

        /// <summary>
        /// Detalles de ventas.
        /// </summary>
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        #endregion

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            // Siempre ejecutar primero la configuración base
            base.OnModelCreating(modelBuilder);

            #region Precisión decimal

            // Precio de producto:
            // 18 dígitos totales y 2 decimales
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            // Total de la venta
            modelBuilder.Entity<Venta>()
                .Property(v => v.Total)
                .HasPrecision(18, 2);

            // Precio unitario del detalle
            modelBuilder.Entity<DetalleVenta>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            #endregion

            #region Índices únicos

            // No permitir usuarios repetidos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // No permitir emails repetidos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            #endregion

            #region Categoria -> Productos

            // Una categoría puede tener muchos productos
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Proveedor -> Productos

            // Un proveedor puede proveer muchos productos
            modelBuilder.Entity<Proveedor>()
                .HasMany(pv => pv.Productos)
                .WithOne(p => p.Proveedor)
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Cliente -> Salidas

            // Un cliente puede tener múltiples salidas
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.SalidasProductos)
                .WithOne(sp => sp.Cliente)
                .HasForeignKey(sp => sp.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Producto -> Ingresos

            // Un producto puede registrarse en múltiples ingresos
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.IngresosProductos)
                .WithOne(ip => ip.Producto)
                .HasForeignKey(ip => ip.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Producto -> Salidas

            // Un producto puede registrarse en múltiples salidas
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.SalidasProductos)
                .WithOne(sp => sp.Producto)
                .HasForeignKey(sp => sp.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Venta -> Detalles

            // Una venta posee muchos detalles
            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Detalles)
                .WithOne(d => d.Venta)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Detalle -> Producto

            // Cada detalle referencia un producto
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}