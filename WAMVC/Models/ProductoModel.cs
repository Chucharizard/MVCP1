using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class ProductoModel
    {
        //No se olviden es la llave primaria de la tabla
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        // Propiedad de navegación para la relación con DetallePedido.
        // Un producto puede estar en muchos detalles de pedido.
        public ICollection<DetallePedidoModel> DetallePedidos { get; set; }

    }
}
