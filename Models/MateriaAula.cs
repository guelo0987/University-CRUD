using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;

public class MateriaAula
{
    
    
    
    
    
    public int? AulaId { get; set; }
    [ForeignKey("AulaId")]
    public virtual  Aula? Aulas { get; set; }
    
    
    
    public int? CodigoMateria { get; set; }
    [ForeignKey("CodigoMateria")]
    public virtual  Materia? Materias { get; set; }
    
}