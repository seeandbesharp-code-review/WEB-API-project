using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User newUser);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<IEnumerable<User>> GetUsers();
        Task UpdateUser(int id, User updateUser);
        Task<bool> UserWithSameEmail(string email, int id);
    }
}
