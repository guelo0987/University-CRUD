using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRUD.Models;
using Newtonsoft.Json;

public  class Estudiante
{
    [Key]
    [Range(1111111, 1999999)] // Establecer el rango para el Id
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

    [MaxLength(150)]
     public string CorreoEstudiante { get; set; } = null!;

    public DateTime FechaIngreso { get; set; } = DateTime.Now;

    public int? PeriodosCursados { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? IndiceGeneral { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? IndicePeriodo { get; set; }
    
    
    public string? PeriodoActual { get; set; }

    [MaxLength(50)]
    public string CondicionAcademica { get; set; } = null!;

    public int? CreditosAprobados { get; set; }

    
    [MaxLength(100)]
    public string ContraseñaEstudiante { get; set; } = null!;

    public int? CarreraId { get; set; }
    
    [ForeignKey("CarreraId")]
    public virtual Carrera? Carreras { get; set; }
    
    [MaxLength(50)]
    public string? Alertas { get; set; }


    public string Rol { get; set; } = "Estudiante";
    
    
    [JsonIgnore]
    public virtual ICollection<CarreraMateria>? CarreraMaterias { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<EstudianteMateria>? EstudianteMaterias { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Factura>? Facturas { get; set; }
}