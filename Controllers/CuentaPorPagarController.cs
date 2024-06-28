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
        var obj = _db.CuentaPorPagars.Include(es => es.EstudianteMateria)
            .Where(c => c.EstudianteMateria.PeriodoCursado == periodo && c.CodigoEstudiante == codigoEstudiante)
            .Select(c => new {
                c.IdCuentaPorPagar,
                c.CodigoMateria,
                c.CodigoEstudiante,
                c.MontoTotalaPagar,
                Materia = c.EstudianteMateria.Materias.NombreMateria, // Assuming Materia has a Nombre property
                Periodo = c.EstudianteMateria.PeriodoCursado
            });

        if (!obj.Any())
        {
            return NotFound("No se encontraron cuentas por pagar para el estudiante en el periodo especificado.");
        }

        return Ok(obj);
    }

    
    
    
    
    
    
    
}