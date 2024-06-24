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
    [Route("api/MateriaAulaApi")]
    [ApiController]
    public class MateriaAulaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<MateriaAulaController> _logger;

        public MateriaAulaController(ILogger<MateriaAulaController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una MateriaAula
        [HttpPost("CreateMateriaAula", Name = "CreateMateriaAula")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MateriaAula> CreateMateriaAula([FromBody] MateriaAula materiaAula)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la MateriaAula: Modelo no válido");
                return BadRequest(ModelState);
            }

            // Verificar si Aula y Materia existen
            var aula = _db.Aulas.Find(materiaAula.AulaId);
            var materia = _db.Materias.Find(materiaAula.CodigoMateria);

            if (aula == null || materia == null)
            {
                _logger.LogError("Error al crear la MateriaAula: Aula o Materia no existen");
                return BadRequest("Aula o Materia no existen");
            }

            // Verificar si la relación ya existe
            var existingMateriaAula = _db.MateriaAulas
                .FirstOrDefault(ma => ma.AulaId == materiaAula.AulaId && ma.CodigoMateria == materiaAula.CodigoMateria);

            if (existingMateriaAula != null)
            {
                _logger.LogError("Error al crear la MateriaAula: La relación ya existe");
                ModelState.AddModelError("", "La relación Aula-Materia ya existe");
                return BadRequest(ModelState);
            }

            var newMateriaAula = new MateriaAula
            {
                AulaId = materiaAula.AulaId,
                CodigoMateria = materiaAula.CodigoMateria
            };

            _db.MateriaAulas.Add(newMateriaAula);
            _db.SaveChanges();
            _logger.LogInformation("MateriaAula creada exitosamente");

            return CreatedAtRoute("GetMateriaAula",
                new { aulaId = newMateriaAula.AulaId, codigoMateria = newMateriaAula.CodigoMateria },
                newMateriaAula);
        }

        // Obtener todas las relaciones de aulas y materias
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MateriaAula>> GetAllAulasAndMaterias()
        {
            var materiaAulas = _db.MateriaAulas
                .Include(ma => ma.Aulas)
                .Include(ma => ma.Materias)
                .ToList();

            return Ok(materiaAulas);
        }

        // Obtener una MateriaAula específica
        [HttpGet("{aulaId:int}/{codigoMateria:int}", Name = "GetMateriaAula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MateriaAula> GetMateriaAula(int aulaId, int codigoMateria)
        {
            var materiaAula = _db.MateriaAulas
                .Include(ma => ma.Aulas)
                .Include(ma => ma.Materias)
                .FirstOrDefault(ma => ma.AulaId == aulaId.ToString() && ma.CodigoMateria == codigoMateria.ToString());

            if (materiaAula == null)
            {
                _logger.LogInformation(
                    $"MateriaAula con AulaId: {aulaId} y CodigoMateria: {codigoMateria} no encontrada");
                return NotFound();
            }

            _logger.LogInformation(
                $"MateriaAula con AulaId: {aulaId} y CodigoMateria: {codigoMateria} encontrada");
            return Ok(materiaAula);
        }

        // Editar una MateriaAula
        [HttpPut("{aulaId:int}/{codigoMateria:int}", Name = "EditMateriaAula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditMateriaAula(int aulaId, int codigoMateria, [FromBody] MateriaAula materiaAula)
        {
            if (materiaAula == null || aulaId == 0 || codigoMateria == 0)
            {
                _logger.LogInformation("Datos inválidos o materiaAula no encontrado.");
                return BadRequest();
            }

            var objAulaM = _db.MateriaAulas.FirstOrDefault(u => u.AulaId == aulaId.ToString() && u.CodigoMateria == codigoMateria.ToString());

            if (objAulaM == null)
            {
                _logger.LogInformation($"El aula con ID {aulaId} y materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                return BadRequest(ModelState);
            }

            // Eliminar la entidad existente
            _db.MateriaAulas.Remove(objAulaM);
            _db.SaveChanges();

            // Crear una nueva entidad con los valores actualizados
            var nuevaMateriaAula = new MateriaAula
            {
                AulaId = materiaAula.AulaId,
                CodigoMateria = materiaAula.CodigoMateria
            };

            _db.MateriaAulas.Add(nuevaMateriaAula);
            _db.SaveChanges();

            _logger.LogInformation("MateriaAula editada");
            return NoContent();
        }

        // Actualizar parcialmente una MateriaAula
        [HttpPatch("{aulaId:int}/{codigoMateria:int}", Name = "PatchMateriaAula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchMateriaAula(int aulaId, int codigoMateria, [FromBody] JsonPatchDocument<MateriaAula> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("Documento de parche nulo");
                return BadRequest();
            }

            var materiaAula = _db.MateriaAulas.FirstOrDefault(ma => ma.AulaId == aulaId.ToString() && ma.CodigoMateria == codigoMateria.ToString());

            if (materiaAula == null)
            {
                _logger.LogInformation($"La relación entre el aula con ID {aulaId} y la materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            var nuevaMateriaAula = new MateriaAula
            {
                AulaId = materiaAula.AulaId,
                CodigoMateria = materiaAula.CodigoMateria
            };
            
            
            patchDoc.ApplyTo(nuevaMateriaAula, ModelState);

            if (!TryValidateModel(nuevaMateriaAula))
            {
                _logger.LogError("Error de validación al aplicar el documento de parche");
                return BadRequest(ModelState);
            }
            
            // Eliminar la entidad existente
            _db.MateriaAulas.Remove(materiaAula);
            _db.SaveChanges();

            // Agregar la nueva entidad con los valores actualizados
            _db.MateriaAulas.Add(nuevaMateriaAula);
            _db.SaveChanges();

            _db.SaveChanges();
            _logger.LogInformation("MateriaAula actualizada parcialmente");
            return NoContent();
        }

        // Eliminar una MateriaAula
        [HttpDelete("{aulaId:int}/{codigoMateria:int}", Name = "DeleteMateriaAula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteMateriaAula(int aulaId, int codigoMateria)
        {
            var materiaAula = _db.MateriaAulas.FirstOrDefault(ma => ma.AulaId == aulaId.ToString() && ma.CodigoMateria == codigoMateria.ToString());

            if (materiaAula == null)
            {
                _logger.LogInformation($"La relación entre el aula con ID {aulaId} y la materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            _db.MateriaAulas.Remove(materiaAula);
            _db.SaveChanges();

            _logger.LogInformation("MateriaAula eliminada");
            return NoContent();
        }
    }
}
