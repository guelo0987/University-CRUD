using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/MateriaDocenteApi")]
    [ApiController]
    public class MateriaDocenteController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<MateriaDocenteController> _logger;

        public MateriaDocenteController(ILogger<MateriaDocenteController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una MateriaDocente
        [HttpPost("CreateMateriaDocente", Name = "CreateMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MateriaDocente> CreateMateriaDocente([FromBody] MateriaDocente materiaDocente)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la MateriaDocente: Modelo no válido");
                return BadRequest(ModelState);
            }

            // Verificar si Docente y Materia existen
            var docente = _db.Docentes.Find(materiaDocente.DocenteId);
            var materia = _db.Materias.Find(materiaDocente.CodigoMateria);

            var seccion = _db.Secciones.Find(materiaDocente.SeccionId);

            if (docente == null || materia == null || seccion ==null)
            {
                _logger.LogError("Error al crear la MateriaDocente: Docente , Materia o seccion no existen");
                return BadRequest("Docente , Seccion o  Materia no existen");
            }

            // Verificar si la relación ya existe
            var existingMateriaDocente = _db.MateriaDocentes
                .FirstOrDefault(dm => dm.DocenteId == materiaDocente.DocenteId && dm.CodigoMateria == materiaDocente.CodigoMateria);

            if (existingMateriaDocente != null)
            {
                _logger.LogError("Error al crear la MateriaDocente: La relación ya existe");
                ModelState.AddModelError("", "La relación Docente-Materia ya existe");
                return BadRequest(ModelState);
            }

            var newMateriaDocente = new MateriaDocente
            {
                DocenteId = materiaDocente.DocenteId,
                CodigoMateria = materiaDocente.CodigoMateria,
                SeccionId = materiaDocente.SeccionId,
                Seccions = materiaDocente.Seccions
            };

            _db.MateriaDocentes.Add(newMateriaDocente);
            _db.SaveChanges();
            _logger.LogInformation("MateriaDocente creada exitosamente");

            return CreatedAtRoute("GetMateriaDocente",
                new { docenteId = newMateriaDocente.DocenteId, codigoMateria = newMateriaDocente.CodigoMateria },
                newMateriaDocente);
        }

        // Obtener todas las relaciones de docentes y materias
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MateriaDocente>> GetAllDocentesAndMaterias()
        {
            var materiaDocentes = _db.MateriaDocentes
                .Include(dm => dm.Docentes)
                .Include(dm => dm.Materias)
                .Include(s=>s.Seccions)
                .ToList();

            return Ok(materiaDocentes);
        }

        // Obtener una MateriaDocente específica
        [HttpGet("{docenteId:int}/{codigoMateria}", Name = "GetMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MateriaDocente> GetMateriaDocente(int docenteId, string codigoMateria)
        {
            var materiaDocente = _db.MateriaDocentes
                .Include(dm => dm.Docentes)
                .Include(dm => dm.Materias)
                .Include(s=>s.Seccions)
                .FirstOrDefault(dm => dm.DocenteId == docenteId && dm.CodigoMateria == codigoMateria);

            if (materiaDocente == null)
            {
                _logger.LogInformation(
                    $"MateriaDocente con DocenteId: {docenteId} y CodigoMateria: {codigoMateria} no encontrada");
                return NotFound();
            }

            _logger.LogInformation(
                $"MateriaDocente con DocenteId: {docenteId} y CodigoMateria: {codigoMateria} encontrada");
            return Ok(materiaDocente);
        }

        // Editar una MateriaDocente
        [HttpPut("{docenteId:int}/{codigoMateria}", Name = "EditMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditMateriaDocente(int docenteId, string codigoMateria, [FromBody] MateriaDocente materiaDocente)
        {
            if (materiaDocente == null || docenteId == 0 || string.IsNullOrEmpty(codigoMateria))
            {
                _logger.LogInformation("Datos inválidos o materiaDocente no encontrado.");
                return BadRequest();
            }

            var objDocM = _db.MateriaDocentes.FirstOrDefault(u => u.DocenteId == docenteId && u.CodigoMateria == codigoMateria);

            if (objDocM == null)
            {
                _logger.LogInformation($"El docente con ID {docenteId} y materia con código {codigoMateria} no fue encontrado.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                return BadRequest(ModelState);
            }

            // Eliminar la entidad existente
            _db.MateriaDocentes.Remove(objDocM);
            _db.SaveChanges();

            // Crear una nueva entidad con los valores actualizados
            var nuevaMateriaDocente = new MateriaDocente
            {
                DocenteId = materiaDocente.DocenteId,
                CodigoMateria = materiaDocente.CodigoMateria,
                SeccionId = materiaDocente.SeccionId,
                Seccions = materiaDocente.Seccions
            };

            _db.MateriaDocentes.Add(nuevaMateriaDocente);
            _db.SaveChanges();

            _logger.LogInformation("MateriaDocente editada");
            return NoContent();
        }

        // Actualizar parcialmente una MateriaDocente
        [HttpPatch("{docenteId:int}/{codigoMateria}", Name = "PatchMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchMateriaDocente(int docenteId, string codigoMateria, [FromBody] JsonPatchDocument<MateriaDocente> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("Documento de parche nulo");
                return BadRequest();
            }

            var materiaDocente = _db.MateriaDocentes.FirstOrDefault(dm => dm.DocenteId == docenteId && dm.CodigoMateria == codigoMateria);

            if (materiaDocente == null)
            {
                _logger.LogInformation($"La relación entre el docente con ID {docenteId} y la materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            var nuevaMateriaDocente = new MateriaDocente
            {
                DocenteId = materiaDocente.DocenteId,
                CodigoMateria = materiaDocente.CodigoMateria,
                SeccionId = materiaDocente.SeccionId,
                Seccions = materiaDocente.Seccions
            };

            patchDoc.ApplyTo(nuevaMateriaDocente, ModelState);

            if (!TryValidateModel(nuevaMateriaDocente))
            {
                _logger.LogError("Error de validación al aplicar el documento de parche");
                return BadRequest(ModelState);
            }
            
            // Eliminar la entidad existente
            _db.MateriaDocentes.Remove(materiaDocente);
            _db.SaveChanges();

            // Agregar la nueva entidad con los valores actualizados
            _db.MateriaDocentes.Add(nuevaMateriaDocente);
            _db.SaveChanges();

            _db.SaveChanges();
            _logger.LogInformation("MateriaDocente actualizada parcialmente");
            return NoContent();
        }

        // Eliminar una MateriaDocente
        [HttpDelete("{docenteId:int}/{codigoMateria}", Name = "DeleteMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteMateriaDocente(int docenteId, string codigoMateria)
        {
            var materiaDocente = _db.MateriaDocentes.FirstOrDefault(dm => dm.DocenteId == docenteId && dm.CodigoMateria == codigoMateria);

            if (materiaDocente == null)
            {
                _logger.LogInformation($"La relación entre el docente con ID {docenteId} y la materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            _db.MateriaDocentes.Remove(materiaDocente);
            _db.SaveChanges();

            _logger.LogInformation("MateriaDocente eliminada");
            return NoContent();
        }
        
        // Obtener todos los estudiantes de la misma sección y materia que el docente imparte
        [HttpGet("GetEstudiantes/{codigoDocente}/{codigoMateria}/{seccionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEstudiantes(int codigoDocente, string codigoMateria, string seccionId)
        {
            var estudiantes = _db.EstudianteMaterias
                .Include(em => em.Estudiantes)
                .Include(em => em.Seccions)
                .Include(em => em.Materias)
                .Where(em => em.CodigoMateria == codigoMateria && em.SeccionId == seccionId && em.Seccions.MateriaDocentes.Any(md => md.DocenteId == codigoDocente))
                .Select(em => new
                {
                    em.Estudiantes.Id,
                    em.Estudiantes.NombreEstudiante,
                    em.Estudiantes.CorreoEstudiante,
                    em.Estudiantes.TelefonoEstudiante,
                    em.CalificacionMedioTermino,
                    em.CalificacionFinal
                })
                .ToList();

            if (!estudiantes.Any())
            {
                _logger.LogInformation("No se encontraron estudiantes para la sección y materia especificada.");
                return NotFound("No se encontraron estudiantes para la sección y materia especificada.");
            }

            return Ok(estudiantes);
        }
        [HttpPut("EditCalificacion/{codigoEstudiante}/{codigoMateria}/{seccionId}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public IActionResult EditCalificacion(int codigoEstudiante, string codigoMateria, string seccionId, [FromBody] EditCalificacionModel model)
{
    try
    {
        if (model == null)
        {
            _logger.LogError("Modelo de calificación nulo");
            return BadRequest("Datos de calificación inválidos");
        }

        var existingEstudianteMateria = _db.EstudianteMaterias
            .FirstOrDefault(em => em.CodigoMateria == codigoMateria && em.SeccionId == seccionId && em.CodigoEstudiante == codigoEstudiante);

        if (existingEstudianteMateria == null)
        {
            _logger.LogError($"Estudiante no encontrado: CodigoEstudiante={codigoEstudiante}, CodigoMateria={codigoMateria}, SeccionId={seccionId}");
            return NotFound("Estudiante no encontrado");
        }

        bool updated = false;

        if (!string.IsNullOrWhiteSpace(model.CalificacionMedioTermino))
        {
            existingEstudianteMateria.CalificacionMedioTermino = model.CalificacionMedioTermino;
            _logger.LogInformation($"Calificación de Medio Término actualizada para el estudiante {codigoEstudiante}: {model.CalificacionMedioTermino}");
            updated = true;
        }

        if (!string.IsNullOrWhiteSpace(model.CalificacionFinal))
        {
            existingEstudianteMateria.CalificacionFinal = model.CalificacionFinal;
            _logger.LogInformation($"Calificación Final actualizada para el estudiante {codigoEstudiante}: {model.CalificacionFinal}");
            updated = true;
        }

        if (!updated)
        {
            return BadRequest("No se proporcionaron calificaciones para actualizar");
        }

        _db.SaveChanges();

        _logger.LogInformation("Calificación editada exitosamente");
        return NoContent();
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error al editar calificación: {ex.Message}");
        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
    }
}
        

       
        
        // Obtener todas las secciones y materias que imparte un docente
        [HttpGet("GetMateriasYSeccionesPorDocente/{docenteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetMateriasYSeccionesPorDocente(int docenteId)
        {
            var materiasYSecciones = _db.MateriaDocentes
                .Where(md => md.DocenteId == docenteId)
                .Include(md => md.Materias)
                .Include(md => md.Seccions)
                .ThenInclude(s => s.Aulas)
                .Select(md => new
                {
                    CodigoMateria = md.CodigoMateria,
                    NombreMateria = md.Materias.NombreMateria,
                    TipoMateria = md.Materias.TipoMateria,
                    CreditosMateria = md.Materias.CreditosMateria,
                    AreaAcademica = md.Materias.AreaAcademica,
                    CodigoSeccion = md.Seccions.CodigoSeccion,
                    Horario = md.Seccions.Horario,
                    Cupo = md.Seccions.Cupo,
                    CodigoAula = md.Seccions.CodigoAula,
                    NombreAula = md.Seccions.Aulas.CodigoAula
                })
                .ToList();

            if (!materiasYSecciones.Any())
            {
                _logger.LogInformation($"No se encontraron materias y secciones para el docente con ID {docenteId}.");
                return NotFound($"No se encontraron materias y secciones para el docente con ID {docenteId}.");
            }

            _logger.LogInformation($"Se obtuvieron las materias y secciones para el docente con ID {docenteId}.");
            return Ok(materiasYSecciones);
        }
        
        
    }
    
    
    public class EditCalificacionModel
    {
        public string? CalificacionMedioTermino { get; set; }
        public string? CalificacionFinal { get; set; }
    }
}
