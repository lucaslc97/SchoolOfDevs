using Microsoft.EntityFrameworkCore;
using SchoolOfDev.Entities;
using SchoolOfDev.Helpers;

namespace SchoolOfDev.Services
{
    public interface ICourseService
    {
        public Task<Course> CreateCourse(Course user);
        public Task<Course> GetById(int id);
        public Task<List<Course>> GetAll();
        public Task Update(Course userId, int id);
        public Task Delete(int id);
    }

    public class CourseService : ICourseService
    {
        private readonly DataContext _context;

        public CourseService(DataContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateCourse(Course course)
        {
            Course courseDb = await _context.Courses
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Name == course.Name);

            if (courseDb is not null)
                throw new Exception($"Course {course.Name} already exist.");

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task Delete(int id)
        {
            Course courseDb = await _context.Courses
                .SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
                throw new Exception($"User {id} not found.");

            _context.Courses.Remove(courseDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetAll() => await _context.Courses.ToListAsync();

        public async Task<Course> GetById(int id)
        {
            Course courseDb = await _context.Courses
                .SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
                throw new Exception($"Course {id} not found.");

            return courseDb;
        }

        public async Task Update(Course course, int id)
        {
            if (course.Id != id)
                throw new Exception($"Rout id differs User Id");

            Course courseDb = await _context.Courses
              .AsNoTracking()
              .SingleOrDefaultAsync(u => u.Id == id);

            if (courseDb is null)
                throw new Exception($"User {id} not found.");

            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
