using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRUD.Controllers
{
    [Route("api/AulaApi")]
    [ApiController]
    public class AulaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<AulaController> _logger;

        public AulaController(ILogger<AulaController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una Aula
        [HttpPost("CreateAula", Name = "CreateAula")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Aula> CreateAula([FromBody] Aula aula)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear el aula: Modelo no válido");
                return BadRequest(ModelState);
            }

            var obj = _db.Aulas.FirstOrDefault(u => u.CodigoAula.ToString() == aula.CodigoAula);

            if (obj != null)
            {
                _logger.LogError("Error al crear el aula: El aula con el código {CodigoAula} ya existe", aula.CodigoAula);
                return BadRequest("El aula con el código proporcionado ya existe.");
            }
            

            _db.Aulas.Add(aula);
            _db.SaveChanges();
            _logger.LogInformation("Aula creada exitosamente");

            return CreatedAtRoute("GetAula", new { id = aula.CodigoAula }, aula);
        }

        // Obtener Aulas
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Aula>> GetAulas()
        {
            _logger.LogInformation("Obteniendo Aulas");
            return Ok(_db.Aulas.ToList());
        }

        // Obtener una Aula
        [HttpGet("{id}", Name = "GetAula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Aula> GetAula(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var obj = _db.Aulas.FirstOrDefault(u => u.CodigoAula == id.ToString());

            if (obj == null)
            {
                _logger.LogInformation("Aula con el id: " + id + " no encontrada");
                return NotFound();
            }

            _logger.LogInformation("Aula " + id + " encontrada");
            return Ok(obj);
        }

        // Editar una Aula
        [HttpPut("{id}", Name = "EditAula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditAula(string id, [FromBody] Aula aula)
        {
            if (aula == null || string.IsNullOrEmpty(id) )
            
            {
                _logger.LogInformation("No se pudo obtener el aula");
                return BadRequest();
            }

            var obj = _db.Aulas.FirstOrDefault(u => u.CodigoAula == id.ToString());

            if (obj == null)
            {
                _logger.LogInformation("El aula con id: " + id + " no fue encontrada");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                ModelState.AddModelError("", "Atributos inválidos");
                return BadRequest(ModelState);
            }
            
            
            obj.Capacidad = aula.Capacidad;
            obj.TipoAula = aula.TipoAula;

            _db.SaveChanges();
            _logger.LogInformation("Aula con el id: " + id + " editada");
            return NoContent();
        }

        // Patch una Aula
        [HttpPatch("{id}", Name = "PatchAula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchAula(string id, JsonPatchDocument<Aula> patchDocument)
        {
            if (patchDocument == null || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var obj = _db.Aulas.FirstOrDefault(u => u.CodigoAula == id.ToString());

            if (obj == null)
            {
                _logger.LogInformation("No encontrado");
                return NotFound();
            }

            patchDocument.ApplyTo(obj, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                ModelState.AddModelError("", "Atributos inválidos");
                return BadRequest(ModelState);
            }

            _db.SaveChanges();
            _logger.LogInformation("Aula Patcheada");
            return NoContent();
        }

        // Delete Aula
        [HttpDelete("{id}", Name = "DeleteAula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteAula(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Error al eliminar el aula con id: " + id);
                return BadRequest();
            }

            var obj = _db.Aulas.FirstOrDefault(u => u.CodigoAula == id.ToString());

            if (obj == null)
            {
                return NotFound();
            }

            _db.Aulas.Remove(obj);
            _db.SaveChanges();

            _logger.LogInformation("Aula con id: " + id + " eliminada");
            return NoContent();
        }
    }
}
