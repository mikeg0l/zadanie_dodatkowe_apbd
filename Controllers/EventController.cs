using Microsoft.AspNetCore.Mvc;
using ZadanieDodatkowe.DTOs;
using ZadanieDodatkowe.Exceptions;
using ZadanieDodatkowe.Services;

namespace ZadanieDodatkowe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(IDbService dbService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createEventDto)
    {
        try
        {
            await dbService.CreateEventAsync(createEventDto);
            return Created();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("assign-speaker")]
    public async Task<IActionResult> AssignSpeaker([FromBody] AssignSpeakerDto assignSpeakerDto)
    {
        try
        {
            await dbService.AssignSpeakerToEventAsync(assignSpeakerDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register-attendee")]
    public async Task<IActionResult> RegisterAttendee([FromBody] RegisterAttendeeDto registerAttendeeDto)
    {
        try
        {
            await dbService.RegisterAttendeeAsync(registerAttendeeDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{eventId}/attendees/{attendeeId}")]
    public async Task<IActionResult> CancelRegistration(int eventId, int attendeeId)
    {
        try
        {
            await dbService.CancelRegistrationAsync(eventId, attendeeId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingEvents()
    {
        var events = await dbService.GetUpcomingEventsAsync();
        return Ok(events);
    }
}