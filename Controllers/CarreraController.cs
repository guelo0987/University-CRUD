using CRUD.Context;

using CRUD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers;





[Route("api/CarreraApi")]
[ApiController]
public class CarreraController:ControllerBase
{


    private readonly MyDbContext _db;
    private readonly ILogger<CarreraController> _logger;



    public CarreraController(ILogger<CarreraController> logger, MyDbContext db )
    {
        _logger = logger;
        _db = db;
    }
    
    
    //Crear una Carrera
    [HttpPost("CreateCarrera", Name = "CreateCarrera")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Carrera> CreateCarrera([FromBody] Carrera carrera)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Error al crear la carrera: Modelo no válido");
            return BadRequest(ModelState);
        }
        
      

        if (_db.Carreras.Any(u => u.NombreCarrera.ToLower() == carrera.NombreCarrera.ToLower()))
        {
            _logger.LogError("Error al crear la carrera: La carrera ya existe");
            ModelState.AddModelError("", "La carrera  ya existe");
            return BadRequest(ModelState);
        }
        

        if (carrera.CarreraId > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

       

        _db.Carreras.Add(carrera);
        _db.SaveChanges();
        _logger.LogInformation("Carrera creado exitosamente");

        return CreatedAtRoute("GetCarrera", new { id = carrera.CarreraId }, carrera);
    }
    
    
    
    
    //Obtener Carreras
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Carrera> GetCarreras()
    {
        
        _logger.LogInformation("Obteniendo Carreras");

        return Ok(_db.Carreras.ToList());
    }
    
    
    //Obtener una Carrera
    [HttpGet("id:int", Name = "GetCarrera")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public ActionResult<Estudiante> GetEstudiante(int id)
    {

        if (id <= 0)
        {
            return BadRequest();
        }


        var obj = _db.Carreras.FirstOrDefault(u => u.CarreraId == id);

        if (obj == null)
        {
            _logger.LogInformation("Carrera con el id: "+id+ " No encontrada");
            return NotFound();
        }
            
        _logger.LogInformation("Carrera" +id+ "Encontrada");
        return Ok(obj);

    }
    
    
    
    //Editar una carrera
    [HttpPut("id:int",Name = "EditCarrera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Carrera> EditCarrera(int id, [FromBody] Carrera carrera)
    {

        if (carrera == null || id != carrera.CarreraId || id==0)
        {
            _logger.LogInformation("No se puedo obtener la carrera");
            return BadRequest();
        }

        var obj = _db.Carreras.FirstOrDefault(u => u.CarreraId == id);

        if (obj == null)
        {
            _logger.LogInformation("La carrera"+id+"No fue encontrada");
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Atributos Invalidos");
            ModelState.AddModelError("","Atributos invalidos");
            return BadRequest(ModelState);
        }

        obj.NombreCarrera = carrera.NombreCarrera;
        obj.Departamento = carrera.Departamento;
        obj.DuracionPeriodos = carrera.DuracionPeriodos;
        obj.TotalCreditos = carrera.TotalCreditos;

        _db.SaveChanges();
        
        _logger.LogInformation("Carrera con el id: "+id+" editada");
        return NoContent();
    }
    
    
    
    //PatchCarrera
    [HttpPatch("id:int", Name = "PatchCarrera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Carrera> PatchCarrera(int id, JsonPatchDocument<Carrera> patchDocument)
    {

        if (patchDocument == null || id == 0)
        {
            return BadRequest();
        }

        var obj = _db.Carreras.FirstOrDefault(u => u.CarreraId == id);

        if (obj == null)
        {  
            _logger.LogInformation("No encontrado");
            return NotFound();
        }
        
        patchDocument.ApplyTo(obj,ModelState);

        if (!ModelState.IsValid)
        {
            _logger.LogError("Atributos Invalidos");
            ModelState.AddModelError("","Atributos invalidos");
            return BadRequest(ModelState);
        }

        _db.SaveChanges();
        
        _logger.LogInformation("Carrera Patcheada");
        return NoContent();
    }



    
    //Delete Carrera
    [HttpDelete("id:int", Name = "DeleteCarrera")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public ActionResult<Carrera> DeleteEstudiante(int id)
    {

        if (id <= 0)
        {
            _logger.LogError("Error al eliminar la carrera " +id);
            return BadRequest();

        }

        var obj = _db.Carreras.FirstOrDefault(u => u.CarreraId == id);

        if (obj == null)
        {
            return NotFound();

        }

        _db.Carreras.Remove(obj);
        _db.SaveChanges();

        return NoContent();

    }
    
    
    
    
    
    
    
}