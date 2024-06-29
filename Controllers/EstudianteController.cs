using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PassHash;

namespace CRUD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudianteController : ControllerBase
{
    private readonly ILogger<EstudianteController> _logger;
    private readonly MyDbContext _db;
    private readonly IConfiguration _configuration;

    public EstudianteController(ILogger<EstudianteController> logger, MyDbContext db, IConfiguration configuration)
    {
        _logger = logger;
        _db = db;
        _configuration = configuration;
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

        if (estudiante.Id <= 0)
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
    
    
    
    
    
    
    //Editar Un estudiante
    [HttpPut("id:int",Name = "EditEstudiantw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Estudiante> EditEstudiante(int id, [FromBody] Estudiante estudiante)
    {

        if (estudiante == null || id != estudiante.Id || id==0)
        {
            _logger.LogInformation("No se puedo obtener el estudiante");
            return BadRequest();
        }

        var objEst = _db.Estudiantes.FirstOrDefault(u => u.Id == id);

        var objCarr = _db.Carreras.Find(estudiante.CarreraId);

        if (objCarr == null)
        {
            _logger.LogInformation("La carrera: "+estudiante.CarreraId+" No fue encontrada");
            return NotFound();
        }
        
        if (objEst == null)
        {
            _logger.LogInformation("El estudiante: "+id+" No fue encontrada");
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Atributos Invalidos");
            ModelState.AddModelError("","Atributos invalidos");
            return BadRequest(ModelState);
        }

        objEst.NombreEstudiante = estudiante.NombreEstudiante;
        objEst.DireccionEstudiante = estudiante.DireccionEstudiante;
        objEst.Nacionalidad = estudiante.Nacionalidad;
        objEst.SexoEstudiante = estudiante.SexoEstudiante;
        
        objEst.CiudadEstudiante = estudiante.CiudadEstudiante;
        objEst.TelefonoEstudiante = estudiante.TelefonoEstudiante;
        objEst.CorreoEstudiante = estudiante.CorreoEstudiante;
        objEst.PeriodosCursados = estudiante.PeriodosCursados;
        
        objEst.IndiceGeneral = estudiante.IndiceGeneral;
        objEst.IndicePeriodo = estudiante.IndicePeriodo;
        objEst.CondicionAcademica = estudiante.CondicionAcademica;
        objEst.CreditosAprobados = estudiante.CreditosAprobados;

        objEst.Alertas = estudiante.Alertas;
        
        objEst.ContraseñaEstudiante = PassHasher.HashPassword(estudiante.DireccionEstudiante);
        objEst.CarreraId = estudiante.CarreraId;
        

        _db.SaveChanges();
        
        _logger.LogInformation("Estudiante con el id: "+id+" editado");
        return NoContent();
    }
    
    
    
    
    //Patch un Estudiante
    [HttpPatch("id:int", Name = "PatchEstudiante")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Estudiante> PatchEstudiante(int id, JsonPatchDocument<Estudiante> patchDocument)
    {

        if (patchDocument == null || id == 0)
        {
            return BadRequest();
        }

        var obj = _db.Estudiantes.FirstOrDefault(u => u.Id == id);

        if (obj == null)
        {  
            _logger.LogInformation("Estudiante No encontrado");
            return NotFound();
        }
        
        patchDocument.ApplyTo(obj,ModelState);

        if (!ModelState.IsValid)
        {
            _logger.LogError("Atributos Invalidos");
            ModelState.AddModelError("","Atributos invalidos");
            return BadRequest(ModelState);
        }

        _db.SaveChanges();
        
        _logger.LogInformation("Estudiante Patcheado");
        return NoContent();
    }
    
    
    
    
    //Delete Un Estudiante
    [HttpDelete("id:int", Name = "DeleteEstudiante")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Estudiante> DeleteEstudiante(int id)
    {

        if (id <= 0)
        {
            _logger.LogError("Error al eliminar el estudiante " +id);
            return BadRequest();

        }

        var obj = _db.Estudiantes.FirstOrDefault(u => u.Id == id);

        if (obj == null)
        {
            return NotFound();

        }

        _db.Estudiantes.Remove(obj);
        _db.SaveChanges();

        return NoContent();

    }
    
    
    

    
    
    
    //Login Estudiantes, and Jwt Authentication Token
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
        
        //JWT CONFIGURATION
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Id", estudianteMatch.Id.ToString()),
            new Claim("CorreoEstudiante", estudianteMatch.CorreoEstudiante)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], 
            _configuration["Jwt:Audience"], 
            claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials:signIn
            );
        
        
        

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("Login exitoso");

        return Ok(new { Token = tokenValue, User = estudianteMatch });

        
    }
}
