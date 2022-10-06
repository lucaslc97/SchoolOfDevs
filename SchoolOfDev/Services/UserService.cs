using Microsoft.EntityFrameworkCore;
using SchoolOfDev.Entities;
using SchoolOfDev.Helpers;
using BC = BCrypt.Net.BCrypt;

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
            if (!user.PassWord.Equals(user.ConfirmPassWord))
                throw new Exception($"PassWord does not match confirmPassword.");

            User userDb = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName == user.UserName);

            if (userDb is not null) 
                throw new Exception($"UserName {user.UserName} already exist.");

            user.PassWord = BC.HashPassword(user.PassWord);

            _context.Users.Add(user);
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

        public async Task Update(User user, int id)
        {
            if (user.Id != id)
                throw new Exception($"Rout id differs User Id");
            else if (!user.PassWord.Equals(user.ConfirmPassWord))
                throw new Exception($"PassWord does not match confirmPassword.");

            User userDb = await _context.Users
              .AsNoTracking()
              .SingleOrDefaultAsync(u => u.Id == id);

            if (userDb is null)
                throw new Exception($"User {id} not found.");
            else if (!BC.Verify(user.CurrentPassWord, userDb.PassWord))
                throw new Exception($"Incorrect Password");

            user.CreatedAt = userDb.CreatedAt;
            user.PassWord = BC.HashPassword(user.PassWord);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
