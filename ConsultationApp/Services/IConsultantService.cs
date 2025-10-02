using ConsultationApp.Dto;

namespace ConsultationApp.Services
{
    public interface IConsultantService
    {
        Task<IEnumerable<ConsultantDto>> GetAllConsultantsAsync();
        Task<ConsultantDto?> GetConsultantByIdAsync(int id);
        Task<ConsultantDto> CreateConsultantAsync(CreateConsultantDto dto);
        Task<bool> UpdateConsultantAsync(int id, UpdateConsultantDto dto);
        Task<bool> DeleteConsultantAsync(int id);
    }
}