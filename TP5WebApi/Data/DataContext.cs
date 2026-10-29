namespace TP5WebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<IngresoProducto> IngresosProductos { get; set; }
        public DbSet<SalidaProducto> SalidasProductos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación Categoria 1-N Productos
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Proveedor 1-N Productos
            modelBuilder.Entity<Proveedor>()
                .HasMany(pv => pv.Productos)
                .WithOne(p => p.Proveedor)
                .HasForeignKey(p => p.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Cliente 1-N SalidasProductos
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.SalidasProductos)
                .WithOne(sp => sp.Cliente)
                .HasForeignKey(sp => sp.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Producto 1-N IngresosProductos
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.IngresosProductos)
                .WithOne(ip => ip.Producto)
                .HasForeignKey(ip => ip.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Producto 1-N SalidasProductos
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.SalidasProductos)
                .WithOne(sp => sp.Producto)
                .HasForeignKey(sp => sp.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);
        }
    }
}