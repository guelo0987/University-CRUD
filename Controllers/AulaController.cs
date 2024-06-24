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

            if (!int.TryParse(aula.CodigoAula, out int codigoAula) || codigoAula > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
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
        [HttpGet("{id:int}", Name = "GetAula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Aula> GetAula(int id)
        {
            if (id <= 0)
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
        [HttpPut("{id:int}", Name = "EditAula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditAula(int id, [FromBody] Aula aula)
        {
            if (aula == null || id != int.Parse(aula.CodigoAula) || id == 0)
            
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
        [HttpPatch("{id:int}", Name = "PatchAula")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchAula(int id, JsonPatchDocument<Aula> patchDocument)
        {
            if (patchDocument == null || id == 0)
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
        [HttpDelete("{id:int}", Name = "DeleteAula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteAula(int id)
        {
            if (id <= 0)
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
