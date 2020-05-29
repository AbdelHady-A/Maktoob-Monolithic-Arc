using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maktoob.SPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUserProfileRepository userProfileRepository;

        public HomeController(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.userProfileRepository.GetAsync(new FindByIdSpec<UserProfile>(Guid.NewGuid()));

            if(result == null)
            {
                return Ok("nothing inside");
            }
            else
            {
                return Ok(result);
            }

        }
    }
}