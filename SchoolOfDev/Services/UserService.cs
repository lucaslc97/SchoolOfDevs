using Microsoft.EntityFrameworkCore;
using SchoolOfDev.Entities;
using SchoolOfDev.Helpers;

namespace SchoolOfDev.Services
{
    public interface IUserService
    {
        public Task<User> CreateUser(User user);
        public Task<User> GetById(int id);
        public Task<List<User>> GetAll();
        public Task Update(User userId, int id);
        public Task Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            User userDb = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName == user.UserName);

            if (userDb is not null) 
                throw new Exception($"UserName {user.UserName} already exist.");

            _context.Users.Add(userDb);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task Delete(int id)
        {
            User userDb = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
                throw new Exception($"User {id} not found.");

            _context.Users.Remove(userDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll() => await _context.Users.ToListAsync();

        public async Task<User> GetById(int id) 
        {
            User userDb = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
                throw new Exception($"User {id} not found.");

            return userDb;
        }

        public async Task Update(User userId, int id)
        {
            if (userId.Id != id)
                throw new Exception($"Rout id differs User Id");

            User userDb = await _context.Users
              .AsNoTracking()
              .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
                throw new Exception($"User {id} not found.");

            _context.Entry(userDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
