using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models;





public class Materia
{
    
    
    [Key]
    [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "El código de la materia debe tener 3 letras y 3 números, por ejemplo, MAT123.")]
    public string CodigoMateria { get; set; }
    
    
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
    public virtual ICollection<Seccion>? Seccions { get; set; }
    
    
}