using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
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

        // Crear una Seccion
        [HttpPost("CreateSeccion", Name = "CreateSeccion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Seccion> CreateSeccion([FromBody] Seccion seccion)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la Seccion: Modelo no válido");
                return BadRequest(ModelState);
            }

            // Verificar si Materia y Aula existen
            var aula = _db.Aulas.Find(seccion.CodigoAula);
            var materia = _db.Materias.Find(seccion.CodigoMateria);

            if (aula == null || materia == null)
            {
                _logger.LogError("Error al crear la Seccion: Aula o Materia no existen");
                return BadRequest("Aula o Materia no existen");
            }

            // Verificar si el aula ya está ocupada en el mismo horario
            var overlappingSeccion = _db.Secciones
                .FirstOrDefault(s => s.CodigoAula == seccion.CodigoAula && s.Horario == seccion.Horario);

            if (overlappingSeccion != null)
            {
                _logger.LogError("Error al crear la Seccion: El aula ya está ocupada en ese horario");
                ModelState.AddModelError("", "El aula ya está ocupada en ese horario");
                return BadRequest(ModelState);
            }

            var newSeccion = new Seccion
            {
                CodigoSeccion = seccion.CodigoSeccion,
                CodigoAula = seccion.CodigoAula,
                CodigoMateria = seccion.CodigoMateria,
                Horario = seccion.Horario,
                Cupo = seccion.Cupo
            };

            _db.Secciones.Add(newSeccion);
            _db.SaveChanges();
            _logger.LogInformation("Seccion creada exitosamente");

            return Created();
        }


        // Obtener Secciones
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Seccion>> GetSecciones()
        {
            _logger.LogInformation("Obteniendo Secciones");

            return Ok(_db.Secciones.Include(M=>M.Materias).ToList());
        }

        // Obtener una Sección
        [HttpGet("{id}", Name = "GetSeccion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Seccion> GetSeccion(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var obj = _db.Secciones.Include(m=>m.Materias).FirstOrDefault(s => s.CodigoSeccion == id);

            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj);
        }

        // Editar una Sección
        [HttpPut("{id}", Name = "EditSeccion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditSeccion(string id, [FromBody] Seccion seccion)
        {
            if (seccion == null || id != seccion.CodigoSeccion || string.IsNullOrEmpty(id))
            {
                _logger.LogInformation("No se pudo obtener la sección");
                return BadRequest();
            }

            var objSec = _db.Secciones.FirstOrDefault(s => s.CodigoSeccion == id);
            var materia = _db.Materias.Find(seccion.CodigoMateria);

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
            objSec.CodigoMateria = seccion.CodigoMateria;   

            _db.SaveChanges();

            _logger.LogInformation("Sección con el id: " + id + " editada");
            return NoContent();
        }

        // Actualizar parcialmente una Sección
        [HttpPatch("{id}", Name = "PatchSeccion")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchSeccion(string id, JsonPatchDocument<Seccion> patchDocument)
        {
            if (patchDocument == null || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var obj = _db.Secciones.FirstOrDefault(s => s.CodigoSeccion == id);

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
        [HttpDelete("{id}", Name = "DeleteSeccion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteSeccion(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Error al eliminar la sección " + id);
                return BadRequest();
            }

            var obj = _db.Secciones.FirstOrDefault(s => s.CodigoSeccion == id);

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
