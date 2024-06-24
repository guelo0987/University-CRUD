using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;

public class EstudianteMateria
{
    
    
    public string? CodigoMateria { get; set; }
    [ForeignKey("CodigoMateria")]
    public virtual  Materia? Materias { get; set; }
    
    public int? CodigoEstudiante { get; set; }
    [ForeignKey("CodigoEstudiante")]
    public virtual Estudiante?  Estudiantes { get; set; }
    
    public int PeriodoActual { get; set; }

    [MaxLength(2)] 
    public string Calificacion { get; set; } = null!;



}