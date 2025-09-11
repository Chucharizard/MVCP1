using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAMVC.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "Necesitamos tu nombre completo")]
        [StringLength(120, MinimumLength = 2, ErrorMessage = "Tu nombre debe tener al menos 2 caracteres y m�ximo 120")]
        [RegularExpression(@"^[a-zA-Z������������\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; }

        [Display(Name = "Correo de Contacto")]
        [Required(ErrorMessage = "Tu email es necesario para contactarte")]
        [EmailAddress(ErrorMessage = "Por favor verifica que tu correo sea v�lido")]
        [StringLength(150, ErrorMessage = "El correo no puede tener m�s de 150 caracteres")]
        public string Email { get; set; }

        [Display(Name = "Direcci�n de Env�o")]
        [Required(ErrorMessage = "Necesitamos una direcci�n para hacer el env�o")]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "La direcci�n debe ser m�s espec�fica (m�nimo 10 caracteres, m�ximo 300)")]
        public string Direccion { get; set; }

        public ICollection<PedidoModel>? Pedidos { get; set; }
    }
}