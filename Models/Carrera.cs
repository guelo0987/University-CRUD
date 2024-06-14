using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace CRUD.Models;

public  class Carrera
{
    public int CarreraId { get; set; }

    public string NombreCarrera { get; set; } = null!;

    public string DuracionPeriodos { get; set; } = null!;

    public string TotalCreditos { get; set; } = null!;

    public string Departamento { get; set; } = null!;
    
    [JsonIgnore]
    public  ICollection<Estudiante>? Estudiantes { get; set; } = new List<Estudiante>();
}
