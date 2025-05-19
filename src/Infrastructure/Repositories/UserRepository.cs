using Domain.Exceptions;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WalletContext _context;

        public UserRepository(WalletContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken)
        {
            return _context.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }

        public Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
