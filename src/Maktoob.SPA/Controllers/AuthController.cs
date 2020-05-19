using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Maktoob.Application;
using Maktoob.Application.Users;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.CrossCuttingConcerns.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Maktoob.SPA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Produces("application/json")]
    //[Consumes("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;
        public AuthController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        //[SwaggerResponse(200, typeof(GResult))]
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

        // Only for the purpose of checking
        [HttpGet]
        [Authorize]
        public OkResult IsAuthorized()
        {
            return Ok();
        }
    }
}
