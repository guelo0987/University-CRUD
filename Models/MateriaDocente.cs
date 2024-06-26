using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;

public class MateriaDocente
{
    
    
    
    
    
    public int? DocenteId { get; set; }
    [ForeignKey("DocenteId")]
    public virtual  Docente? Docentes { get; set; }
    
    public string? SeccionId { get; set; }
    [ForeignKey("SeccionId")]
    public virtual Seccion?  Seccions { get; set; }
    
    public string? CodigoMateria { get; set; }
    [ForeignKey("CodigoMateria")]
    public virtual  Materia? Materias { get; set; }
    
}