using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]

        public string Password { get; set; }

        [Required]
        public string Rol { get; set; }

        [Required]
        public bool Activo { get; set; }
    }
}
