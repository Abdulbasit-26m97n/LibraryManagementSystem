using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository(LMSDbContext context) : IUserRepository
    {
        public async Task<bool> UserExistsAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetUserAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(u=>u.Username == username);
        }

        public async Task<User?> GetUserByIdAsync(Guid UserId)
        {
            return await context.Users.FindAsync(UserId);
        }

        public async Task SaveRefreshTokenAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
