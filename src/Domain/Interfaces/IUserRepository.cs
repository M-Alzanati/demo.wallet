using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Exceptions
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken);
        Task AddUserAsync(User user, CancellationToken cancellationToken);
    }
}
