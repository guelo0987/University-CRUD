using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class FacturaController : ControllerBase
{
    private readonly MyDbContext _db;
    private readonly ILogger<FacturaController> _logger;

    public FacturaController(MyDbContext db, ILogger<FacturaController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpPost("generarFactura")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GenerarFactura(string periodo, int codigoEstudiante)
    {
        var cuentasPorPagar = _db.CuentaPorPagars
            .Include(cp => cp.EstudianteMateria)
            .ThenInclude(em => em.Materias)
            .Where(c => c.EstudianteMateria.PeriodoCursado == periodo && c.CodigoEstudiante == codigoEstudiante)
            .ToList();

        if (!cuentasPorPagar.Any())
        {
            return NotFound("No se encontraron cuentas por pagar para el estudiante en el periodo especificado.");
        }

        decimal montoTotal = cuentasPorPagar.Sum(c => c.MontoTotalaPagar ?? 0);

        var nuevaFactura = new Factura
        {
            CodigoEstudiante = codigoEstudiante,
            Periodo = periodo,
            MontoTotal = montoTotal,
            FechaCreacion = DateTime.UtcNow,
            Estado = "Pendiente"
        };

        _db.Facturas.Add(nuevaFactura);
        _db.SaveChanges();

        _logger.LogInformation("Factura creada exitosamente");

        return Ok();
    }
}