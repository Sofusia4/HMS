using HMS.Data;
using HMS.Interfaces;
using HMS.Models;

namespace HMS.Repositories
{
    public class UserDeleteRepository : IUserDelete
    {
        private readonly ApplicationContext _context;

        public UserDeleteRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserDelete userDelete)
        {
            if (userDelete != null)
            {
                if (!_context.UserDeletes.Any(e => e.UserId == userDelete.UserId))
                {
                    await _context.UserDeletes.AddAsync(userDelete);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAsync(UserDelete userDelete)
        {
            if (userDelete != null)
            {
                _context.UserDeletes.Remove(userDelete);
                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<UserDelete> GetAll()
        {
            return _context.UserDeletes;
        }

        public UserDelete GetByUserId(string userId)
        {
            return _context.UserDeletes.FirstOrDefault(e => e.UserId == userId);
        }
    }
}
