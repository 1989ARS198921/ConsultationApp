using System.ComponentModel.DataAnnotations;

namespace ConsultationApp.Models
{
    public class Consultation
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int ClientId { get; set; }
        public int ConsultantId { get; set; }
        public string Status { get; set; } = "booked";


        // Навигационные свойства
        [Required]
        public Client Client { get; set; } = null!;
        [Required]
        public Consultant Consultant { get; set; } = null!;
    }
}
