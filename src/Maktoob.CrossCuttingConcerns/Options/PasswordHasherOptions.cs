using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Maktoob.CrossCuttingConcerns.Options
{
    /// <summary>
    /// Specifies the format used for hashing passwords.
    /// </summary>
    public enum PasswordHasherVersion
    {
        /// <summary>
        /// Indicates hashing passwords in a way that is compatible with version 1.
        /// </summary>
        V1 = 0x01
    }
    /// <summary>
    /// Specifies options for password hashing.
    /// </summary>
    public class PasswordHasherOptions
    {
        /// <summary>
        /// Gets or sets the <see cref="PasswordHasherVersion"> used when hashing passwords. Defaults to 'Version 1'.
        /// </summary>
        /// <value>
        /// The the password hasher version used when hashing passwords.
        /// </value>
        public PasswordHasherVersion Version { get; set; } = PasswordHasherVersion.V1;
        /// <summary>
        /// Gets or sets the number of iterations used when hashing passwords using PBKDF2. Default is 10,000.
        /// </summary>
        /// <value>
        /// The number of iterations used when hashing passwords using PBKDF2.
        /// </value>
        /// <remarks>
        /// The value must be a positive integer. 
        /// </remarks>
        public UInt16 IterationCount { get; set; } = 10000;
        /// <summary>
        /// Gets or sets the size of salt in byte used when hashing passwords. Default is 16.
        /// </summary>
        /// <value>
        /// The size of salt used when hashing passwords.
        /// </value>
        /// <remarks>
        /// The value must be a positive integer. 
        /// </remarks>
        public UInt16 SaltSize { get; set; } = 16;
        /// <summary>
        /// Gets or sets the size of hash in byte produced when hashing passwords. Default is 32.
        /// </summary>
        /// <value>
        /// The size of hash produced when hashing passwords.
        /// </value>
        /// <remarks>
        /// The value must be a positive integer. 
        /// </remarks>
        public UInt16 HashSize { get; set; } = 32;
        /// <summary>
        /// Gets or sets the pseudo random function which should be used for the key derivation algorithm. Default is <see cref="KeyDerivationPrf.HMACSHA256"/>.
        /// </summary>
        /// <value>
        /// The size of salt used when hashing passwords.
        /// </value>
        /// <remarks>
        /// The value must be a positive integer. 
        /// </remarks>
        public KeyDerivationPrf PseudoRandomFunction { get; set; } = KeyDerivationPrf.HMACSHA256;
        /// <summary>
        /// Gets or sets the random number generator which used when hashing passwords. Default is <see cref="RandomNumberGenerator.Create"/>.
        /// </summary>
        /// <value>
        /// The size of salt used when hashing passwords.
        /// </value>
        /// <remarks>
        /// The value must be a positive integer. 
        /// </remarks>
        public RandomNumberGenerator Rng { get; set; } = RandomNumberGenerator.Create();
    }
}
