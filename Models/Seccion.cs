using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models
{
    public class Seccion
    {
        [Key] 
        public string CodigoSeccion { get; set; }
        
        
        public string? CodigoMateria { get; set; }
        [ForeignKey("CodigoMateria")]
        public virtual  Materia? Materias { get; set; }
        
        
        public string? CodigoAula { get; set; }
        [ForeignKey("CodigoAula")]
        public virtual  Aula? Aulas { get; set; }

        [MaxLength(256)]
        public string Horario { get; set; }

        public int Cupo { get; set; }
        
        
        [JsonIgnore]
        public virtual ICollection<EstudianteMateria>? EstudianteMaterias { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<MateriaDocente>? MateriaDocentes { get; set; }
        
        
    

        
        
    }
}