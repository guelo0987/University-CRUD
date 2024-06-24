using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models
{
    public class Aula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoAula { get; set; }

        public int Capacidad { get; set; }

        [MaxLength(50)]
        public string TipoAula { get; set; }
        
        
        [JsonIgnore]
        public virtual ICollection<MateriaAula>? MateriaAulas { get; set; }
    }
}