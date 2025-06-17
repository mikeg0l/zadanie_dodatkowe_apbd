using Microsoft.AspNetCore.Mvc;
using ZadanieDodatkowe.Exceptions;
using ZadanieDodatkowe.Services;

namespace ZadanieDodatkowe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendeeController(IDbService dbService) : ControllerBase
{
    [HttpGet("{attendeeId}/report")]
        public async Task<IActionResult> GetAttendeeReport(int attendeeId)
        {
            try
            {
                var report = await dbService.GetAttendeeParticipationReportAsync(attendeeId);
                return Ok(report);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
}