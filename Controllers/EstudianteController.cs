using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassHash;

namespace CRUD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudianteController : ControllerBase
{
    private readonly ILogger<EstudianteController> _logger;
    private readonly MyDbContext _db;

    public EstudianteController(ILogger<EstudianteController> logger, MyDbContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    
    
    
    //Crear un Estudiante  
    [HttpPost("CreateEstudiante", Name = "CreateEstudiante")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Estudiante> CreateEstudiante([FromBody] Estudiante estudiante)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Error al crear el estudiante: Modelo no válido");
            return BadRequest(ModelState);
        }
        
      

        if (_db.Estudiantes.Any(u => u.CorreoEstudiante.ToLower() == estudiante.CorreoEstudiante.ToLower()))
        {
            _logger.LogError("Error al crear el estudiante: El correo ya existe");
            ModelState.AddModelError("", "El correo ya existe");
            return BadRequest(ModelState);
        }

   
        var carrera = _db.Carreras.Find(estudiante.CarreraId);
        if (carrera == null)
        {
            _logger.LogError("Error al crear el estudiante: Carrera no encontrada");
            ModelState.AddModelError("", "Carrera no encontrada");
            return BadRequest(ModelState);
        }

        if (estudiante.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        estudiante.ContraseñaEstudiante = PassHasher.HashPassword(estudiante.ContraseñaEstudiante);
        estudiante.Carreras = carrera;

        _db.Estudiantes.Add(estudiante);
        _db.SaveChanges();
        _logger.LogInformation("Estudiante creado exitosamente");

        return CreatedAtRoute("GetEstudiante", new { id = estudiante.Id }, estudiante);
    }
        
    
    
    //Obtener Estudiantes
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Estudiante> GetEstudiantes()
    {
        
        _logger.LogInformation("Obteniendo Estudiantes");

        return Ok(_db.Estudiantes.ToList());
    }
    
    
    
    //Obtener un estudiante
    [HttpGet("{id:int}", Name = "GetEstudiante")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public ActionResult<Estudiante> GetEstudiante(int id)
    {

        if (id <= 0)
        {
            return BadRequest();
        }


        var obj = _db.Estudiantes.FirstOrDefault(u => u.Id == id);

        if (obj == null)
        {
            return NotFound();
        }


        return Ok(obj);

    }
    
    
    
    
    
    
    
    

    
    
    
    //Login Estudiantes
    [HttpPost("LoginEstudiante", Name = "LoginEstudiante")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] LoginModel loginModel)
    {
        if (loginModel == null || string.IsNullOrWhiteSpace(loginModel.Id.ToString()) || string.IsNullOrWhiteSpace(loginModel.Password))
        {
            _logger.LogError("Datos de login inválidos");
            return BadRequest("Datos de login inválidos");
        }

        var estudianteMatch = _db.Estudiantes.SingleOrDefault(u => u.Id == loginModel.Id);

        if (estudianteMatch == null)
        {
            _logger.LogError("Credenciales inválidas");
            return Unauthorized("Credenciales inválidas");
        }

        if (!PassHasher.VerifyPassword(loginModel.Password, estudianteMatch.ContraseñaEstudiante))
        {
            _logger.LogError("Credenciales inválidas");
            return Unauthorized("Credenciales inválidas");
        }

        _logger.LogInformation("Login exitoso");
        return Ok("Login exitoso");
    }
}
