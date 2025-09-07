using AngularApp.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Server.Data
{
    public class ServerDbContext: DbContext
    {
        public ServerDbContext(DbContextOptions db):base(db)
        {
           
        }
        public DbSet<ClientesModel> Clientes { get; set; }
        public DbSet<UsuariosModel> UsuariosModel { get; set; }
    public DbSet<ProductoModel> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed inicial de Clientes
            modelBuilder.Entity<ClientesModel>().HasData(
                new ClientesModel { Id = 1, Nombres = "Juan", Apellidos = "Pérez", Direccion = "Calle 1 #123", Telefono = "555-1111", Cedula = "001-0000001-1", Correo = "juan.perez@example.com" },
                new ClientesModel { Id = 2, Nombres = "María", Apellidos = "García", Direccion = "Av. Central 45", Telefono = "555-2222", Cedula = "001-0000002-2", Correo = "maria.garcia@example.com" }
            );

            // Seed inicial de Usuarios
            modelBuilder.Entity<UsuariosModel>().HasData(
                new UsuariosModel { Id = 1, nombre = "admin", correo = "admin@example.com", pwd = "admin123" }
            );

            // Seed inicial de Productos
            modelBuilder.Entity<ProductoModel>().HasData(
                new ProductoModel { Id = 1, Codigo = "P-001", Nombre = "Laptop básica", Descripcion = "14'' 8GB RAM", PrecioCompra = 400m, PrecioVenta = 550m, Stock = 10, Activo = true },
                new ProductoModel { Id = 2, Codigo = "P-002", Nombre = "Mouse inalámbrico", Descripcion = "2.4G", PrecioCompra = 5m, PrecioVenta = 12.5m, Stock = 100, Activo = true }
            );
        }
    }
}
