using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAMVC.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "Necesitamos tu nombre completo")]
        [StringLength(120, MinimumLength = 2, ErrorMessage = "Tu nombre debe tener al menos 2 caracteres y máximo 120")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; }

        [Display(Name = "Correo de Contacto")]
        [Required(ErrorMessage = "Tu email es necesario para contactarte")]
        [EmailAddress(ErrorMessage = "Por favor verifica que tu correo sea válido")]
        [StringLength(150, ErrorMessage = "El correo no puede tener más de 150 caracteres")]
        public string Email { get; set; }

        [Display(Name = "Dirección de Envío")]
        [Required(ErrorMessage = "Necesitamos una dirección para hacer el envío")]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "La dirección debe ser más específica (mínimo 10 caracteres, máximo 300)")]
        public string Direccion { get; set; }

        public ICollection<PedidoModel>? Pedidos { get; set; }
    }
}