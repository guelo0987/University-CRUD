using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly string _stripeSecretKey;

        public StripeController(IConfiguration configuration)
        {
            _stripeSecretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        [HttpPost("CreatePaymentIntent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentCreateRequest request)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = "dop",
                    PaymentMethod = request.PaymentMethodId,
                    Confirm = true,
                    OffSession = true
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                return Ok(new { Success = true, PaymentIntentId = paymentIntent.Id });
            }
            catch (StripeException e)
            {
                return BadRequest(new { Success = false, Error = e.Message });
            }
        }
    }

    public class PaymentIntentCreateRequest
    {
        public long Amount { get; set; }
        public string PaymentMethodId { get; set; }
    }
}