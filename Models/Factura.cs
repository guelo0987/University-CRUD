using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models;

public class Factura
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FacturaId { get; set; }
        
    public int? IdCuentaPorPagar { get; set; }

    [ForeignKey("IdCuentaPorPagar")]
    public virtual CuentaPorPagar? CuentaPorPagars { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MontoTotalaPagar { get; set; }

    public string Estado { get; set; }
}