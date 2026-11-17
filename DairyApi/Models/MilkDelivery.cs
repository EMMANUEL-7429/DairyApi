using System.ComponentModel.DataAnnotations;

namespace DairyApi.Models
{
    public class MilkDelivery
    {
        public int DeliveryId { get; set; }

        [Required]
        public int FarmerId { get; set; } // Foreign key only

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public decimal QuantityLitres { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
