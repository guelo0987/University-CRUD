using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CRUD.Models;

public class EstudianteMateria
{
    
    public string CodigoMateria { get; set; }
    [ForeignKey("CodigoMateria")]
    public virtual  Materia? Materias { get; set; }
    
    public string? SeccionId { get; set; }
    [ForeignKey("SeccionId")]
    public virtual Seccion?  Seccions { get; set; }
    
    public int CodigoEstudiante { get; set; }
    [ForeignKey("CodigoEstudiante")]
    public virtual Estudiante?  Estudiantes { get; set; }
    
    
    [MaxLength(50)]
    [Required]
    public string PeriodoCursado { get; set; }

    public string CalificacionMedioTermino { get; set; } = null!;
    
    [MaxLength(2)] 
    public string CalificacionFinal { get; set; } = null!;
    
    
    
    [JsonIgnore]
    public virtual ICollection<CuentaPorPagar>? CuentaPorPagars { get; set; }
    
}