using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ConsultationApp.Data;
using ConsultationApp.Dto;
using ConsultationApp.Models;

namespace ConsultationApp.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientService> _logger;

        public ClientService(ApplicationDbContext context, ILogger<ClientService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            _logger.LogInformation("Получение списка всех клиентов");

            return await _context.Clients
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email
                })
                .ToListAsync();
        }

        public async Task<ClientDto?> GetClientByIdAsync(int id)
        {
            _logger.LogInformation("Поиск клиента с ID {ClientId}", id);

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                _logger.LogWarning("Клиент с ID {ClientId} не найден", id);
                return null;
            }

            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email
            };
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientDto dto)
        {
            _logger.LogInformation("Создание нового клиента с именем {ClientName}", dto.Name);

            var client = new Client
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Клиент с ID {ClientId} создан", client.Id);

            return new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email
            };
        }

        public async Task<bool> UpdateClientAsync(int id, UpdateClientDto dto)
        {
            _logger.LogInformation("Обновление клиента с ID {ClientId}", id);

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                _logger.LogWarning("Клиент с ID {ClientId} не найден при обновлении", id);
                return false;
            }

            client.Name = dto.Name;
            client.Email = dto.Email;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Клиент с ID {ClientId} обновлён", id);
            return true;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            _logger.LogInformation("Удаление клиента с ID {ClientId}", id);

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                _logger.LogWarning("Клиент с ID {ClientId} не найден при удалении", id);
                return false;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Клиент с ID {ClientId} удалён", id);
            return true;
        }
    }
}