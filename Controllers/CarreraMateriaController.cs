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
    [Route("api/CarreraMateriaApi")]
    [ApiController]
    public class CarreraMateriaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<CarreraMateriaController> _logger;

        public CarreraMateriaController(ILogger<CarreraMateriaController> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // Crear una CarreraMateria
        [HttpPost("CreateCarreraMateria", Name = "CreateCarreraMateria")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CarreraMateria> CreateCarreraMateria([FromBody] CarreraMateria carreraMateria)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error al crear la CarreraMateria: Modelo no válido");
                return BadRequest(ModelState);
            }

            // Verificar si Carrera y Materia existen
            var carrera = _db.Carreras.Find(carreraMateria.CarreraId);
            var materia = _db.Materias.Find(carreraMateria.CodigoMateria);

            if (carrera == null || materia == null)
            {
                _logger.LogError("Error al crear la CarreraMateria: Carrera o Materia no existen");
                return BadRequest("Carrera o Materia no existen");
            }

            // Verificar si la relación ya existe
            var existingCarreraMateria = _db.CarreraMaterias
                .FirstOrDefault(cm =>
                    cm.CarreraId == carreraMateria.CarreraId && cm.CodigoMateria == carreraMateria.CodigoMateria);

            if (existingCarreraMateria != null)
            {
                _logger.LogError("Error al crear la CarreraMateria: La relación ya existe");
                ModelState.AddModelError("", "La relación Carrera-Materia ya existe");
                return BadRequest(ModelState);
            }

            var newCarreraMateria = new CarreraMateria
            {
                CarreraId = carreraMateria.CarreraId,
                CodigoMateria = carreraMateria.CodigoMateria
            };

            _db.CarreraMaterias.Add(newCarreraMateria);
            _db.SaveChanges();
            _logger.LogInformation("CarreraMateria creada exitosamente");

            return CreatedAtRoute("GetCarreraMateria",
                new { carreraId = newCarreraMateria.CarreraId, codigoMateria = newCarreraMateria.CodigoMateria },
                newCarreraMateria);
        }

        // Obtener todas las carreras y materias relacionadas
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CarreraMateria>> GetAllCarrerasAndMaterias()
        {
            var carreraMaterias = _db.CarreraMaterias
                .Include(cm => cm.Carreras)
                .Include(cm => cm.Materias)
                .ToList();

            return Ok(carreraMaterias);
        }
        
        
        

        // Obtener una CarreraMateria específica
        [HttpGet("{carreraId:int}/{codigoMateria:int}", Name = "GetCarreraMateria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CarreraMateria> GetCarreraMateria(int carreraId, int codigoMateria)
        {
            var carreraMateria = _db.CarreraMaterias
                .Include(cm => cm.Carreras)
                .Include(cm => cm.Materias)
                .FirstOrDefault(cm => cm.CarreraId == carreraId && cm.CodigoMateria == codigoMateria);

            if (carreraMateria == null)
            {
                _logger.LogInformation(
                    $"CarreraMateria con CarreraId: {carreraId} y CodigoMateria: {codigoMateria} no encontrada");
                return NotFound();
            }

            _logger.LogInformation(
                $"CarreraMateria con CarreraId: {carreraId} y CodigoMateria: {codigoMateria} encontrada");
            return Ok(carreraMateria);
        }



        
        // Editar Un estudiante
        [HttpPut("CarreraId/{CarreraId:int}/MateriaId/{MateriaId:int}", Name = "EditCarreraMateria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult EditCarreraMateria(int CarreraId, int MateriaId, [FromBody] CarreraMateria carreraMateria)
        {
            if (carreraMateria == null || CarreraId == 0 || MateriaId == 0)
            {
                _logger.LogInformation("Datos inválidos o estudiante no encontrado.");
                return BadRequest();
            }

            var objCrrM = _db.CarreraMaterias.FirstOrDefault(u => u.CarreraId == CarreraId && u.CodigoMateria == MateriaId);

            if (objCrrM == null)
            {
                _logger.LogInformation($"La carrera con ID {CarreraId} y materia con código {MateriaId} no fue encontrada.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Atributos Invalidos");
                return BadRequest(ModelState);
            }

            // Eliminar la entidad existente
            _db.CarreraMaterias.Remove(objCrrM);
            _db.SaveChanges();

            // Crear una nueva entidad con los valores actualizados
            var nuevaCarreraMateria = new CarreraMateria
            {
                CarreraId = carreraMateria.CarreraId,
                CodigoMateria = carreraMateria.CodigoMateria
            };

            _db.CarreraMaterias.Add(nuevaCarreraMateria);
            _db.SaveChanges();

            _logger.LogInformation("CarreraMateria editada");
            return NoContent();
        }












    }
}