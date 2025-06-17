using Microsoft.EntityFrameworkCore;
using ZadanieDodatkowe.Data;
using ZadanieDodatkowe.DTOs;
using ZadanieDodatkowe.Exceptions;
using ZadanieDodatkowe.Models;

namespace ZadanieDodatkowe.Services;


public interface IDbService
{
    Task CreateEventAsync(CreateEventDto eventDto);
    Task AssignSpeakerToEventAsync(AssignSpeakerDto assignSpeakerDto);
    Task RegisterAttendeeAsync(RegisterAttendeeDto registerAttendeeDto);
    Task CancelRegistrationAsync(int eventId, int attendeeId);
    Task<IEnumerable<GetUpcomingEventDto>> GetUpcomingEventsAsync();
    Task<IEnumerable<GetAttendeeReportDto>> GetAttendeeParticipationReportAsync(int attendeeId);
}

public class DbService(AppDbContext db) : IDbService
{
    public async Task CreateEventAsync(CreateEventDto eventDto)
    {
        if (eventDto.StartDate <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Data wydarzenia nie może być przeszła.");
        }

        var newEvent = new Event
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            StartDate = eventDto.StartDate,
            MaxAttendees = eventDto.MaxAttendees,
            Attendees = new List<Attendee>(),
            Speakers = new List<Speaker>()
        };

        await db.Events.AddAsync(newEvent);
        await db.SaveChangesAsync();
    }

    public async Task AssignSpeakerToEventAsync(AssignSpeakerDto assignSpeakerDto)
    {
        var fetchedEvent = await db.Events
            .Include(e => e.Speakers)
            .FirstOrDefaultAsync(e => e.Id == assignSpeakerDto.EventId);

        if (fetchedEvent == null)
        {
            throw new NotFoundException("Wydarzenie nie zostało znalezione.");
        }

        var speaker = await db.Speakers
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == assignSpeakerDto.SpeakerId);

        if (speaker == null)
        {
            throw new NotFoundException("Prelegent nie został znaleziony.");
        }

        var isSpeakerBusy = speaker.Events.Any(e => e.StartDate == fetchedEvent.StartDate);
        if (isSpeakerBusy)
        {
            throw new InvalidOperationException(
                "Prelegent jest już przypisany do innego wydarzenia w tym samym czasie.");
        }

        fetchedEvent.Speakers.Add(speaker);
        await db.SaveChangesAsync();
    }

    public async Task RegisterAttendeeAsync(RegisterAttendeeDto registerAttendeeDto)
    {
        var fetchedEvent = await db.Events
            .Include(e => e.Attendees)
            .FirstOrDefaultAsync(e => e.Id == registerAttendeeDto.EventId);

        if (fetchedEvent == null)
        {
            throw new NotFoundException("Wydarzenie nie zostało znalezione.");
        }

        if (fetchedEvent.Attendees.Count >= fetchedEvent.MaxAttendees)
        {
            throw new InvalidOperationException("Osiągnięto limit miejsc na to wydarzenie.");
        }

        var attendee = await db.Attendees.FindAsync(registerAttendeeDto.AttendeeId);
        if (attendee == null)
        {
            throw new NotFoundException("Uczestnik nie został znaleziony.");
        }

        if (fetchedEvent.Attendees.Any(a => a.Id == attendee.Id))
        {
            throw new InvalidOperationException("Uczestnik jest już zarejestrowany na to wydarzenie.");
        }

        fetchedEvent.Attendees.Add(attendee);
        await db.SaveChangesAsync();
    }

    public async Task CancelRegistrationAsync(int eventId, int attendeeId)
    {
        var fetchedEvent = await db.Events
            .Include(e => e.Attendees)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (fetchedEvent == null)
        {
            throw new NotFoundException("Wydarzenie nie zostało znalezione.");
        }

        if (fetchedEvent.StartDate <= DateTime.UtcNow.AddHours(24))
        {
            throw new InvalidOperationException(
                "Nie można anulować rejestracji na mniej niż 24 godziny przed rozpoczęciem wydarzenia.");
        }

        var attendee = fetchedEvent.Attendees.FirstOrDefault(a => a.Id == attendeeId);
        if (attendee == null)
        {
            throw new NotFoundException("Uczestnik nie jest zarejestrowany na to wydarzenie.");
        }

        fetchedEvent.Attendees.Remove(attendee);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetUpcomingEventDto>> GetUpcomingEventsAsync()
    {
        return await db.Events
            .Where(e => e.StartDate > DateTime.UtcNow)
            .Include(e => e.Speakers)
            .Include(e => e.Attendees)
            .Select(e => new GetUpcomingEventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                RegisteredAttendees = e.Attendees.Count,
                FreeSpots = e.MaxAttendees - e.Attendees.Count,
                Speakers = e.Speakers.Select(s => new GetUpcomingEventSpeakerDto
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName
                }).ToList()
            }).ToListAsync();
    }

    public async Task<IEnumerable<GetAttendeeReportDto>> GetAttendeeParticipationReportAsync(int attendeeId)
    {
        var attendee = await db.Attendees
            .Include(a => a.Events)
            .ThenInclude(e => e.Speakers)
            .FirstOrDefaultAsync(a => a.Id == attendeeId);

        if (attendee == null)
        {
            throw new NotFoundException("Uczestnik nie został znaleziony.");
        }

        return attendee.Events
            .Where(e => e.StartDate < DateTime.UtcNow)
            .Select(e => new GetAttendeeReportDto
            {
                Title = e.Title,
                StartDate = e.StartDate,
                Description = e.Description,
                SpeakerLastNames = e.Speakers.Select(s => s.LastName).ToList()
            });
    }
}