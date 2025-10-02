using Microsoft.AspNetCore.Mvc;
using ConsultationApp.Dto;
using ConsultationApp.Services;

namespace ConsultationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultantsController : ControllerBase
    {
        private readonly IConsultantService _consultantService;

        public ConsultantsController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultantDto>>> GetConsultants()
        {
            var consultants = await _consultantService.GetAllConsultantsAsync();
            return Ok(consultants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultantDto>> GetConsultant(int id)
        {
            var consultant = await _consultantService.GetConsultantByIdAsync(id);
            if (consultant == null)
            {
                return NotFound();
            }
            return Ok(consultant);
        }

        [HttpPost]
        public async Task<ActionResult<ConsultantDto>> CreateConsultant([FromBody] CreateConsultantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _consultantService.CreateConsultantAsync(dto);
            return CreatedAtAction(nameof(GetConsultant), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultant(int id, [FromBody] UpdateConsultantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _consultantService.UpdateConsultantAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultant(int id)
        {
            var success = await _consultantService.DeleteConsultantAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}