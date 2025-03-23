using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserAsync(string username);
        Task<User?> GetUserByIdAsync(Guid UserId);
        Task SaveRefreshTokenAsync();
        Task<bool> UserExistsAsync(string username);
    }
}
