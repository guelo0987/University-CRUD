using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models
{
    public class Docente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1111111, 1999999)]
        public int CodigoDocente { get; set; }

        [MaxLength(100)]
        public string NombreDocente { get; set; }

        [MaxLength(100)]
        public string CorreoDocente { get; set; }

        [MaxLength(20)]
        public string TelefonoDocente { get; set; }

        [MaxLength(1)]
        public string SexoDocente { get; set; }
        
        
        [JsonIgnore]
        public virtual ICollection<MateriaDocente>? MateriaDocentes { get; set; }
    }
}