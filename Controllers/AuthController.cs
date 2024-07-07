using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PassHash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, MyDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }

        [HttpPost("Login")]
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

            // Primero, busca en los administradores
            var admin = _db.Admins.SingleOrDefault(u => u.AdminId == loginModel.Id);
            if (admin != null)
            {
                return AuthenticateAdmin(admin, loginModel.Password);
            }

            // Si no es un administrador, busca en los docentes
            var docente = _db.Docentes.SingleOrDefault(u => u.CodigoDocente == loginModel.Id);
            if (docente != null)
            {
                return AuthenticateDocente(docente, loginModel.Password);
            }

            // Si no es un administrador ni un docente, busca en los estudiantes
            var estudiante = _db.Estudiantes.SingleOrDefault(u => u.Id == loginModel.Id);
            if (estudiante != null)
            {
                return AuthenticateEstudiante(estudiante, loginModel.Password);
            }

            _logger.LogError("Credenciales inválidas");
            return Unauthorized("Credenciales inválidas");
        }

        private IActionResult AuthenticateAdmin(Admin admin, string password)
        {
            if (!PassHasher.VerifyPassword(password, admin.ContraseñaAdmin))
            {
                _logger.LogError("Credenciales inválidas");
                return Unauthorized("Credenciales inválidas");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", admin.AdminId.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            };

            return GenerateToken(claims, admin, "Admin");
        }

        private IActionResult AuthenticateDocente(Docente docente, string password)
        {
            if (!PassHasher.VerifyPassword(password, docente.ContraseñaDocente))
            {
                _logger.LogError("Credenciales inválidas");
                return Unauthorized("Credenciales inválidas");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", docente.CodigoDocente.ToString()),
                new Claim(ClaimTypes.Role, "Docente")
            };

            return GenerateToken(claims, docente, "Docente");
        }

        private IActionResult AuthenticateEstudiante(Estudiante estudiante, string password)
        {
            if (!PassHasher.VerifyPassword(password, estudiante.ContraseñaEstudiante))
            {
                _logger.LogError("Credenciales inválidas");
                return Unauthorized("Credenciales inválidas");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", estudiante.Id.ToString()),
                new Claim(ClaimTypes.Role, "Estudiante")
            };

            // Obtener el nombre de la carrera
            var nombreCarrera = _db.Carreras
                .Where(c => c.CarreraId == estudiante.CarreraId)
                .Select(c => c.NombreCarrera)
                .FirstOrDefault();

            // Crear un objeto anónimo con los datos del estudiante y el nombre de la carrera
            var estudianteData = new
            {
                estudiante.Id,
                estudiante.NombreEstudiante,
                estudiante.DireccionEstudiante,
                estudiante.Nacionalidad,
                estudiante.SexoEstudiante,
                estudiante.CiudadEstudiante,
                estudiante.TelefonoEstudiante,
                estudiante.CorreoEstudiante,
                estudiante.FechaIngreso,
                estudiante.PeriodosCursados,
                estudiante.IndiceGeneral,
                estudiante.IndicePeriodo,
                estudiante.PeriodoActual,
                estudiante.CondicionAcademica,
                estudiante.CreditosAprobados,
                estudiante.CarreraId,
                NombreCarrera = nombreCarrera,
                estudiante.Alertas,
                estudiante.Rol
            };

            return GenerateToken(claims, estudianteData, "Estudiante");
        }

        private IActionResult GenerateToken(Claim[] claims, object userData, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Login exitoso");

            return Ok(new { Token = tokenValue, User = userData, Role = role });
        }

        [HttpGet("GetCurrentUser")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            var userId = User.FindFirst("Id")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null || role == null)
            {
                _logger.LogError("Usuario no autenticado");
                return Unauthorized();
            }

            if (role == "Admin")
            {
                var admin = _db.Admins.SingleOrDefault(u => u.AdminId.ToString() == userId);
                if (admin == null)
                {
                    _logger.LogError($"Administrador con ID {userId} no encontrado");
                    return NotFound();
                }
                return Ok(admin);
            }
            else if (role == "Docente")
            {
                var docente = _db.Docentes.SingleOrDefault(u => u.CodigoDocente.ToString() == userId);
                if (docente == null)
                {
                    _logger.LogError($"Docente con ID {userId} no encontrado");
                    return NotFound();
                }
                return Ok(docente);
            }
            else if (role == "Estudiante")
            {
                var estudiante = _db.Estudiantes
                    .Include(e => e.Carreras)
                    .Where(u => u.Id.ToString() == userId)
                    .Select(e => new {
                        e.Id,
                        e.NombreEstudiante,
                        e.DireccionEstudiante,
                        e.Nacionalidad,
                        e.SexoEstudiante,
                        e.CiudadEstudiante,
                        e.TelefonoEstudiante,
                        e.CorreoEstudiante,
                        e.FechaIngreso,
                        e.PeriodosCursados,
                        e.IndiceGeneral,
                        e.IndicePeriodo,
                        e.PeriodoActual,
                        e.CondicionAcademica,
                        e.CreditosAprobados,
                        e.CarreraId,
                        NombreCarrera = _db.Carreras
                            .Where(c => c.CarreraId == e.CarreraId)
                            .Select(c => c.NombreCarrera)
                            .FirstOrDefault(),
                        e.Alertas,
                        e.Rol
                    })
                    .SingleOrDefault();

                if (estudiante == null)
                {
                    _logger.LogError($"Estudiante con ID {userId} no encontrado");
                    return NotFound();
                }
                return Ok(estudiante);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
