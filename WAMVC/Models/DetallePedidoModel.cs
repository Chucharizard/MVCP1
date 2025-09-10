using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class DetallePedidoModel
    {
        public int Id { get; set; }
        public int IdPedido{ get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public PedidoModel Pedido { get; set; }
        public ProductoModel Producto { get; set; }
    }
}
