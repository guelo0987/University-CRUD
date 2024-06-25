using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/MateriaApi")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<MateriaController> _logger;

        public MateriaController(ILogger<MateriaController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una Materia
        [HttpPost("CreateMateria", Name = "CreateMateria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Materia> CreateMateria([FromBody] Materia materia)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la materia: Modelo no válido");
                return BadRequest(ModelState);
            }

            if (_db.Materias.Any(u => u.NombreMateria.ToLower() == materia.NombreMateria.ToLower()))
            {
                _logger.LogError("Error al crear la materia: La materia ya existe");
                ModelState.AddModelError("", "La materia ya existe");
                return BadRequest(ModelState);
            }

            if (int.TryParse(materia.CodigoMateria, out int codigo) && codigo == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            
            _db.Materias.Add(materia);
            _db.SaveChanges();
            _logger.LogInformation("Materia creada exitosamente");

            return CreatedAtRoute("GetMateria", new { id = materia.CodigoMateria }, materia);
        }

        // Obtener Materias
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Materia>> GetMaterias()
        {
            _logger.LogInformation("Obteniendo Materias");
            return Ok(_db.Materias.ToList());
        }

        // Obtener una Materia
        [HttpGet("id", Name = "GetMateria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Materia> GetMateria(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var obj = _db.Materias.FirstOrDefault(u => u.CodigoMateria == id);

            if (obj == null)
            {
                _logger.LogInformation("Materia con el id: " + id + " no encontrada");
                return NotFound();
            }

            _logger.LogInformation("Materia " + id + " encontrada");
            return Ok(obj);
        }

        // Editar una Materia
        [HttpPut("id", Name = "EditMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Materia> EditMateria(string id, [FromBody] Materia materia)
        {
            if (materia == null || id != materia.CodigoMateria || string.IsNullOrEmpty(id))
            {   
                _logger.LogInformation("No se pudo obtener la materia");
                return BadRequest();
            }

            var obj = _db.Materias.FirstOrDefault(u => u.CodigoMateria == id);

            if (obj == null)
            {
                _logger.LogInformation("La materia " + id + " no fue encontrada");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos inválidos");
                ModelState.AddModelError("", "Atributos inválidos");
                return BadRequest(ModelState);
            }

            obj.NombreMateria = materia.NombreMateria;
            obj.TipoMateria = materia.TipoMateria;
            obj.CreditosMateria = materia.CreditosMateria;
            obj.AreaAcademica = materia.AreaAcademica;

            _db.SaveChanges();

            _logger.LogInformation("Materia con el id: " + id + " editada");
            return NoContent();
        }

        // Patch Materia
        [HttpPatch("id", Name = "PatchMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Materia> PatchMateria(string id, JsonPatchDocument<Materia> patchDocument)
        {
            if (patchDocument == null || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var obj = _db.Materias.FirstOrDefault(u => u.CodigoMateria == id);

            if (obj == null)
            {
                _logger.LogInformation("No encontrado");
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

            _logger.LogInformation("Materia parcheada");
            return NoContent();
        }

        // Delete Materia
        [HttpDelete("id", Name = "DeleteMateria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Materia> DeleteMateria(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Error al eliminar la materia " + id);
                return BadRequest();
            }

            var obj = _db.Materias.FirstOrDefault(u => u.CodigoMateria == id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Materias.Remove(obj);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
