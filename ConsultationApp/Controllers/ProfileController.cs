using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultationApp.Services;
using ConsultationApp.Dto;

namespace ConsultationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ← Защищаем весь контроллер
    public class ProfileController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IConsultationService _consultationService;

        public ProfileController(IClientService clientService, IConsultationService consultationService)
        {
            _clientService = clientService;
            _consultationService = consultationService;
        }

        [HttpGet]
        public async Task<ActionResult<ClientDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);
            var client = await _clientService.GetClientByIdAsync(userId);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet("consultations")]
        public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetMyConsultations()
        {
            var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);
            var consultations = await _consultationService.GetAllConsultationsAsync();
            var myConsultations = consultations.Where(c => c.ClientId == userId);
            return Ok(myConsultations);
        }
    }
}