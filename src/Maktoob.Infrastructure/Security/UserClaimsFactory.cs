using Maktoob.CrossCuttingConcerns.Security;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Infrastructure.Security
{
    /// <summary>
    /// Provides methods to create a claims principal for a given user.
    /// </summary>
    public class UserClaimsFactory : IUserClaimsFactory
    {
        public async Task<IEnumerable<Claim>> CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new Claim[]
            {
                new Claim(JwtClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtClaimNames.UniqueName, user.Name),
                new Claim(JwtClaimNames.Jti, Guid.NewGuid().ToString())
            };
            return await Task.FromResult(claims);
        }
    }
}
