using Maktoob.Application.Commands;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Maktoob.Application.Users
{
    public class SignOutUserCommand : ICommand<GResult>
    {
        public Guid UserId { get; set; }
        public string JwtId { get; set; }
        public string RefreshToken { get; set; }
        public bool LogOutAll { get; set; }

        public class Handler : ICommandHandler<SignOutUserCommand, GResult>
        {
            private readonly ISignInService _signInService;

            public Handler(ISignInService signInService)
            {
                _signInService = signInService;
            }

            public async Task<GResult> HandleAsync(SignOutUserCommand command)
            {
                var result = await _signInService.SignOutAsync(command.UserId, command.JwtId,command.RefreshToken, command.LogOutAll);

                return result;
            }
        }
    }
}
