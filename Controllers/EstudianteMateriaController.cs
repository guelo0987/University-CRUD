using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/EstudianteMateriaApi")]
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

        // Crear una EstudianteMateria
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

            var newEstudianteMateria = new EstudianteMateria
            {
                CodigoEstudiante = estudianteMateria.CodigoEstudiante,
                CodigoMateria = estudianteMateria.CodigoMateria,
                Calificacion = estudianteMateria.Calificacion,
                PeriodoActual = estudianteMateria.PeriodoActual
            };

            _db.EstudianteMaterias.Add(newEstudianteMateria);
            _db.SaveChanges();
            _logger.LogInformation("EstudianteMateria creada exitosamente");

            return CreatedAtRoute("GetEstudianteMateria",
                new { codigoEstudiante = newEstudianteMateria.CodigoEstudiante, codigoMateria = newEstudianteMateria.CodigoMateria },
                newEstudianteMateria);
        }

        // Obtener todas las relaciones de estudiantes y materias
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EstudianteMateria>> GetAllEstudiantesAndMaterias()
        {
            var estudianteMaterias = _db.EstudianteMaterias
                .Include(em => em.Estudiantes)
                .Include(em => em.Materias)
                .ToList();

            return Ok(estudianteMaterias);
        }

        // Obtener una EstudianteMateria específica
        [HttpGet("{codigoEstudiante:int}/{codigoMateria}", Name = "GetEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EstudianteMateria> GetEstudianteMateria(int codigoEstudiante, string codigoMateria)
        {
            var estudianteMateria = _db.EstudianteMaterias
                .Include(em => em.Estudiantes)
                .Include(em => em.Materias)
                .FirstOrDefault(em => em.CodigoEstudiante == codigoEstudiante && em.CodigoMateria == codigoMateria);

            if (estudianteMateria == null)
            {
                _logger.LogInformation(
                    $"EstudianteMateria con CodigoEstudiante: {codigoEstudiante} y CodigoMateria: {codigoMateria} no encontrada");
                return NotFound();
            }

            _logger.LogInformation(
                $"EstudianteMateria con CodigoEstudiante: {codigoEstudiante} y CodigoMateria: {codigoMateria} encontrada");
            return Ok(estudianteMateria);
        }

        // Editar una EstudianteMateria
        [HttpPut("EstudianteId/{EstudianteId:int}/MateriaId/{MateriaId}", Name = "EditEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditEstudianteMateria(int EstudianteId, string MateriaId, [FromBody] EstudianteMateria estudianteMateria)
        {
            if (estudianteMateria == null || EstudianteId == 0 ||  string.IsNullOrEmpty(MateriaId))
            {
                _logger.LogInformation("Datos inválidos o estudiante no encontrado.");
                return BadRequest();
            }

            var objEstM = _db.EstudianteMaterias.FirstOrDefault(u => u.CodigoEstudiante == EstudianteId && u.CodigoMateria == MateriaId);

            if (objEstM == null)
            {
                _logger.LogInformation($"El estudiante con ID {EstudianteId} y materia con código {MateriaId} no fue encontrado.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                return BadRequest(ModelState);
            }

            // Eliminar la entidad existente
            _db.EstudianteMaterias.Remove(objEstM);
            _db.SaveChanges();

            // Crear una nueva entidad con los valores actualizados
            var nuevaEstudianteMateria = new EstudianteMateria
            {
                CodigoEstudiante = estudianteMateria.CodigoEstudiante,
                CodigoMateria = estudianteMateria.CodigoMateria,
                Calificacion = estudianteMateria.Calificacion,
                PeriodoActual = estudianteMateria.PeriodoActual
            };

            _db.EstudianteMaterias.Add(nuevaEstudianteMateria);
            _db.SaveChanges();

            _logger.LogInformation("EstudianteMateria editada");
            return NoContent();
        }
        
        // Actualizar parcialmente una EstudianteMateria
        [HttpPatch("EstudianteId/{EstudianteId:int}/MateriaId/{MateriaId}", Name = "PatchEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchEstudianteMateria(int EstudianteId, string MateriaId, [FromBody] JsonPatchDocument<EstudianteMateria> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("Documento de parche nulo");
                return BadRequest();
            }

            var estudianteMateria = _db.EstudianteMaterias.FirstOrDefault(em => em.CodigoEstudiante == EstudianteId && em.CodigoMateria == MateriaId);

            if (estudianteMateria == null)
            {
                _logger.LogInformation($"La relación entre el estudiante con ID {EstudianteId} y la materia con código {MateriaId} no fue encontrada.");
                return NotFound();
            }
            var nuevoEstudianteMateria = new EstudianteMateria
            {
                CodigoMateria = estudianteMateria.CodigoMateria,
                CodigoEstudiante = estudianteMateria.CodigoEstudiante,
                Calificacion = estudianteMateria.Calificacion,
                PeriodoActual = estudianteMateria.PeriodoActual
            };
            
            patchDoc.ApplyTo(nuevoEstudianteMateria, ModelState);

            if (!TryValidateModel(nuevoEstudianteMateria))
            {
                _logger.LogError("Error de validación al aplicar el documento de parche");
                return BadRequest(ModelState);
            }



            _db.EstudianteMaterias.Remove(estudianteMateria);
            _db.SaveChanges();


            _db.EstudianteMaterias.Add(nuevoEstudianteMateria);
            _db.SaveChanges();

            _logger.LogInformation("EstudianteMateria actualizada parcialmente");
            return NoContent();
        }
        
        
        // Eliminar una EstudianteMateria
        [HttpDelete("EstudianteId/{EstudianteId:int}/MateriaId/{MateriaId}", Name = "DeleteEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteEstudianteMateria(int EstudianteId, string MateriaId)
        {
            var estudianteMateria = _db.EstudianteMaterias.FirstOrDefault(em => em.CodigoEstudiante == EstudianteId && em.CodigoMateria == MateriaId);

            if (estudianteMateria == null)
            {
                _logger.LogInformation($"La relación entre el estudiante con ID {EstudianteId} y la materia con código {MateriaId} no fue encontrada.");
                return NotFound();
            }

            _db.EstudianteMaterias.Remove(estudianteMateria);
            _db.SaveChanges();

            _logger.LogInformation("EstudianteMateria eliminada");
            return NoContent();
        }


        
        
    }
}
