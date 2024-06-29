using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PassHash;

namespace CRUD.Controllers
{
    [Route("api/Admin")]
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
        [Authorize(Policy = "RequireAdministratorRole")]
        
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
        [Authorize(Policy = "RequireAdministratorRole")]
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
        
        
        
        
        
        
        

        
    }
}
