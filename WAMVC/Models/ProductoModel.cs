using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAMVC.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }

        [Display(Name = "Título del Artículo")]
        [Required(ErrorMessage = "Por favor, ingresa el nombre del artículo")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "El título debe contener entre 2 y 80 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Detalles del Producto")]
        [StringLength(800, ErrorMessage = "La descripción no puede exceder los 800 caracteres")]
        public string? Descripcion { get; set; }

        [Display(Name = "Valor Unitario")]
        [Required(ErrorMessage = "Debes especificar un precio válido")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.50, 99999.99, ErrorMessage = "El precio debe estar entre $0.50 y $99,999.99")]
        public decimal Precio { get; set; }

        [Display(Name = "Existencias Disponibles")]
        [Range(0, 50000, ErrorMessage = "Las existencias deben estar entre 0 y 50,000 unidades")]
        public int Stock { get; set; }

        //Propiedad de navegación para la relación con DetallePedido
        //Un producto puede estar en muchos detalles de pedido 1-N
        public ICollection<DetallePedidoModel>? DetallePedidos { get; set; }
    }
}
