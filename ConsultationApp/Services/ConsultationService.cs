using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ConsultationApp.Data;
using ConsultationApp.Dto;
using ConsultationApp.Models;

namespace ConsultationApp.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConsultationService> _logger;

        public ConsultationService(ApplicationDbContext context, ILogger<ConsultationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ConsultationDto>> GetAllConsultationsAsync()
        {
            _logger.LogInformation("Получение списка всех консультаций");

            return await _context.Consultations
                .Select(c => new ConsultationDto
                {
                    Id = c.Id,
                    DateTime = c.DateTime,
                    ClientId = c.ClientId,
                    ConsultantId = c.ConsultantId,
                    Status = c.Status
                })
                .ToListAsync();
        }

        public async Task<ConsultationDto?> GetConsultationByIdAsync(int id)
        {
            _logger.LogInformation("Поиск консультации с ID {ConsultationId}", id);

            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null)
            {
                _logger.LogWarning("Консультация с ID {ConsultationId} не найдена", id);
                return null;
            }

            return new ConsultationDto
            {
                Id = consultation.Id,
                DateTime = consultation.DateTime,
                ClientId = consultation.ClientId,
                ConsultantId = consultation.ConsultantId,
                Status = consultation.Status
            };
        }

        public async Task<ConsultationDto> CreateConsultationAsync(CreateConsultationDto dto)
        {
            _logger.LogInformation("Создание новой консультации для клиента {ClientId}", dto.ClientId);

            if (dto.DateTime < DateTime.Now)
            {
                _logger.LogWarning("Попытка создания консультации на прошедшую дату {DateTime}", dto.DateTime);
                throw new ArgumentException("Дата консультации не может быть в прошлом.");
            }

            var consultation = new Consultation
            {
                DateTime = dto.DateTime,
                ClientId = dto.ClientId,
                ConsultantId = dto.ConsultantId,
                Status = dto.Status
            };

            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Консультация создана с ID {ConsultationId}", consultation.Id);

            return new ConsultationDto
            {
                Id = consultation.Id,
                DateTime = consultation.DateTime,
                ClientId = consultation.ClientId,
                ConsultantId = consultation.ConsultantId,
                Status = consultation.Status
            };
        }

        public async Task<bool> UpdateConsultationAsync(int id, UpdateConsultationDto dto)
        {
            _logger.LogInformation("Обновление консультации с ID {ConsultationId}", id);

            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null)
            {
                _logger.LogWarning("Консультация с ID {ConsultationId} не найдена при обновлении", id);
                return false;
            }

            consultation.DateTime = dto.DateTime;
            consultation.ClientId = dto.ClientId;
            consultation.ConsultantId = dto.ConsultantId;
            consultation.Status = dto.Status;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Консультация с ID {ConsultationId} обновлена", id);
            return true;
        }

        public async Task<bool> DeleteConsultationAsync(int id)
        {
            _logger.LogInformation("Удаление консультации с ID {ConsultationId}", id);

            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null)
            {
                _logger.LogWarning("Консультация с ID {ConsultationId} не найдена при удалении", id);
                return false;
            }

            _context.Consultations.Remove(consultation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Консультация с ID {ConsultationId} удалена", id);
            return true;
        }
    }
}