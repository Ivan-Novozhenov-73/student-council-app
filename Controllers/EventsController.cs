using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.Services;
using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentCouncilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // Получить все события (фильтрация по статусу или userId)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents(
            [FromQuery] EventStatus? status = null,
            [FromQuery] Guid? userId = null)
        {
            IEnumerable<Event> events;
            if (userId.HasValue)
            {
                events = await _eventService.GetEventsByUserAsync(userId.Value);
            }
            else if (status.HasValue)
            {
                events = await _eventService.GetEventsByStatusAsync(status.Value);
            }
            else
            {
                events = await _eventService.GetAllEventsAsync();
            }
            return Ok(events.Select(EventMapper.ToDto));
        }

        // Получить событие по id
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(Guid id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
                return NotFound();
            return Ok(EventMapper.ToDto(eventItem));
        }

        // Создать событие (только Head, Chairman)
        [HttpPost]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            try
            {
                var createdEvent = await _eventService.CreateEventAsync(EventMapper.ToEntity(eventDto));
                return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.EventId }, EventMapper.ToDto(createdEvent));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Обновить событие (только Head, Chairman)
        [HttpPut("{id}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<EventDto>> UpdateEvent(Guid id, [FromBody] UpdateEventDto eventDto)
        {
            if (id != eventDto.EventId)
                return BadRequest();

            var eventEntity = await _eventService.GetEventByIdAsync(id);
            if (eventEntity == null)
                return NotFound();

            EventMapper.UpdateEntity(eventEntity, eventDto);
            var updatedEvent = await _eventService.UpdateEventAsync(eventEntity);
            return Ok(EventMapper.ToDto(updatedEvent));
        }

        // Удалить событие (только Chairman)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Chairman")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // Добавить участника к событию
        [HttpPost("{id}/participants")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> AddParticipant(Guid id, [FromBody] EventUserLinkDto request)
        {
            var result = await _eventService.AddUserToEventAsync(id, request.UserId, request.Role);
            if (!result)
                return BadRequest();
            return NoContent();
        }

        // Удалить участника из события
        [HttpDelete("{id}/participants/{userId}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> RemoveParticipant(Guid id, Guid userId)
        {
            var result = await _eventService.RemoveUserFromEventAsync(id, userId);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // Получить участников события
        [HttpGet("{id}/participants")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetParticipants(Guid id)
        {
            var participants = await _eventService.GetEventParticipantsAsync(id);
            return Ok(participants.Select(UserMapper.ToDto));
        }
    }
}