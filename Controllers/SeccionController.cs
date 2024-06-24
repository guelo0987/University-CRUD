using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRUD.Controllers
{
    [Route("api/SeccionApi")]
    [ApiController]
    public class SeccionController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<SeccionController> _logger;

        public SeccionController(ILogger<SeccionController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una Sección
        [HttpPost("CreateSeccion", Name = "CreateSeccion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Seccion> CreateSeccion([FromBody] Seccion seccion)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la sección: Modelo no válido");
                return BadRequest(ModelState);
            }

            
            if (int.TryParse(seccion.CodigoSeccion, out int codigoSeccion) && codigoSeccion > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            

            _db.Secciones.Add(seccion);
            _db.SaveChanges();
            _logger.LogInformation("Sección creada exitosamente");

            return CreatedAtRoute("GetSeccion", new { id = seccion.CodigoSeccion }, seccion);
        }

        // Obtener Secciones
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Seccion>> GetSecciones()
        {
            _logger.LogInformation("Obteniendo Secciones");

            return Ok(_db.Secciones.ToList());
        }

        // Obtener una Sección
        [HttpGet("{id:int}", Name = "GetSeccion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Seccion> GetSeccion(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var obj = _db.Secciones.FirstOrDefault(s => s.CodigoSeccion == id.ToString());

            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj);
        }

        // Editar una Sección
        [HttpPut("{id:int}", Name = "EditSeccion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditSeccion(int id, [FromBody] Seccion seccion)
        {
            if (seccion == null || id != int.Parse(seccion.CodigoSeccion) || id == 0)
            {
                _logger.LogInformation("No se pudo obtener la sección");
                return BadRequest();
            }

            var objSec = _db.Secciones.FirstOrDefault(s => s.CodigoSeccion == id.ToString());
            

            if (objSec == null)
            {
                _logger.LogInformation("La sección: " + id + " no fue encontrada");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos inválidos");
                ModelState.AddModelError("", "Atributos inválidos");
                return BadRequest(ModelState);
            }

           
            objSec.Horario = seccion.Horario;
            objSec.Cupo = seccion.Cupo;

            _db.SaveChanges();

            _logger.LogInformation("Sección con el id: " + id + " editada");
            return NoContent();
        }

        // Actualizar parcialmente una Sección
        [HttpPatch("{id:int}", Name = "PatchSeccion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchSeccion(int id, JsonPatchDocument<Seccion> patchDocument)
        {
            if (patchDocument == null || id == 0)
            {
                return BadRequest();
            }

            var obj = _db.Secciones.FirstOrDefault(s => s.CodigoSeccion == id.ToString());

            if (obj == null)
            {
                _logger.LogInformation("Sección no encontrada");
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

            _logger.LogInformation("Sección actualizada parcialmente");
            return NoContent();
        }

        // Eliminar una Sección
        [HttpDelete("{id:int}", Name = "DeleteSeccion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteSeccion(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Error al eliminar la sección " + id);
                return BadRequest();
            }

            var obj = _db.Secciones.FirstOrDefault(s => s.CodigoSeccion == id.ToString());

            if (obj == null)
            {
                return NotFound();
            }

            _db.Secciones.Remove(obj);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
