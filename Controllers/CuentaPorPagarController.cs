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


    
    
    [HttpGet("periodo")]
    public IActionResult GetCuentaPorPagar(string periodo)
    {



        var obj = _db.CuentaPorPagars.Include(es => es.EstudianteMateria)
            .Where(c => c.EstudianteMateria.PeriodoCursado == periodo);
        
        

        return Ok(obj);
    }
    
    
    
    
    
    
    
}