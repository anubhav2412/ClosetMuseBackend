using ClosetMuseBackend.Models;

namespace ClosetMuseBackend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string userId);
        Task<List<User>> GetAllUsersAsync();
    }
}
