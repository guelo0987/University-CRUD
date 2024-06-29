using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(555555,999999)]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreAdmin { get; set; }

        [Required]
        [MaxLength(100)]
        public string CorreoAdmin { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContraseñaAdmin { get; set; }

        [Required]
        [MaxLength(50)]
        public string Rol { get; set; } = "Administrador";
    }
}