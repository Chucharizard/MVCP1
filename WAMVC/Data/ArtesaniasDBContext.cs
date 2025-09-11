using Microsoft.EntityFrameworkCore;
using WAMVC.Models;

namespace WAMVC.Data
{
    public class ArtesaniasDBContext : DbContext
    {
        public ArtesaniasDBContext(DbContextOptions<ArtesaniasDBContext> options) : base(options)
        {
        }

        public DbSet<ProductoModel> Productos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<DetallePedidoModel> DetallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraci�n de precisi�n para campos decimales
            modelBuilder.Entity<ProductoModel>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PedidoModel>()
                .Property(p => p.MontoTotal)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DetallePedidoModel>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(10, 2);

            // Configuraci�n de longitudes para campos de texto
            modelBuilder.Entity<ClienteModel>()
                .Property(c => c.Nombre)
                .HasMaxLength(100);

            modelBuilder.Entity<ClienteModel>()
                .Property(c => c.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<ClienteModel>()
                .Property(c => c.Direccion)
                .HasMaxLength(200);

            modelBuilder.Entity<ProductoModel>()
                .Property(p => p.Nombre)
                .HasMaxLength(100);

            modelBuilder.Entity<ProductoModel>()
                .Property(p => p.Descripcion)
                .HasMaxLength(500);

            // Configuraci�n de relaciones
            modelBuilder.Entity<PedidoModel>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetallePedidoModel>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetallePedidoModel>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}