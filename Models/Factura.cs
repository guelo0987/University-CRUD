using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models
{
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFactura { get; set; }

        public int CodigoEstudiante { get; set; }
        [ForeignKey("CodigoEstudiante")]
        public virtual Estudiante Estudiante { get; set; }

        public string Periodo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoTotal { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        public string? Estado { get; set; }

        // Campos adicionales para Stripe
        public string? StripePaymentIntentId { get; set; }
        public string? StripeCustomerId { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? MetodoPago { get; set; }
        public string? UltimosDigitosTarjeta { get; set; }
        public string? MarcaTarjeta { get; set; }
    }
}