using System.ComponentModel.DataAnnotations;

namespace DairyApi.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FarmerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string NationalId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ICollection<MilkDelivery> MilkDeliveries { get; set; } = new List<MilkDelivery>();
    }
}
