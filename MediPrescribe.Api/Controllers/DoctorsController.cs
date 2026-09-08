using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor == null) return NotFound(new { message = "الطبيب غير موجود" });
        return Ok(doctor);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DoctorDto>> PostDoctor(DoctorDto doctor)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _doctorService.CreateAsync(doctor);

        return CreatedAtAction(nameof(GetDoctor), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutDoctor(int id, DoctorDto doctor)
    {
        if (id != doctor.Id) return BadRequest(new { message = "معرف الطبيب غير متطابق" });
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _doctorService.UpdateAsync(id, doctor);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var deleted = await _doctorService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}