using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAMVC.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }

        [Display(Name = "Cliente Asociado")]
        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cliente válido")]
        public int IdCliente { get; set; }

        [Display(Name = "Fecha de Solicitud")]
        [Required(ErrorMessage = "La fecha de solicitud es requerida")]
        [DataType(DataType.DateTime)]
        public DateTime FechaPedido { get; set; }

        [Display(Name = "Destino de Entrega")]
        [Required(ErrorMessage = "Especifica dónde realizar la entrega")]
        [StringLength(400, MinimumLength = 15, ErrorMessage = "El destino debe tener entre 15 y 400 caracteres")]
        public string Direccion { get; set; }

        [Display(Name = "Total a Pagar")]
        [Required(ErrorMessage = "El total debe calcularse")]
        [Column(TypeName = "decimal(12,2)")]
        [Range(1.00, 999999.99, ErrorMessage = "El total debe estar entre $1.00 y $999,999.99")]
        public decimal MontoTotal { get; set; }

        // Propiedades de navegación
        public ClienteModel? Cliente { get; set; }
        public ICollection<DetallePedidoModel>? DetallePedidos { get; set; }
    }
}
