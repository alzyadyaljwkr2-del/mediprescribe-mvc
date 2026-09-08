using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediPrescribe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacistsController : ControllerBase
    {
        private readonly IPharmacistService _pharmacistService;

        public PharmacistsController(IPharmacistService pharmacistService)
        {
            _pharmacistService = pharmacistService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PharmacistDto>>> GetPharmacists()
        {
            var pharmacists = await _pharmacistService.GetAllAsync();
            return Ok(pharmacists);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PharmacistDto>> GetPharmacist(int id)
        {
            var pharmacist = await _pharmacistService.GetByIdAsync(id);
            if (pharmacist == null) return NotFound(new { message = "الصيدلي غير موجود" });
            return Ok(pharmacist);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePharmacist(int id)
        {
            var deleted = await _pharmacistService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
