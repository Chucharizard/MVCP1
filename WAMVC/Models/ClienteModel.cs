using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es v�lido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La direcci�n es obligatoria")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "La direcci�n debe tener entre 5 y 200 caracteres")]
        [Display(Name = "Direcci�n completa")] 
        public string Direccion { get; set; }

        public ICollection<PedidoModel> Pedidos { get; set; }
    }
}

