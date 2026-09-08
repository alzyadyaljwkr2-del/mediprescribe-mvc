using MediPrescribe.Application.DTOs;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;
    private readonly IPatientService _patientService;

    public PrescriptionsController(IPrescriptionService prescriptionService, IPatientService patientService)
    {
        _prescriptionService = prescriptionService;
        _patientService = patientService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor,Pharmacist")]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptions()
    {
        var prescriptions = await _prescriptionService.GetAllAsync();
        return Ok(prescriptions);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetMyPrescriptions()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
        if (!int.TryParse(userIdStr, out var userId)) 
            return Unauthorized();

        var patient = await _patientService.GetByUserIdAsync(userId);
        if (patient == null) 
            return NotFound(new { message = "المريض غير موجود" });

        var allPrescriptions = await _prescriptionService.GetAllAsync();
        var myPrescriptions = allPrescriptions.Where(p => p.PatientId == patient.Id).ToList();

        return Ok(myPrescriptions);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Doctor,Pharmacist,Patient")]
    public async Task<ActionResult<PrescriptionDto>> GetPrescription(int id)
    {
        var prescription = await _prescriptionService.GetByIdAsync(id);
        if (prescription == null) return NotFound(new { message = "الوصفة الطبية غير موجودة" });

        if (User.IsInRole("Patient"))
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserId");
            if (int.TryParse(userIdStr, out var userId))
            {
                var patient = await _patientService.GetByUserIdAsync(userId);
                if (patient == null || prescription.PatientId != patient.Id)
                {
                    return Forbid();
                }
            }
            else
            {
                return Forbid();
            }
        }

        return Ok(prescription);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<ActionResult<PrescriptionDto>> PostPrescription(CreatePrescriptionDto prescription)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var created = await _prescriptionService.CreateAsync(prescription);
            return CreatedAtAction(nameof(GetPrescription), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> PutPrescription(int id, UpdatePrescriptionDto prescription)
    {
        if (id != prescription.Id) return BadRequest(new { message = "معرف الوصفة غير متطابق" });
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await _prescriptionService.UpdateAsync(id, prescription);
            if (!updated) return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePrescription(int id)
    {
        var deleted = await _prescriptionService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/dispense")]
    [Authorize(Roles = "Pharmacist,Admin")]
    public async Task<IActionResult> DispensePrescription(int id)
    {
        var prescription = await _prescriptionService.GetByIdAsync(id);
        if (prescription == null)
        {
            return NotFound();
        }

        if (prescription.Status == PrescriptionStatus.Dispensed)
        {
            return Conflict(new { message = "الروشتة مصروفة مسبقاً" });
        }

        var updateDto = new UpdatePrescriptionDto
        {
            DoctorId = prescription.DoctorId,
            PatientId = prescription.PatientId,
            MedicationDetails = prescription.MedicationDetails,
            ImageUrl = prescription.ImageUrl,
            Status = PrescriptionStatus.Dispensed
        };

        await _prescriptionService.UpdateAsync(id, updateDto);

        return Ok(new { message = "تم صرف الروشتة بنجاح" });
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Doctor,Pharmacist")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var all = await _prescriptionService.GetAllAsync();
        var results = all.Where(p => 
            p.MedicationDetails.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Id.ToString() == query
        ).ToList();

        return Ok(results);
    }
}