using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CRUD.Controllers
{
    [Route("api/SeccionMateriaApi")]
    [ApiController]
    public class SeccionMateriaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<SeccionMateriaController> _logger;

        public SeccionMateriaController(ILogger<SeccionMateriaController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una SeccionMateria
        [HttpPost("CreateSeccionMateria", Name = "CreateSeccionMateria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MateriaSeccion> CreateSeccionMateria([FromBody] MateriaSeccion seccionMateria)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la SeccionMateria: Modelo no válido");
                return BadRequest(ModelState);
            }

            // Verificar si Seccion y Materia existen
            var seccion = _db.Secciones.Find(seccionMateria.CodigoSeccion);
            var materia = _db.Materias.Find(seccionMateria.CodigoMateria);

            if (seccion == null || materia == null)
            {
                _logger.LogError("Error al crear la SeccionMateria: Seccion o Materia no existen");
                return BadRequest("Seccion o Materia no existen");
            }

            // Verificar si la relación ya existe
            var existingSeccionMateria = _db.SeccionMaterias
                .FirstOrDefault(sm => sm.CodigoSeccion == seccionMateria.CodigoSeccion && sm.CodigoMateria == seccionMateria.CodigoMateria);

            if (existingSeccionMateria != null)
            {
                _logger.LogError("Error al crear la SeccionMateria: La relación ya existe");
                ModelState.AddModelError("", "La relación Seccion-Materia ya existe");
                return BadRequest(ModelState);
            }

            var newSeccionMateria = new MateriaSeccion
            {
                CodigoSeccion = seccionMateria.CodigoSeccion,
                CodigoMateria = seccionMateria.CodigoMateria
            };

            _db.SeccionMaterias.Add(newSeccionMateria);
            _db.SaveChanges();
            _logger.LogInformation("SeccionMateria creada exitosamente");

            return CreatedAtRoute("GetSeccionMateria",
                new { codigoSeccion = newSeccionMateria.CodigoSeccion, codigoMateria = newSeccionMateria.CodigoMateria },
                newSeccionMateria);
        }

        // Obtener todas las relaciones de secciones y materias
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MateriaSeccion>> GetAllSeccionesAndMaterias()
        {
            var seccionMaterias = _db.SeccionMaterias
                .Include(sm => sm.Seccion)
                .Include(sm => sm.Materia)
                .ToList();

            return Ok(seccionMaterias);
        }

        // Obtener una SeccionMateria específica
        [HttpGet("{codigoSeccion}/{codigoMateria}", Name = "GetSeccionMateria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MateriaSeccion> GetSeccionMateria(string codigoSeccion, string codigoMateria)
        {
            var seccionMateria = _db.SeccionMaterias
                .Include(sm => sm.Seccion)
                .Include(sm => sm.Materia)
                .FirstOrDefault(sm => sm.CodigoSeccion == codigoSeccion && sm.CodigoMateria == codigoMateria);

            if (seccionMateria == null)
            {
                _logger.LogInformation(
                    $"SeccionMateria con CodigoSeccion: {codigoSeccion} y CodigoMateria: {codigoMateria} no encontrada");
                return NotFound();
            }

            _logger.LogInformation(
                $"SeccionMateria con CodigoSeccion: {codigoSeccion} y CodigoMateria: {codigoMateria} encontrada");
            return Ok(seccionMateria);
        }

        // Editar una SeccionMateria
        [HttpPut("{codigoSeccion}/{codigoMateria}", Name = "EditSeccionMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditSeccionMateria(string codigoSeccion, string codigoMateria, [FromBody] MateriaSeccion seccionMateria)
        {
            if (seccionMateria == null || string.IsNullOrEmpty(codigoSeccion)|| string.IsNullOrEmpty(codigoMateria))
            {
                _logger.LogInformation("Datos inválidos o seccionMateria no encontrada.");
                return BadRequest();
            }

            var objSecMat = _db.SeccionMaterias.FirstOrDefault(u => u.CodigoSeccion == codigoSeccion && u.CodigoMateria == codigoMateria);

            if (objSecMat == null)
            {
                _logger.LogInformation($"La seccion con ID {codigoSeccion} y materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                return BadRequest(ModelState);
            }

            // Eliminar la entidad existente
            _db.SeccionMaterias.Remove(objSecMat);
            _db.SaveChanges();

            // Crear una nueva entidad con los valores actualizados
            var nuevaSeccionMateria = new MateriaSeccion()
            {
                CodigoSeccion = seccionMateria.CodigoSeccion,
                CodigoMateria = seccionMateria.CodigoMateria
            };

            _db.SeccionMaterias.Add(nuevaSeccionMateria);
            _db.SaveChanges();

            _logger.LogInformation("SeccionMateria editada");
            return NoContent();
        }

        // Actualizar parcialmente una SeccionMateria
        [HttpPatch("{codigoSeccion}/{codigoMateria}", Name = "PatchSeccionMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchSeccionMateria(string codigoSeccion, string codigoMateria, [FromBody] JsonPatchDocument<MateriaSeccion> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("Documento de parche nulo");
                return BadRequest();
            }

            var seccionMateria = _db.SeccionMaterias.FirstOrDefault(sm => sm.CodigoSeccion == codigoSeccion && sm.CodigoMateria == codigoMateria);

            if (seccionMateria == null)
            {
                _logger.LogInformation($"La relación entre la seccion con código {codigoSeccion} y la materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            // Clonar la entidad existente para aplicar los cambios
            var nuevaSeccionMateria = new MateriaSeccion()
            {
                CodigoSeccion = seccionMateria.CodigoSeccion,
                CodigoMateria = seccionMateria.CodigoMateria
            };

            patchDoc.ApplyTo(nuevaSeccionMateria, ModelState);

            if (!TryValidateModel(nuevaSeccionMateria))
            {
                _logger.LogError("Error de validación al aplicar el documento de parche");
                return BadRequest(ModelState);
            }

            // Eliminar la entidad existente
            _db.SeccionMaterias.Remove(seccionMateria);
            _db.SaveChanges();

            // Agregar la nueva entidad con los valores actualizados
            _db.SeccionMaterias.Add(nuevaSeccionMateria);
            _db.SaveChanges();

            _logger.LogInformation("SeccionMateria actualizada parcialmente");
            return NoContent();
        }

        // Eliminar una SeccionMateria
        [HttpDelete("{codigoSeccion}/{codigoMateria}", Name = "DeleteSeccionMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteSeccionMateria(string codigoSeccion, string codigoMateria)
        {
            var seccionMateria = _db.SeccionMaterias.FirstOrDefault(sm => sm.CodigoSeccion == codigoSeccion && sm.CodigoMateria == codigoMateria);

            if (seccionMateria == null)
            {
                _logger.LogInformation($"La relación entre la seccion con código {codigoSeccion} y la materia con código {codigoMateria} no fue encontrada.");
                return NotFound();
            }

            _db.SeccionMaterias.Remove(seccionMateria);
            _db.SaveChanges();

            _logger.LogInformation("SeccionMateria eliminada");
            return NoContent();
        }
    }
}
