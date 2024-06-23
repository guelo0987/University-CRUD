using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRUD.Controllers
{
    [Route("api/DocenteApi")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<DocenteController> _logger;

        public DocenteController(ILogger<DocenteController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear un Docente
        [HttpPost("CreateDocente", Name = "CreateDocente")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            if (docente.CodigoDocente > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _db.Docentes.Add(docente);
            _db.SaveChanges();
            _logger.LogInformation("Docente creado exitosamente");

            return CreatedAtRoute("GetDocente", new { id = docente.CodigoDocente }, docente);
        }

        // Obtener Docentes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
    }
}
