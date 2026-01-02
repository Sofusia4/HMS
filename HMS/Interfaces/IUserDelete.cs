using HMS.Models;

namespace HMS.Interfaces
{
    public interface IUserDelete
    {
        IEnumerable<UserDelete> GetAll();
        UserDelete GetByUserId(string userId);
        Task CreateAsync(UserDelete userDelete);
        Task DeleteAsync(UserDelete userDelete);
    }
}
