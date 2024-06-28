using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;




public class CuentaPorPagar
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    
    public int IdCuentaPorPagar { get; set; }
    
    public string CodigoMateria { get; set; }
    public int CodigoEstudiante { get; set; }

    [ForeignKey("CodigoMateria, CodigoEstudiante")]
    public virtual EstudianteMateria? EstudianteMateria { get; set; }
    
    
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MontoTotalaPagar { get; set; }
    
    
    
    public virtual ICollection<Factura>? Facturas { get; set; }
    
    
    
    
    
    
    
    
}