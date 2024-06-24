using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;

public class CarreraMateria
{
    
    
    
    

    public int? CarreraId { get; set; }
    [ForeignKey("CarreraId")]
    public virtual  Carrera? Carreras { get; set; }
    
    

    public string? CodigoMateria { get; set; }
    [ForeignKey("CodigoMateria")]
    public virtual  Materia? Materias { get; set; }
    
}