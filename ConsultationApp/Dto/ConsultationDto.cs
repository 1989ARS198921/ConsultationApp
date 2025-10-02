using System.ComponentModel.DataAnnotations;

namespace ConsultationApp.Dto
{
    public class ConsultationDto
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public int ClientId { get; set; }

        public int ConsultantId { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}