using Maktoob.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Maktoob.Domain.Infrastructure
{
    /// <summary>
    /// Provides an abstraction for a factory to create a <see cref="ClaimsPrincipal"/> from a user.
    /// </summary>
    public interface IUserClaimsFactory
    {
        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsPrincipal"/>.</returns>
        Task<IEnumerable<Claim>> CreateAsync(User user);
    }
}
