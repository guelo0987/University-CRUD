using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                .FirstOrDefault(cm => cm.CarreraId == carreraMateria.CarreraId && cm.CodigoMateria == carreraMateria.CodigoMateria);

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

            return Ok();
        }


        
        //Obtener todas las carreras y materias relacionadas
        [HttpGet]
        public ActionResult<CarreraMateria> GetAllCarrerasAndMaterias()
        {
            return Ok(_db.CarreraMaterias.ToList());

        }
    }
}