using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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

            if (docente == null || materia == null)
            {
                _logger.LogError("Error al crear la MateriaDocente: Docente o Materia no existen");
                return BadRequest("Docente o Materia no existen");
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
                CodigoMateria = materiaDocente.CodigoMateria
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
                .ToList();

            return Ok(materiaDocentes);
        }

        // Obtener una MateriaDocente específica
        [HttpGet("{docenteId:int}/{codigoMateria:int}", Name = "GetMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MateriaDocente> GetMateriaDocente(int docenteId, int codigoMateria)
        {
            var materiaDocente = _db.MateriaDocentes
                .Include(dm => dm.Docentes)
                .Include(dm => dm.Materias)
                .FirstOrDefault(dm => dm.DocenteId == docenteId && dm.CodigoMateria == codigoMateria.ToString());

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
        [HttpPut("{docenteId:int}/{codigoMateria:int}", Name = "EditMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditMateriaDocente(int docenteId, int codigoMateria, [FromBody] MateriaDocente materiaDocente)
        {
            if (materiaDocente == null || docenteId == 0 || codigoMateria == 0)
            {
                _logger.LogInformation("Datos inválidos o materiaDocente no encontrado.");
                return BadRequest();
            }

            var objDocM = _db.MateriaDocentes.FirstOrDefault(u => u.DocenteId == docenteId && u.CodigoMateria == codigoMateria.ToString());

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
                CodigoMateria = materiaDocente.CodigoMateria
            };

            _db.MateriaDocentes.Add(nuevaMateriaDocente);
            _db.SaveChanges();

            _logger.LogInformation("MateriaDocente editada");
            return NoContent();
        }

        // Actualizar parcialmente una MateriaDocente
        [HttpPatch("{docenteId:int}/{codigoMateria:int}", Name = "PatchMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchMateriaDocente(int docenteId, int codigoMateria, [FromBody] JsonPatchDocument<MateriaDocente> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("Documento de parche nulo");
                return BadRequest();
            }

            var materiaDocente = _db.MateriaDocentes.FirstOrDefault(dm => dm.DocenteId == docenteId && dm.CodigoMateria == codigoMateria.ToString());

            if (materiaDocente == null)
            {
                _logger.LogInformation($"La relación entre el docente con ID {docenteId} y la materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            var nuevaMateriaDocente = new MateriaDocente
            {
                CodigoMateria = materiaDocente.CodigoMateria,
                DocenteId = materiaDocente.DocenteId
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
        [HttpDelete("{docenteId:int}/{codigoMateria:int}", Name = "DeleteMateriaDocente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteMateriaDocente(int docenteId, int codigoMateria)
        {
            var materiaDocente = _db.MateriaDocentes.FirstOrDefault(dm => dm.DocenteId == docenteId && dm.CodigoMateria == codigoMateria.ToString());

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
    }
}
