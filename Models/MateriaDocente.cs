using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;

public class MateriaDocente
{
    
    
    
    
    
    public int? DocenteId { get; set; }
    [ForeignKey("DocenteId")]
    public virtual  Docente? Docentes { get; set; }
    
    
    
    public int? CodigoMateria { get; set; }
    [ForeignKey("CodigoMateria")]
    public virtual  Materia? Materias { get; set; }
    
}