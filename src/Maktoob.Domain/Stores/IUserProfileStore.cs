using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maktoob.Domain.Stores
{
    /// <summary>
    /// Provides an abstraction for a store which manages user profiles.
    /// </summary>
    /// <typeparam name="TUserProfile">The type encapsulating a user profile.</typeparam>
    /// <typeparam name="TKey">The type representing a key.</typeparam>
    public interface IUserProfileStore<TUserProfile> : IDisposable
    where TUserProfile : class
    {
        /// <summary>
        /// Gets the userprofile identifier for specified <paramref name="userProfile"/>
        /// </summary>
        Task<string> GetUserProfileIdAsync(TUserProfile userProfile, CancellationToken cancellationToken = default(CancellationToken));
        Task<TUserProfile> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<TUserProfile> FindByIdAsync(string profileId, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAsync(TUserProfile userProfile);
    }
}