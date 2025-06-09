using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.Services;
using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Mappers;
using System.Security.Claims;

namespace StudentCouncilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes(
            [FromQuery] Guid? userId = null,
            [FromQuery] Guid? eventId = null)
        {
            IEnumerable<Note> notes;

            if (userId.HasValue)
            {
                notes = await _noteService.GetNotesByUserAsync(userId.Value);
            }
            else if (eventId.HasValue)
            {
                notes = await _noteService.GetNotesByEventAsync(eventId.Value);
            }
            else
            {
                notes = await _noteService.GetAllNotesAsync();
            }

            return Ok(notes.Select(NoteMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetNote(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(NoteMapper.ToDto(note));
        }

        [HttpGet("{id}/content")]
        public async Task<ActionResult<string>> GetNoteContent(Guid id)
        {
            var content = await _noteService.GetNoteContentAsync(id);
            return Ok(new { content });
        }

        [HttpPost]
        public async Task<ActionResult<NoteDto>> CreateNote([FromBody] CreateNoteDto request)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized();
                }

                var note = NoteMapper.FromCreateDto(request);
                note.UserId = Guid.Parse(currentUserId);

                var createdNote = await _noteService.CreateNoteAsync(note);

                if (!string.IsNullOrEmpty(request.Content))
                {
                    await _noteService.SaveNoteContentAsync(createdNote.NoteId, request.Content);
                }

                return CreatedAtAction(nameof(GetNote), new { id = createdNote.NoteId }, NoteMapper.ToDto(createdNote));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NoteDto>> UpdateNote(Guid id, [FromBody] UpdateNoteDto request)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                if (note == null)
                {
                    return NotFound();
                }

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserId != note.UserId.ToString() && currentUserRole != "Chairman")
                {
                    return Forbid();
                }

                NoteMapper.UpdateEntity(note, request);

                var updatedNote = await _noteService.UpdateNoteAsync(note);

                if (request.Content != null)
                {
                    await _noteService.SaveNoteContentAsync(id, request.Content);
                }

                return Ok(NoteMapper.ToDto(updatedNote));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserId != note.UserId.ToString() && currentUserRole != "Chairman")
            {
                return Forbid();
            }

            var result = await _noteService.DeleteNoteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}