using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PassHash;

namespace CRUD.Controllers
{
    [Route("api/DocenteApi")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<DocenteController> _logger;
        private readonly IConfiguration _configuration;

        public DocenteController(ILogger<DocenteController> logger, MyDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }

        // Crear un Docente
        [HttpPost("CreateDocente", Name = "CreateDocente")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "RequireAdministratorRole")]
        public ActionResult<Docente> CreateDocente([FromBody] Docente docente)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear el docente: Modelo no válido");
                return BadRequest(ModelState);
            }

            if (_db.Docentes.Any(u => u.CorreoDocente.ToLower() == docente.CorreoDocente.ToLower()))
            {
                _logger.LogError("Error al crear el docente: El docente ya existe");
                ModelState.AddModelError("", "El docente ya existe");
                return BadRequest(ModelState);
            }


            if (docente.CodigoDocente <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            docente.ContraseñaDocente = PassHasher.HashPassword(docente.ContraseñaDocente);

            _db.Docentes.Add(docente);
            _db.SaveChanges();
            _logger.LogInformation("Docente creado exitosamente");

            return CreatedAtRoute("GetDocente", new { id = docente.CodigoDocente }, docente);
        }

        // Obtener Docentes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireAdministratorRole")]
        public ActionResult<IEnumerable<Docente>> GetDocentes()
        {
            _logger.LogInformation("Obteniendo Docentes");
            return Ok(_db.Docentes.ToList());
        }

        // Obtener un Docente
        [HttpGet("{id:int}", Name = "GetDocente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "RequireAdministratorRole")]
        public ActionResult<Docente> GetDocente(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var obj = _db.Docentes.FirstOrDefault(u => u.CodigoDocente == id);

            if (obj == null)
            {
                _logger.LogInformation("Docente con el id: " + id + " no encontrado");
                return NotFound();
            }

            _logger.LogInformation("Docente " + id + " encontrado");
            return Ok(obj);
        }

        // Editar un Docente
        [HttpPut("{id:int}", Name = "EditDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "RequireAdministratorRole")]
        public ActionResult EditDocente(int id, [FromBody] Docente docente)
        {
            if (docente == null || id != docente.CodigoDocente)
            {
                _logger.LogInformation("No se pudo obtener el docente");
                return BadRequest();
            }

            var obj = _db.Docentes.FirstOrDefault(u => u.CodigoDocente == id);

            if (obj == null)
            {
                _logger.LogInformation("El docente con id: " + id + " no fue encontrado");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos inválidos");
                ModelState.AddModelError("", "Atributos inválidos");
                return BadRequest(ModelState);
            }

            obj.NombreDocente = docente.NombreDocente;
            obj.CorreoDocente = docente.CorreoDocente;
            obj.ContraseñaDocente = docente.ContraseñaDocente;
            obj.TelefonoDocente = docente.TelefonoDocente;
            obj.SexoDocente = docente.SexoDocente;

            _db.SaveChanges();
            _logger.LogInformation("Docente con el id: " + id + " editado");
            return NoContent();
        }

        // Patch un Docente
        [HttpPatch("{id:int}", Name = "PatchDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "RequireAdministratorRole")]
        public ActionResult PatchDocente(int id, JsonPatchDocument<Docente> patchDocument)
        {
            if (patchDocument == null || id == 0)
            {
                return BadRequest();
            }

            var obj = _db.Docentes.FirstOrDefault(u => u.CodigoDocente == id);

            if (obj == null)
            {
                _logger.LogInformation("Docente no encontrado");
                return NotFound();
            }

            patchDocument.ApplyTo(obj, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos inválidos");
                ModelState.AddModelError("", "Atributos inválidos");
                return BadRequest(ModelState);
            }

            _db.SaveChanges();
            _logger.LogInformation("Docente parcheado");
            return NoContent();
        }

        // Eliminar un Docente
        [HttpDelete("{id:int}", Name = "DeleteDocente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "RequireAdministratorRole")]
        public ActionResult DeleteDocente(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Error al eliminar el docente con id: " + id);
                return BadRequest();
            }

            var obj = _db.Docentes.FirstOrDefault(u => u.CodigoDocente == id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Docentes.Remove(obj);
            _db.SaveChanges();
            _logger.LogInformation("Docente con id: " + id + " eliminado");

            return NoContent();
        }


        // Login Docentes and Jwt Authentication Token
        [HttpPost("LoginDocente", Name = "LoginDocente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrWhiteSpace(loginModel.Id.ToString()) ||
                string.IsNullOrWhiteSpace(loginModel.Password))
            {
                _logger.LogError("Datos de login inválidos");
                return BadRequest("Datos de login inválidos");
            }

            var docenteMatch = _db.Docentes.SingleOrDefault(u => u.CodigoDocente == loginModel.Id);

            if (docenteMatch == null)
            {
                _logger.LogError("Credenciales inválidas");
                return Unauthorized("Credenciales inválidas");
            }

            if (!PassHasher.VerifyPassword(loginModel.Password, docenteMatch.ContraseñaDocente))
            {
                _logger.LogError("Credenciales inválidas");
                return Unauthorized("Credenciales inválidas");
            }

            // JWT CONFIGURATION
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", docenteMatch.CodigoDocente.ToString()),
                new Claim("CorreoDocente", docenteMatch.CorreoDocente),
                new Claim(ClaimTypes.Role, "Docente")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Login exitoso");

            return Ok(new { Token = tokenValue, User = docenteMatch });
        }
    }
}

