using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [Route("api/EstudianteMateriaApi")]
    [ApiController]
    public class EstudianteMateriaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<EstudianteMateriaController> _logger;

        public EstudianteMateriaController(ILogger<EstudianteMateriaController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una EstudianteMateria
        [HttpPost("CreateEstudianteMateria", Name = "CreateEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEstudianteMateria([FromBody] EstudianteMateria estudianteMateria)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la EstudianteMateria: Modelo no válido");
                return BadRequest(ModelState);
            }

            // Verificar si Materia y Estudiante existen
            var materia = await _db.Materias.FindAsync(estudianteMateria.CodigoMateria);
            var estudiante = await _db.Estudiantes.FindAsync(estudianteMateria.CodigoEstudiante);

            if (materia == null || estudiante == null)
            {
                _logger.LogError("Error al crear la EstudianteMateria: Materia o Estudiante no existen");
                return BadRequest("Materia o Estudiante no existen");
            }

            // Verificar si la relación ya existe
            var existingEstudianteMateria = await _db.EstudianteMaterias
                .FirstOrDefaultAsync(em => em.CodigoMateria == estudianteMateria.CodigoMateria && em.CodigoEstudiante == estudianteMateria.CodigoEstudiante);

            if (existingEstudianteMateria != null)
            {
                _logger.LogError("Error al crear la EstudianteMateria: La relación ya existe");
                ModelState.AddModelError("", "La relación Estudiante-Materia ya existe");
                return BadRequest(ModelState);
            }

            var newEstudianteMateria = new EstudianteMateria
            {
                CodigoEstudiante = estudianteMateria.CodigoEstudiante,
                SeccionId = estudianteMateria.SeccionId,
                Seccions = estudianteMateria.Seccions,
                PeriodoCursado = estudianteMateria.PeriodoCursado,
                CodigoMateria = estudianteMateria.CodigoMateria,
                Calificacion = estudianteMateria.Calificacion,
            };

            _db.EstudianteMaterias.Add(newEstudianteMateria);
            await _db.SaveChangesAsync();

            // Crear la cuenta por pagar
            var nuevaCuentaPorPagar = new CuentaPorPagar
            {
                CodigoMateria = newEstudianteMateria.CodigoMateria,
                CodigoEstudiante = newEstudianteMateria.CodigoEstudiante,
                MontoTotalaPagar = 5000 * newEstudianteMateria.Materias.CreditosMateria // Ajustar según la lógica de negocio
            };

            _db.CuentaPorPagars.Add(nuevaCuentaPorPagar);
            await _db.SaveChangesAsync();

            _logger.LogInformation("EstudianteMateria y CuentaPorPagar creadas exitosamente");

            return CreatedAtRoute("GetEstudianteMateria",
                new { codigoEstudiante = newEstudianteMateria.CodigoEstudiante, codigoMateria = newEstudianteMateria.CodigoMateria },
                newEstudianteMateria);
        }

        // Obtener todas las relaciones de estudiantes y materias
        [HttpGet("GetAllEstudiantesAndMaterias")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EstudianteMateria>> GetAllEstudiantesAndMaterias()
        {
            var estudianteMaterias = _db.EstudianteMaterias
                .Include(em => em.Estudiantes)
                .Include(em => em.Materias)
                .Include(em => em.Seccions) // Incluir la información de la sección
                .ToList();

            return Ok(estudianteMaterias);
        }

        // Obtener una EstudianteMateria específica
        [HttpGet("{codigoEstudiante:int}/{codigoMateria}", Name = "GetEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EstudianteMateria> GetEstudianteMateria(int codigoEstudiante, string codigoMateria)
        {
            var estudianteMateria = _db.EstudianteMaterias
                .Include(em => em.Estudiantes)
                .Include(em => em.Materias)
                .Include(em => em.Seccions)
                .FirstOrDefault(em => em.CodigoEstudiante == codigoEstudiante && em.CodigoMateria == codigoMateria);

            if (estudianteMateria == null)
            {
                _logger.LogInformation(
                    $"EstudianteMateria con CodigoEstudiante: {codigoEstudiante} y CodigoMateria: {codigoMateria} no encontrada");
                return NotFound();
            }

            _logger.LogInformation(
                $"EstudianteMateria con CodigoEstudiante: {codigoEstudiante} y CodigoMateria: {codigoMateria} encontrada");
            return Ok(estudianteMateria);
        }

        // Editar una EstudianteMateria
        [HttpPut("EstudianteId/{EstudianteId:int}/MateriaId/{MateriaId}", Name = "EditEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditEstudianteMateria(int EstudianteId, string MateriaId, [FromBody] EstudianteMateria estudianteMateria)
        {
            if (estudianteMateria == null || EstudianteId == 0 || string.IsNullOrEmpty(MateriaId))
            {
                _logger.LogInformation("Datos inválidos o estudiante no encontrado.");
                return BadRequest();
            }

            var objEstM = _db.EstudianteMaterias.FirstOrDefault(u => u.CodigoEstudiante == EstudianteId && u.CodigoMateria == MateriaId);

            if (objEstM == null)
            {
                _logger.LogInformation($"El estudiante con ID {EstudianteId} y materia con código {MateriaId} no fue encontrado.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                return BadRequest(ModelState);
            }

            // Eliminar la entidad existente
            _db.EstudianteMaterias.Remove(objEstM);
            _db.SaveChanges();

            // Crear una nueva entidad con los valores actualizados
            var nuevaEstudianteMateria = new EstudianteMateria
            {
                CodigoEstudiante = estudianteMateria.CodigoEstudiante,
                SeccionId = estudianteMateria.SeccionId,
                Seccions = estudianteMateria.Seccions,
                CodigoMateria = estudianteMateria.CodigoMateria,
                Calificacion = estudianteMateria.Calificacion,
            };

            _db.EstudianteMaterias.Add(nuevaEstudianteMateria);
            _db.SaveChanges();

            _logger.LogInformation("EstudianteMateria editada");
            return NoContent();
        }

        // Actualizar parcialmente una EstudianteMateria
        [HttpPatch("EstudianteId/{EstudianteId:int}/MateriaId/{MateriaId}", Name = "PatchEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PatchEstudianteMateria(int EstudianteId, string MateriaId, [FromBody] JsonPatchDocument<EstudianteMateria> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("Documento de parche nulo");
                return BadRequest();
            }

            var estudianteMateria = _db.EstudianteMaterias.FirstOrDefault(em => em.CodigoEstudiante == EstudianteId && em.CodigoMateria == MateriaId);

            if (estudianteMateria == null)
            {
                _logger.LogInformation($"La relación entre el estudiante con ID {EstudianteId} y la materia con código {MateriaId} no fue encontrada.");
                return NotFound();
            }

            var nuevoEstudianteMateria = new EstudianteMateria
            {
                CodigoEstudiante = estudianteMateria.CodigoEstudiante,
                SeccionId = estudianteMateria.SeccionId,
                Seccions = estudianteMateria.Seccions,
                CodigoMateria = estudianteMateria.CodigoMateria,
                Calificacion = estudianteMateria.Calificacion,
            };

            patchDoc.ApplyTo(nuevoEstudianteMateria, ModelState);

            if (!TryValidateModel(nuevoEstudianteMateria))
            {
                _logger.LogError("Error de validación al aplicar el documento de parche");
                return BadRequest(ModelState);
            }

            _db.EstudianteMaterias.Remove(estudianteMateria);
            _db.SaveChanges();

            _db.EstudianteMaterias.Add(nuevoEstudianteMateria);
            _db.SaveChanges();

            _logger.LogInformation("EstudianteMateria actualizada parcialmente");
            return NoContent();
        }

        // Eliminar una EstudianteMateria
        [HttpDelete("EstudianteId/{EstudianteId:int}/MateriaId/{MateriaId}", Name = "DeleteEstudianteMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteEstudianteMateria(int EstudianteId, string MateriaId)
        {
            var estudianteMateria = _db.EstudianteMaterias.FirstOrDefault(em => em.CodigoEstudiante == EstudianteId && em.CodigoMateria == MateriaId);

            if (estudianteMateria == null)
            {
                _logger.LogInformation($"La relación entre el estudiante con ID {EstudianteId} y la materia con código {MateriaId} no fue encontrada.");
                return NotFound();
            }

            _db.EstudianteMaterias.Remove(estudianteMateria);
            _db.SaveChanges();

            _logger.LogInformation("EstudianteMateria eliminada");
            return NoContent();
        }
        
        
        
        
        
        //Obtener todas las materias del estudiante en base al periodo actual
        [HttpGet("GetMateriasEstudiante/{codigoEstudiante}/{periodo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMateriasEstudiante(int codigoEstudiante, string periodo)
        {
            var materias = _db.EstudianteMaterias
                .Include(em => em.Materias)
                .Include(em => em.Seccions)
                .ThenInclude(s => s.Aulas)
                .Include(em => em.Seccions)
                .ThenInclude(s => s.MateriaDocentes)
                .ThenInclude(md => md.Docentes)
                .Where(em => em.CodigoEstudiante == codigoEstudiante && em.PeriodoCursado == periodo)
                .Select(em => new
                {
                    NombreMateria = em.Materias.NombreMateria,
                    CodigoSeccion = em.Seccions.CodigoSeccion,
                    Horario = em.Seccions.Horario,
                    Aula = em.Seccions.Aulas.CodigoAula,
                    Profesor = em.Seccions.MateriaDocentes.Select(md => md.Docentes.NombreDocente).FirstOrDefault()
                })
                .ToList();

            if (!materias.Any())
            {
                return NotFound("No se encontraron materias para el estudiante especificado en el período especificado.");
            }

            return Ok(materias);
        }
        
        //Obtener el record entero de del estudiantes de las materias que a cursado
        [HttpGet("GetMateriasYSeccionesPorEstudiante/{codigoEstudiante}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMateriasYSeccionesPorEstudiante(int codigoEstudiante)
        {
            var estudianteMaterias = await _db.EstudianteMaterias
                .Include(em => em.Materias)
                .Include(em => em.Seccions)
                .Where(em => em.CodigoEstudiante == codigoEstudiante)
                .Select(em => new
                {
                    em.Materias.CodigoMateria,
                    em.Materias.NombreMateria,
                    em.Seccions.CodigoSeccion,
                    em.Seccions.Horario,
                    em.Calificacion
                })
                .ToListAsync();

            if (estudianteMaterias == null || !estudianteMaterias.Any())
            {
                _logger.LogInformation($"No se encontraron materias para el estudiante con ID {codigoEstudiante}.");
                return NotFound("No se encontraron materias para el estudiante especificado.");
            }

            return Ok(estudianteMaterias);
        }
        
        
        
        
        //Estudiante selecciona una materia y seccion
         [HttpPost("SelectSeccion")]
        public async Task<IActionResult> SelectSeccion(int estudianteId, string seccionId)
        {
            var estudiante = await _db.Estudiantes.FindAsync(estudianteId);
            if (estudiante == null)
            {
                _logger.LogError($"Estudiante con ID {estudianteId} no encontrado");
                return NotFound("Estudiante no encontrado");
            }

            var seccion = await _db.Secciones.FindAsync(seccionId);
            if (seccion == null)
            {
                _logger.LogError($"Sección con ID {seccionId} no encontrada");
                return NotFound("Sección no encontrada");
            }

            var materia = await _db.Materias.FindAsync(seccion.CodigoMateria);
            if (materia == null)
            {
                _logger.LogError($"Materia con código {seccion.CodigoMateria} no encontrada");
                return NotFound("Materia no encontrada");
            }

            var existingEstudianteMateria = await _db.EstudianteMaterias
                .FirstOrDefaultAsync(em => em.CodigoEstudiante == estudianteId && em.SeccionId == seccionId);

            if (existingEstudianteMateria != null)
            {
                _logger.LogError($"El estudiante ya está inscrito en la sección {seccionId}");
                return BadRequest("El estudiante ya está inscrito en esta sección");
            }

            var estudianteMateria = new EstudianteMateria
            {
                CodigoMateria = seccion.CodigoMateria,
                SeccionId = seccionId,
                CodigoEstudiante = estudianteId,
                PeriodoCursado = estudiante.PeriodoActual,
                Calificacion = "NA"
            };

            _db.EstudianteMaterias.Add(estudianteMateria);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"El estudiante con ID {estudianteId} se ha inscrito en la sección {seccionId}");

            return Ok(estudianteMateria);
        }
        
    }
}
