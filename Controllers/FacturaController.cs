using CRUD.Context;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<FacturaController> _logger;
        private readonly IConfiguration _configuration;

        public FacturaController(MyDbContext db, ILogger<FacturaController> logger, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost("generarFactura")]
        public async Task<IActionResult> GenerarFactura([FromBody] FacturaRequest request)
        {
            var cuentasPorPagar = await _db.CuentaPorPagars
                .Include(cp => cp.EstudianteMateria)
                .ThenInclude(em => em.Materias)
                .Where(c => c.EstudianteMateria.PeriodoCursado == request.Periodo && c.CodigoEstudiante == request.CodigoEstudiante && c.MontoTotalaPagar > 0)
                .ToListAsync();

            if (!cuentasPorPagar.Any())
            {
                return Ok(new { Success = false, Message = "No se encontraron cuentas por pagar pendientes para el estudiante en el periodo especificado." });
            }

            decimal montoTotal = cuentasPorPagar.Sum(c => c.MontoTotalaPagar ?? 0);

            var nuevaFactura = new Factura
            {
                CodigoEstudiante = request.CodigoEstudiante,
                Periodo = request.Periodo,
                MontoTotal = montoTotal,
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente"
            };

            _db.Facturas.Add(nuevaFactura);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Factura creada exitosamente");

            return Ok(new { Success = true, FacturaId = nuevaFactura.IdFactura, MontoTotal = montoTotal });
        }

        [HttpPost("procesarPago")]
        public async Task<IActionResult> ProcesarPago([FromBody] PagoRequest request)
        {
            var factura = await _db.Facturas.FindAsync(request.FacturaId);
            if (factura == null)
            {
                return NotFound("Factura no encontrada");
            }

            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(factura.MontoTotal * 100),
                    Currency = "dop",
                    PaymentMethod = request.PaymentMethodId,
                    Confirm = true,
                    OffSession = true
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                factura.StripePaymentIntentId = paymentIntent.Id;
                factura.Estado = paymentIntent.Status;
                factura.FechaPago = DateTime.UtcNow;
                factura.MetodoPago = paymentIntent.PaymentMethodTypes[0];

                if (!string.IsNullOrEmpty(paymentIntent.PaymentMethodId))
                {
                    var paymentMethodService = new PaymentMethodService();
                    var paymentMethod = await paymentMethodService.GetAsync(paymentIntent.PaymentMethodId);
                    factura.UltimosDigitosTarjeta = paymentMethod.Card.Last4;
                    factura.MarcaTarjeta = paymentMethod.Card.Brand;
                }

                // Actualizar las cuentas por pagar
                var cuentasPorPagar = await _db.CuentaPorPagars
                    .Where(c => c.CodigoEstudiante == factura.CodigoEstudiante && c.EstudianteMateria.PeriodoCursado == factura.Periodo)
                    .ToListAsync();

                foreach (var cuenta in cuentasPorPagar)
                {
                    cuenta.MontoTotalaPagar = 0;
                    cuenta.Estado = "Pagado";
                }

                await _db.SaveChangesAsync();

                return Ok(new { Success = true, PaymentIntentId = paymentIntent.Id });
            }
            catch (StripeException e)
            {
                return BadRequest(new { Success = false, Error = e.Message });
            }
        }

        [HttpGet("verificarDeuda")]
        public async Task<IActionResult> VerificarDeuda(int codigoEstudiante, string periodo)
        {
            var cuentasPorPagar = await _db.CuentaPorPagars
                .Where(c => c.CodigoEstudiante == codigoEstudiante && c.EstudianteMateria.PeriodoCursado == periodo && c.MontoTotalaPagar > 0)
                .ToListAsync();

            if (!cuentasPorPagar.Any())
            {
                return Ok(new { TieneDeuda = false, Mensaje = "No tiene deudas pendientes para este período." });
            }

            var montoTotal = cuentasPorPagar.Sum(c => c.MontoTotalaPagar ?? 0);
            return Ok(new { TieneDeuda = true, MontoTotal = montoTotal });
        }

        [HttpGet("obtenerFactura")]
        public async Task<IActionResult> ObtenerFactura(int facturaId)
        {
            var factura = await _db.Facturas
                .Include(f => f.Estudiante)
                .FirstOrDefaultAsync(f => f.IdFactura == facturaId);

            if (factura == null)
            {
                return NotFound("Factura no encontrada");
            }

            return Ok(factura);
        }

        [HttpGet("obtenerFacturasEstudiante")]
        public async Task<IActionResult> ObtenerFacturasEstudiante(int codigoEstudiante)
        {
            var facturas = await _db.Facturas
                .Where(f => f.CodigoEstudiante == codigoEstudiante)
                .OrderByDescending(f => f.FechaCreacion)
                .ToListAsync();

            if (!facturas.Any())
            {
                return NotFound("No se encontraron facturas para el estudiante especificado");
            }

            return Ok(facturas);
        }
    }

    public class FacturaRequest
    {
        public string Periodo { get; set; }
        public int CodigoEstudiante { get; set; }
    }

    public class PagoRequest
    {
        public int FacturaId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}