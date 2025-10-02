namespace ConsultationApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Навигационное свойство
        public List<Consultation> Consultations { get; set; } = new();
    }
}