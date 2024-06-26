using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;





public class CuentasPorPagar
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    public string? CodigoMateria { get; set; }
    public int? CodigoEstudiante { get; set; }

    [ForeignKey("CodigoMateria, CodigoEstudiante")]
    public virtual EstudianteMateria? EstudianteMateria { get; set; }

    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Monto { get; set; }
    
    
    
    public string? Estado { get; set; } // Ej: "Pendiente", "Pagado", "Atrasado"

    public DateTime? FechaPago { get; set; }
    
    
    

    
    
    
}