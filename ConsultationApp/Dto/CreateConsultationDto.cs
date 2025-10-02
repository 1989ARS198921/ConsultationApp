using System.ComponentModel.DataAnnotations;

namespace ConsultationApp.Dto
{
    public class CreateConsultationDto
    {
        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ClientId должен быть положительным числом")]
        public int ClientId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ConsultantId должен быть положительным числом")]
        public int ConsultantId { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "booked";
    }
}