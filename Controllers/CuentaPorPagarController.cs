using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers;


[Route("api/CuentaPorPagar")]
[ApiController]

public class CuentaPorPagarController:ControllerBase

{

    
    private readonly MyDbContext _db;
    private readonly ILogger<CuentaPorPagarController> _logger;
    private readonly IConfiguration _configuration;

    public CuentaPorPagarController(ILogger<CuentaPorPagarController> logger, MyDbContext db, IConfiguration configuration)
    {
        _logger = logger;
        _db = db;
        _configuration = configuration;
    }


    
    
    [HttpGet("cuentaporpagar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCuentaPorPagar(string periodo, int codigoEstudiante)
    {
        var obj = _db.CuentaPorPagars
            .Include(c => c.EstudianteMateria)
            .ThenInclude(em => em.Materias)
            .Include(c => c.EstudianteMateria)
            .ThenInclude(em => em.Seccions)
            .ThenInclude(s => s.MateriaDocentes)
            .ThenInclude(md => md.Docentes)
            .Where(c => c.EstudianteMateria.PeriodoCursado == periodo && c.CodigoEstudiante == codigoEstudiante)
            .Select(c => new {
                c.IdCuentaPorPagar,
                c.CodigoMateria,
                c.CodigoEstudiante,
                c.MontoTotalaPagar,
                NombreMateria = c.EstudianteMateria.Materias.NombreMateria,
                Periodo = c.EstudianteMateria.PeriodoCursado,
                Seccion = c.EstudianteMateria.Seccions.CodigoSeccion,
                Profesor = c.EstudianteMateria.Seccions.MateriaDocentes
                    .FirstOrDefault(md => md.CodigoMateria == c.CodigoMateria).Docentes.NombreDocente,
                Aula = c.EstudianteMateria.Seccions.CodigoAula,
                Horario = c.EstudianteMateria.Seccions.Horario,
                Cupo = c.EstudianteMateria.Seccions.Cupo
            });

        if (!obj.Any())
        {
            return NotFound("No se encontraron cuentas por pagar para el estudiante en el periodo especificado.");
        }

        return Ok(obj);
    }

    
    
    
    
    
    
    
}