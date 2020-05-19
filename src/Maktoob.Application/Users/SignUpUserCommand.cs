using Maktoob.Application.Commands;
using System.Threading.Tasks;
using Maktoob.Domain.Entities;
using Maktoob.CrossCuttingConcerns.Result;
using System.ComponentModel.DataAnnotations;
using Maktoob.Domain.Services;

namespace Maktoob.Application.Users
{
    public class SignUpUserCommand : ICommand<GResult>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public class Handler : ICommandHandler<SignUpUserCommand, GResult>
        {
            private readonly IUserService _userService;
            private readonly ISignInService _signInService;

            public Handler(IUserService userService, ISignInService signInService)
            {
                _userService = userService;
                this._signInService = signInService;
            }

            public async Task<GResult> HandleAsync(SignUpUserCommand command)
            {
                var user = new User
                {
                    Email = command.Email,
                    Name = command.UserName
                };

                user.PasswordHash = _userService.PasswordHasher.Hash(command.Password);

                var result = await _userService.CreateAsync(user);

                return result;
            }
        }
    }
    
}
