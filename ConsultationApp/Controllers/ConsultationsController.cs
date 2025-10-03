using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultationApp.Dto;
using ConsultationApp.Services;

namespace ConsultationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationService _consultationService;

        public ConsultationsController(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        [HttpGet]
        [Authorize] // ← Защищённый маршрут
        public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetConsultations()
        {
            var consultations = await _consultationService.GetAllConsultationsAsync();
            return Ok(consultations);
        }

        [HttpGet("{id}")]
        [Authorize] // ← Защищённый маршрут
        public async Task<ActionResult<ConsultationDto>> GetConsultation(int id)
        {
            var consultation = await _consultationService.GetConsultationByIdAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            return Ok(consultation);
        }

        [HttpPost]
        [Authorize] // ← Защищённый маршрут
        public async Task<ActionResult<ConsultationDto>> CreateConsultation([FromBody] CreateConsultationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _consultationService.CreateConsultationAsync(dto);
            return CreatedAtAction(nameof(GetConsultation), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize] // ← Защищённый маршрут
        public async Task<IActionResult> UpdateConsultation(int id, [FromBody] UpdateConsultationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _consultationService.UpdateConsultationAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize] // ← Защищённый маршрут
        public async Task<IActionResult> DeleteConsultation(int id)
        {
            var success = await _consultationService.DeleteConsultationAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}