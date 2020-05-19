using Maktoob.Application.Commands;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Maktoob.Application.Users
{
    public class SignInUserCommand : ICommand<GResult>
    {
        [Required]
        public string Credentials { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool EmailCredentials { get; set; }
        public bool RememberMe { get; set; }
        public string OperatingSystem { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        public string UserAgent { get; set; }
        public string OperatingSystemVersion { get; set; }



        public class Handler : ICommandHandler<SignInUserCommand, GResult>
        {
            private readonly ISignInService _signInService;

            public Handler(ISignInService signInAsync)
            {
                _signInService = signInAsync;
            }

            public async Task<GResult> HandleAsync(SignInUserCommand command)
            {
                var userLogin = new UserLogin { 
                    Browser = command.Browser, 
                    Device = command.Device, 
                    OperatingSystem = command.OperatingSystem, 
                    UserAgent = command.UserAgent };

                var result = await _signInService.SignInAsync(command.Credentials, command.Password, userLogin, command.EmailCredentials);

                return result;
            }
        }
    }
}
