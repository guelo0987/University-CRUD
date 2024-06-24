using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models;





public class Materia
{
    
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CodigoMateria { get; set; }
    
    
    [MaxLength(100)]
    public string NombreMateria { get; set; }
    
    
    [MaxLength(50)]
    public string TipoMateria { get; set; }
    
    
    public int CreditosMateria { get; set; }
    
    
    [MaxLength(100)]
    public string AreaAcademica { get; set; }
    
    
    
    [JsonIgnore]
    public virtual ICollection<CarreraMateria>? CarreraMaterias { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<EstudianteMateria>? EstudianteMaterias { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<MateriaDocente>? MateriaDocentes { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<MateriaAula>? MateriaAulas { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<MateriaSeccion>? MateriaSecciones { get; set; }
    
    
    
}