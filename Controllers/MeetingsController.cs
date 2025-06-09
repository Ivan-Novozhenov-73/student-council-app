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
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingDto>>> GetMeetings(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] Guid? userId = null)
        {
            IEnumerable<Meeting> meetings;
            if (startDate.HasValue && endDate.HasValue)
            {
                meetings = await _meetingService.GetMeetingsByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else if (userId.HasValue)
            {
                meetings = await _meetingService.GetMeetingsByUserAsync(userId.Value);
            }
            else
            {
                meetings = await _meetingService.GetAllMeetingsAsync();
            }

            return Ok(meetings.Select(MeetingMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingDto>> GetMeeting(Guid id)
        {
            var meeting = await _meetingService.GetMeetingByIdAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }
            return Ok(MeetingMapper.ToDto(meeting));
        }

        [HttpPost]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<MeetingDto>> CreateMeeting([FromBody] CreateMeetingDto meetingDto)
        {
            try
            {
                var createdMeeting = await _meetingService.CreateMeetingAsync(MeetingMapper.ToEntity(meetingDto));
                return CreatedAtAction(nameof(GetMeeting), new { id = createdMeeting.MeetingId }, MeetingMapper.ToDto(createdMeeting));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<MeetingDto>> UpdateMeeting(Guid id, [FromBody] UpdateMeetingDto meetingDto)
        {
            if (id != meetingDto.MeetingId)
            {
                return BadRequest();
            }

            try
            {
                var meetingEntity = await _meetingService.GetMeetingByIdAsync(id);
                if (meetingEntity == null)
                    return NotFound();

                MeetingMapper.UpdateEntity(meetingEntity, meetingDto);
                var updatedMeeting = await _meetingService.UpdateMeetingAsync(meetingEntity);
                return Ok(MeetingMapper.ToDto(updatedMeeting));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> DeleteMeeting(Guid id)
        {
            var result = await _meetingService.DeleteMeetingAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("{id}/participants")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> AddParticipant(Guid id, [FromBody] AddMeetingParticipantRequestDto request)
        {
            var result = await _meetingService.AddUserToMeetingAsync(id, request.UserId, request.Role);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}/participants/{userId}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> RemoveParticipant(Guid id, Guid userId)
        {
            var result = await _meetingService.RemoveUserFromMeetingAsync(id, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

    // DTO для добавления участника
    public class AddMeetingParticipantRequestDto
    {
        public Guid UserId { get; set; }
        public MeetingUserRole Role { get; set; }
    }
}