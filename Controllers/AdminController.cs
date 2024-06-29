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

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger, MyDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }
        
        
        
        
          // Crear un Administrador
        [HttpPost("CreateAdmin", Name = "CreateAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Admin> CreateAdmin([FromBody] Admin admin)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear el administrador: Modelo no válido");
                return BadRequest(ModelState);
            }

            if (_db.Admins.Any(a => a.CorreoAdmin.ToLower() == admin.CorreoAdmin.ToLower()))
            {
                _logger.LogError("Error al crear el administrador: El correo ya existe");
                ModelState.AddModelError("", "El correo ya existe");
                return BadRequest(ModelState);
            }

            if (_db.Admins.Any(a => a.AdminId == admin.AdminId))
            {
                _logger.LogError("Error al crear el administrador: El ID de administrador ya existe");
                ModelState.AddModelError("", "El ID de administrador ya existe");
                return BadRequest(ModelState);
            }

            if (admin.AdminId <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            admin.ContraseñaAdmin = PassHasher.HashPassword(admin.ContraseñaAdmin);

            _db.Admins.Add(admin);
            _db.SaveChanges();
            _logger.LogInformation("Administrador creado exitosamente");

            return CreatedAtRoute("GetAdmin", new { id = admin.AdminId }, admin);
        }

        // Método para obtener un administrador por ID (opcional, para que CreatedAtRoute funcione)
        [HttpGet("{id}", Name = "GetAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Admin> GetAdmin(int id)
        {
            var admin = _db.Admins.Find(id);
            if (admin == null)
            {
                _logger.LogError($"Administrador con ID {id} no encontrado");
                return NotFound();
            }
            return Ok(admin);
        }
        
        
        
        

        [HttpPost("Login", Name = "LoginAdmin")]
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

            var adminMatch = _db.Admins.SingleOrDefault(u => u.AdminId == loginModel.Id);

            if (adminMatch == null)
            {
                _logger.LogError("Credenciales inválidas");
                return Unauthorized("Credenciales inválidas");
            }

            if (!PassHasher.VerifyPassword(loginModel.Password, adminMatch.ContraseñaAdmin))
            {
                _logger.LogError("Credenciales inválidas");
                return Unauthorized("Credenciales inválidas");
            }

            // JWT CONFIGURATION
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", adminMatch.AdminId.ToString()),
                new Claim("CorreoAdmin", adminMatch.CorreoAdmin),
                new Claim(ClaimTypes.Role, adminMatch.Rol) // Agregar rol al token
            };

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

            return Ok(new { Token = tokenValue, User = adminMatch });
        }
    }
}
