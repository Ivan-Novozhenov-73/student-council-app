using Microsoft.EntityFrameworkCore;
using StudentCouncilAPI.Data;
using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public NoteService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _context.Notes
                .Include(n => n.User)
                .Include(n => n.Event)
                .OrderByDescending(n => n.NoteId)
                .ToListAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(Guid id)
        {
            return await _context.Notes
                .Include(n => n.User)
                .Include(n => n.Event)
                .FirstOrDefaultAsync(n => n.NoteId == id);
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            note.NoteId = Guid.NewGuid();
            
            // Создаем путь к файлу
            var notesDirectory = Path.Combine(_environment.ContentRootPath, "Notes");
            if (!Directory.Exists(notesDirectory))
            {
                Directory.CreateDirectory(notesDirectory);
            }
            
            note.FilePath = Path.Combine("Notes", $"{note.NoteId}.md");
            
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNoteAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteNoteAsync(Guid id)
        {
            var note = await GetNoteByIdAsync(id);
            if (note == null) return false;

            // Удаляем файл
            var fullPath = Path.Combine(_environment.ContentRootPath, note.FilePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Note>> GetNotesByUserAsync(Guid userId)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId)
                .Include(n => n.Event)
                .OrderByDescending(n => n.NoteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetNotesByEventAsync(Guid eventId)
        {
            return await _context.Notes
                .Where(n => n.EventId == eventId)
                .Include(n => n.User)
                .OrderByDescending(n => n.NoteId)
                .ToListAsync();
        }

        public async Task<string> GetNoteContentAsync(Guid noteId)
        {
            var note = await GetNoteByIdAsync(noteId);
            if (note == null) return string.Empty;

            var fullPath = Path.Combine(_environment.ContentRootPath, note.FilePath);
            if (!File.Exists(fullPath)) return string.Empty;

            return await File.ReadAllTextAsync(fullPath);
        }

        public async Task<bool> SaveNoteContentAsync(Guid noteId, string content)
        {
            var note = await GetNoteByIdAsync(noteId);
            if (note == null) return false;

            var fullPath = Path.Combine(_environment.ContentRootPath, note.FilePath);
            var directory = Path.GetDirectoryName(fullPath);
            
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(fullPath, content);
            return true;
        }
    }
}
