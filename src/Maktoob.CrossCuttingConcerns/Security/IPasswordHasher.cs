using Maktoob.CrossCuttingConcerns.Result;
using System.Threading.Tasks;

namespace Maktoob.CrossCuttingConcerns.Security
{
    /// <summary>
    /// Provides an abstraction for hashing passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password"/>.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password"/>.</returns>
        string Hash(string password);
        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hash">The hash value for a user's stored password.</param>
        /// <param name="password">The password supplied for comparison.</param>
        /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        PasswordVerificationResult Verify(string password, string hash);
    }
}