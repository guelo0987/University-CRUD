using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models
{
    public class Seccion
    {
        [Key] 
        [Range(1,10)]
        public string CodigoSeccion { get; set; }

        [MaxLength(50)]
        public string Horario { get; set; }

        public int Cupo { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<MateriaSeccion>? MateriaSecciones { get; set; }
    }
}