using Maktoob.CrossCuttingConcerns.Properties;

namespace Maktoob.CrossCuttingConcerns.Error
{
    /// <summary>
    /// Service to enable localization for application facing Maktoob errors.
    /// </summary>
    /// <remarks>
    /// These errors are returned to controllers and are generally used as display messages to end users.
    /// </remarks>
    public class GErrorDescriber
    {
        /// <summary>
        /// Returns an <see cref="GError"/> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="GError"/> indicating an invalid token.</returns>
        public GError InvalidToken(string cause)
        {
            return new GError
            {
                Code = nameof(InvalidToken),
                Description = Errors.InvalidTokenFormat(cause)
            };
        }

        /// <summary>
        /// Returns an <see cref="GError"/> indicating user sign out can not be completed.
        /// </summary>
        /// <returns>An <see cref="GError"/> indicating user sign out can not be completed.</returns>
        public GError SignOutFailed()
        {
            return new GError
            {
                Code = nameof(SignOutFailed),
                Description = Errors.SignOutFailed
            };
        }
        /// <summary>
        /// Returns an <see cref="GError"/> indicating an invalid credentials.
        /// </summary>
        /// <returns>An <see cref="GError"/> indicating an invalid credentials.</returns>
        public GError InvalidCredentials()
        {
            return new GError
            {
                Code = nameof(InvalidCredentials),
                Description = Errors.InvalidCredentials
            };
        }

        /// <summary>
        /// Returns an <see cref="GError"/> indicating a resource not found.
        /// </summary>
        /// <returns>An <see cref="GError"/> indicating a resource not found.</returns>
        public GError NotFound()
        {
            return new GError
            {
                Code = nameof(NotFound),
                Description = Errors.NotFound
            };
        }
        /// <summary>
        /// Returns the default <see cref="GError"/>.
        /// </summary>
        /// <returns>The default <see cref="GError"/>.</returns>
        public GError DefaultError()
        {
            return new GError
            {
                Code = nameof(DefaultError),
                Description = Errors.DefaultError
            };
        }

        /// <summary>
        /// Returns an <see cref="GError"/> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="GError"/> indicating a concurrency failure.</returns>
        public GError ConcurrencyFailure()
        {
            return new GError
            {
                Code = nameof(ConcurrencyFailure),
                Description = Errors.ConcurrencyFailure
            };
        }



        /// <summary>
        /// Returns an <see cref="GError"/> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="GError"/> indicating an external login is already associated with an account.</returns>
        public GError LoginAlreadyAssociated()
        {
            return new GError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = Errors.LoginAlreadyAssociated
            };
        }

        /// <summary>
        /// Returns an <see cref="GError"/> indicating the specified user <paramref name="userName"/> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="GError"/> indicating the specified user <paramref name="userName"/> is invalid.</returns>
        public GError InvalidUserName(string userName)
        {
            return new GError
            {
                Code = nameof(InvalidUserName),
                Description = Errors.InvalidUserNameFormat(userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="GError"/> indicating the specified <paramref name="email"/> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="GError"/> indicating the specified <paramref name="email"/> is invalid.</returns>
        public GError InvalidEmail(string email)
        {
            return new GError
            {
                Code = nameof(InvalidEmail),
                Description = Errors.InvalidEmailFormat(email)
            };
        }

        /// <summary>
        /// Returns an <see cref="GError"/> indicating the specified <paramref name="userName"/> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="GError"/> indicating the specified <paramref name="userName"/> already exists.</returns>
        public GError DuplicateUserName(string userName)
        {
            return new GError
            {
                Code = nameof(DuplicateUserName),
                Description = Errors.DuplicateUserNameFormat(userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="GError"/> indicating the specified <paramref name="email"/> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="GError"/> indicating the specified <paramref name="email"/> is already associated with an account.</returns>
        public GError DuplicateEmail(string email)
        {
            return new GError
            {
                Code = nameof(DuplicateEmail),
                Description = Errors.DuplicateEmailFormat(email)
            };
        }
    }
}