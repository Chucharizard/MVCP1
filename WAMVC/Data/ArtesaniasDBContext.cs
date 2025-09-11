using Microsoft.EntityFrameworkCore;
using WAMVC.Models;

namespace WAMVC.Data
{
    public class ArtesaniasDBContext : DbContext
    {
        public ArtesaniasDBContext(DbContextOptions<ArtesaniasDBContext> options) : base(options) { }

        public DbSet<ProductoModel> Productos { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<DetallePedidoModel> DetallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de precisión para campos decimales
            modelBuilder.Entity<ProductoModel>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2); // 10 dígitos totales, 2 decimales

            modelBuilder.Entity<PedidoModel>()
                .Property(p => p.MontoTotal)
                .HasPrecision(12, 2); // 12 dígitos totales, 2 decimales

            modelBuilder.Entity<DetallePedidoModel>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(10, 2); // 10 dígitos totales, 2 decimales

            // Configuración de relaciones
            modelBuilder.Entity<PedidoModel>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IdCliente);

            modelBuilder.Entity<PedidoModel>()
                .HasMany(p => p.DetallePedidos)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.IdPedido);

            modelBuilder.Entity<DetallePedidoModel>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto);
        }
    }
}
