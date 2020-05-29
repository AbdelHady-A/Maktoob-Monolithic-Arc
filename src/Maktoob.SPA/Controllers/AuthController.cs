using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Maktoob.Application;
using Maktoob.Application.Users;
using Maktoob.CrossCuttingConcerns.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maktoob.SPA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;
        public AuthController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserCommand command)
        {
            var result = await _dispatcher.DispatchAsync(command);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInUserCommand command)
        {
            var result = await _dispatcher.DispatchAsync(command);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _dispatcher.DispatchAsync(command);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignOut([FromBody] SignOutUserCommand command)
        {
            command.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            command.JwtId = User.FindFirstValue(JwtClaimNames.Jti);
            var result = await _dispatcher.DispatchAsync(command);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
