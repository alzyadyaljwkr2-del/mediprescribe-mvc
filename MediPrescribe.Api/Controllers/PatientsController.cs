using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<PatientDto>> GetPatient(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient == null) return NotFound(new { message = "المريض غير موجود" });
        return Ok(patient);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PatientDto>> PostPatient(PatientDto patient)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _patientService.CreateAsync(patient);

        return CreatedAtAction(nameof(GetPatient), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutPatient(int id, PatientDto patient)
    {
        if (id != patient.Id) return BadRequest(new { message = "معرف المريض غير متطابق" });
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _patientService.UpdateAsync(id, patient);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var deleted = await _patientService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}