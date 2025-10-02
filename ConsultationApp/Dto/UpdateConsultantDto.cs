using System.ComponentModel.DataAnnotations;

namespace ConsultationApp.Dto
{
    public class UpdateConsultantDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; } = string.Empty;
    }
}