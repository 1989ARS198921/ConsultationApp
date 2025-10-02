using ConsultationApp.Dto;

namespace ConsultationApp.Services
{
    public interface IConsultationService
    {
        Task<IEnumerable<ConsultationDto>> GetAllConsultationsAsync();
        Task<ConsultationDto?> GetConsultationByIdAsync(int id);
        Task<ConsultationDto> CreateConsultationAsync(CreateConsultationDto dto);
        Task<bool> UpdateConsultationAsync(int id, UpdateConsultationDto dto);
        Task<bool> DeleteConsultationAsync(int id);
    }
}