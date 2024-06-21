using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/EstudianteMateria")]
    [ApiController]
    public class EstudianteMateriaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<EstudianteMateriaController> _logger;

        public EstudianteMateriaController(ILogger<EstudianteMateriaController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpPost("CreateEstudianteMateria", Name = "CreateEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EstudianteMateria> CreateEstudianteMateria([FromBody] EstudianteMateria estudianteMateria)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la EstudianteMateria: Modelo no válido");
                return BadRequest(ModelState);
            }

            // Verificar si Materia y Estudiante existen
            var materia = _db.Materias.Find(estudianteMateria.CodigoMateria);
            var estudiante = _db.Estudiantes.Find(estudianteMateria.CodigoEstudiante);

            if (materia == null || estudiante == null)
            {
                _logger.LogError("Error al crear la EstudianteMateria: Materia o Estudiante no existen");
                return BadRequest("Materia o Estudiante no existen");
            }

            // Verificar si la relación ya existe
            var existingEstudianteMateria = _db.EstudianteMaterias
                .FirstOrDefault(em => em.CodigoMateria == estudianteMateria.CodigoMateria && em.CodigoEstudiante == estudianteMateria.CodigoEstudiante);

            if (existingEstudianteMateria != null)
            {
                _logger.LogError("Error al crear la EstudianteMateria: La relación ya existe");
                ModelState.AddModelError("", "La relación Estudiante-Materia ya existe");
                return BadRequest(ModelState);
            }

            var newEstudianteCarrera = new EstudianteMateria
            {
                CodigoEstudiante = estudianteMateria.CodigoEstudiante,
                CodigoMateria = estudianteMateria.CodigoMateria,
                Calificacion = estudianteMateria.Calificacion,
                PeriodoActual = estudianteMateria.PeriodoActual
            };
            

            _db.EstudianteMaterias.Add(newEstudianteCarrera);
            _db.SaveChanges();
            _logger.LogInformation("EstudianteMateria creada exitosamente");

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EstudianteMateria>> GetAllMaterias()
        {
            _logger.LogInformation("Obteniendo EstudiantesMaterias");
            return Ok(_db.EstudianteMaterias.Include(
                c=>c.Estudiantes).Include(c=>c.Materias).ToList());
        }
    }
}
