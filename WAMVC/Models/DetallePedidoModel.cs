using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAMVC.Models
{
    public class DetallePedidoModel
    {
        public int Id { get; set; }

        [Display(Name = "N�mero de Orden")]
        [Required(ErrorMessage = "Se requiere el n�mero de orden")]
        [Range(1, int.MaxValue, ErrorMessage = "N�mero de orden inv�lido")]
        public int IdPedido { get; set; }

        [Display(Name = "Art�culo Seleccionado")]
        [Required(ErrorMessage = "Debe seleccionar un art�culo")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecci�n de art�culo inv�lida")]
        public int IdProducto { get; set; }

        [Display(Name = "Cantidad Solicitada")]
        [Required(ErrorMessage = "Indica cu�ntas unidades necesitas")]
        [Range(1, 1000, ErrorMessage = "Puedes solicitar entre 1 y 1,000 unidades")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio al Momento")]
        [Required(ErrorMessage = "Se debe registrar el precio vigente")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.10, 50000.00, ErrorMessage = "El precio debe estar entre $0.10 y $50,000.00")]
        public decimal PrecioUnitario { get; set; }

        public PedidoModel? Pedido { get; set; }
        public ProductoModel? Producto { get; set; }
    }
}
