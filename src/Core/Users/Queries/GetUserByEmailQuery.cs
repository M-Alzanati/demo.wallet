using System.Threading;
using System.Threading.Tasks;
using Application.Users.Dtos;
using Common;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Users.Queries
{
    public class ValidateUserQuery : IRequest<UserDto>
    {
        public string Email { get; }
        public string Password { get; }

        public ValidateUserQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class ValidateUserQueryHandler : IRequestHandler<ValidateUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public ValidateUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user == null)
                return await Task.FromResult<UserDto>(null);

            var inputHash = PasswordHelper.ComputeSha256Hash(request.Password);

            if (user.PasswordHash != inputHash)
                return await Task.FromResult<UserDto>(null);

            return await Task.FromResult(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
            });
        }
    }
}
