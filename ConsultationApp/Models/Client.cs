using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

namespace ConsultationApp.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Не публичное свойство для хранения хеша пароля
        public string PasswordHash { get; set; } = string.Empty;

        // Метод для установки пароля (хеширование)
        public void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Метод для проверки пароля
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        // Навигационное свойство
        public List<Consultation> Consultations { get; set; } = new();
    }
}