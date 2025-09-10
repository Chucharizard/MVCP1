using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdCliente { get; set; }
        public string Direccion { get; set; }
        public decimal MontoTotal { get; set; }
        
        public ClienteModel Cliente { get; set; }//Un pedido pertenece a un solo cliente.

        public ICollection<DetallePedidoModel> DetallePedidos { get; set; }// Un pedido puede tener muchos detalles de pedido.
    }
}
