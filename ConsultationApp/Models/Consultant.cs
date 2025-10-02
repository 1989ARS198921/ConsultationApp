namespace ConsultationApp.Models
{
    public class Consultant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;

        // Навигационное свойство
        public List<Consultation> Consultations { get; set; } = new();
    }
}