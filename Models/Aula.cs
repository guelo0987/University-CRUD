using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CRUD.Models
{
    public class Aula
    {
        [Key]
        [RegularExpression(@"^[A-Z]{2}\d{3}$", ErrorMessage = "El código del aula debe tener 2 letras y 3 números, por ejemplo, GC301.")]
        public string CodigoAula { get; set; }

        public int? Capacidad { get; set; }

        [MaxLength(6)]
        public string? TipoAula { get; set; }
        
        
        [JsonIgnore]
        public virtual ICollection<MateriaAula>? MateriaAulas { get; set; }
    }
}