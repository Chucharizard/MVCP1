using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdCliente { get; set; }
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El monto total es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El monto total debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Display(Name = "Monto Total")]
        public decimal MontoTotal { get; set; }


        public ClienteModel Cliente { get; set; }//Un pedido pertenece a un solo cliente.

        public ICollection<DetallePedidoModel> DetallePedidos { get; set; }// Un pedido puede tener muchos detalles de pedido.
    }
}
