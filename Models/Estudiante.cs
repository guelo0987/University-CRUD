using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRUD.Models;

public  class Estudiante
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)]
    public string NombreEstudiante { get; set; } = null!;

    [MaxLength(200)]
    public string DireccionEstudiante { get; set; } = null!;

    [MaxLength(50)]
    public string Nacionalidad { get; set; } = null!;

    [MaxLength(1)]
    public string SexoEstudiante { get; set; } = null!;

    [MaxLength(100)]
    public string CiudadEstudiante { get; set; } = null!;

    [MaxLength(15)]
    public string TelefonoEstudiante { get; set; } = null!;

    [MaxLength(100)]
     public string CorreoEstudiante { get; set; } = null!;

    public DateTime FechaIngreso { get; set; } = DateTime.Now;

    public int? PeriodosCursados { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? IndiceGeneral { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? IndicePeriodo { get; set; }

    [MaxLength(50)]
    public string CondicionAcademica { get; set; } = null!;

    public int? CreditosAprobados { get; set; }

    [MaxLength(10)]
    public string? CodigoCarrera { get; set; }

    [MaxLength(100)]
    public string ContraseñaEstudiante { get; set; } = null!;

    public int CarreraId { get; set; }
    
    [ForeignKey("CarreraId")]
    public Carrera? Carreras { get; set; }
}