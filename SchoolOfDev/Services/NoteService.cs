using SchoolOfDev.Entities;
using SchoolOfDev.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SchoolOfDev.Services
{
    public interface INoteService
    {
        public Task<Note> CreateNote(Note note);
        public Task<Note> GetById(int id);
        public Task<List<Note>> GetAll();
        public Task Update(Note noteId, int id);
        public Task Delete(int id);
    }

    public class NoteService : INoteService
    {
        private readonly DataContext _context;

        public NoteService(DataContext context)
        {
            _context = context;
        }

        public async Task<Note> CreateNote(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task Delete(int id)
        {
            Note noteDb = await _context.Notes
                .SingleOrDefaultAsync(u => u.Id == id);

            if (noteDb is null)
                throw new Exception($"User {id} not found.");

            _context.Notes.Remove(noteDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Note>> GetAll() => await _context.Notes.ToListAsync();

        public async Task<Note> GetById(int id)
        {
            Note noteDb = await _context.Notes
                .SingleOrDefaultAsync(u => u.Id == id);

            if (noteDb is null)
                throw new Exception($"User {id} not found.");

            return noteDb;
        }

        public async Task Update(Note note, int id)
        {
            if (note.Id != id)
                throw new Exception($"Rout id differs User Id");

            Note noteDb = await _context.Notes
              .AsNoTracking()
              .SingleOrDefaultAsync(u => u.Id == id);

            if (noteDb is null)
                throw new Exception($"User {id} not found.");

            note.CreatedAt = noteDb.CreatedAt;

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
