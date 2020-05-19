using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.CrossCuttingConcerns.Result
{
    /// <summary>
    /// Specifies the results for password verfication.
    /// </summary>
    public enum PasswordVerificationResult
    {
        /// <summary>
        /// Indicates password verification failed.
        /// </summary>
        FAILED = 0,

        /// <summary>
        /// Indicates password verification was successful.
        /// </summary>
        SUCCESS = 1,

        /// <summary>
        /// Indicates password verification was successful however the password was encoded using a deprecated algorithm
        /// and should be rehashed and updated.
        /// </summary>
        SUCCESS_REHASH_NEEDED = 2
    }

}
