using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ConsultationApp.Data;
using ConsultationApp.Dto;
using ConsultationApp.Models;

namespace ConsultationApp.Services
{
    public class ConsultantService : IConsultantService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConsultantService> _logger;

        public ConsultantService(ApplicationDbContext context, ILogger<ConsultantService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ConsultantDto>> GetAllConsultantsAsync()
        {
            _logger.LogInformation("Получение списка всех консультантов");

            return await _context.Consultants
                .Select(c => new ConsultantDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Specialty = c.Specialty
                })
                .ToListAsync();
        }

        public async Task<ConsultantDto?> GetConsultantByIdAsync(int id)
        {
            _logger.LogInformation("Поиск консультанта с ID {ConsultantId}", id);

            var consultant = await _context.Consultants.FindAsync(id);
            if (consultant == null)
            {
                _logger.LogWarning("Консультант с ID {ConsultantId} не найден", id);
                return null;
            }

            return new ConsultantDto
            {
                Id = consultant.Id,
                Name = consultant.Name,
                Specialty = consultant.Specialty
            };
        }

        public async Task<ConsultantDto> CreateConsultantAsync(CreateConsultantDto dto)
        {
            _logger.LogInformation("Создание нового консультанта с именем {ConsultantName}", dto.Name);

            var consultant = new Consultant
            {
                Name = dto.Name,
                Specialty = dto.Specialty
            };

            _context.Consultants.Add(consultant);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Консультант с ID {ConsultantId} создан", consultant.Id);

            return new ConsultantDto
            {
                Id = consultant.Id,
                Name = consultant.Name,
                Specialty = consultant.Specialty
            };
        }

        public async Task<bool> UpdateConsultantAsync(int id, UpdateConsultantDto dto)
        {
            _logger.LogInformation("Обновление консультанта с ID {ConsultantId}", id);

            var consultant = await _context.Consultants.FindAsync(id);
            if (consultant == null)
            {
                _logger.LogWarning("Консультант с ID {ConsultantId} не найден при обновлении", id);
                return false;
            }

            consultant.Name = dto.Name;
            consultant.Specialty = dto.Specialty;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Консультант с ID {ConsultantId} обновлён", id);
            return true;
        }

        public async Task<bool> DeleteConsultantAsync(int id)
        {
            _logger.LogInformation("Удаление консультанта с ID {ConsultantId}", id);

            var consultant = await _context.Consultants.FindAsync(id);
            if (consultant == null)
            {
                _logger.LogWarning("Консультант с ID {ConsultantId} не найден при удалении", id);
                return false;
            }

            _context.Consultants.Remove(consultant);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Консультант с ID {ConsultantId} удалён", id);
            return true;
        }
    }
}