using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Maktoob.CrossCuttingConcerns.Options;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.CrossCuttingConcerns.Security;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace Maktoob.Infrastructure.Security
{
    /// <summary>
    /// Implements the standard password hashing.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasherOptions _options;
        private readonly IPasswordHasher _hasher;

        public PasswordHasher(IOptions<PasswordHasherOptions> options = null)
        {
            if (options == null)
            {
                options = Options.Create(new PasswordHasherOptions());
            }
            _options = options.Value;
            switch (_options.Version)
            {
                case PasswordHasherVersion.V1:
                    _hasher = new PassowrdHasherV1(this);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password"/>.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password"/>.</returns>
        public string Hash(string password)
        {
            return _hasher.Hash(password);
        }

        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="hash">The hash value for a user's stored password.</param>
        /// <param name="password">The password supplied for comparison.</param>
        /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        public PasswordVerificationResult Verify(string password, string hash)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            byte[] decodedHash = Convert.FromBase64String(hash);

            if (decodedHash.Length == 0)
            {
                return PasswordVerificationResult.FAILED;
            }

            var passwordHasherVersion = (PasswordHasherVersion)decodedHash[0];
            if(passwordHasherVersion == _options.Version)
            {
                return _hasher.Verify(password, hash);
            }
            // here must be decided other versions but we have only one now
            else
            {
                return PasswordVerificationResult.FAILED;
            }
        }




        /// <summary>
        /// Implements the version 1 of password hashing.
        /// </summary> 
        private class PassowrdHasherV1 : IPasswordHasher
        {
            private readonly PasswordHasher _outer;

            public PassowrdHasherV1(PasswordHasher outer)
            {
                _outer = outer;
            }

            
            public string Hash(string password)
            {
                return Convert.ToBase64String(HashPassword(password, _outer._options.Rng));
            }
            
            private byte[] HashPassword(string password, RandomNumberGenerator rng)
            {
                return HashPassword(password, rng, 
                    prf: _outer._options.PseudoRandomFunction, 
                    iterationCount: _outer._options.IterationCount, 
                    saltSize: _outer._options.SaltSize, 
                    numBytesRequested: _outer._options.HashSize);
            }

            private byte[] HashPassword(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterationCount, int saltSize, int numBytesRequested)
            {
                byte[] salt = new byte[saltSize];
                rng.GetBytes(salt);
                byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, numBytesRequested);

                // first 13 byte reseved to store hasing info 
                var outputBytes = new byte[13 + salt.Length + subkey.Length];
                outputBytes[0] = (byte)PasswordHasherVersion.V1; // format marker
                
                WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
                WriteNetworkByteOrder(outputBytes, 5, (uint)iterationCount);
                WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
                Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
                Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
                return outputBytes;
            }

            private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
            {
                buffer[offset + 0] = (byte)(value >> 24);
                buffer[offset + 1] = (byte)(value >> 16);
                buffer[offset + 2] = (byte)(value >> 8);
                buffer[offset + 3] = (byte)(value >> 0);
            }

            public PasswordVerificationResult Verify(string password, string hash)
            {
                byte[] decodedHash = Convert.FromBase64String(hash);

                return VerifyHashedPassword(decodedHash, password) ?
                    PasswordVerificationResult.SUCCESS :
                    PasswordVerificationResult.FAILED;
            }

            private bool VerifyHashedPassword(byte[] hash, string password)
            {
                try
                {
                    // read header info
                    KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetwrokByteOrder(hash, 1);
                    int iterationCount = (int)ReadNetwrokByteOrder(hash, 5);
                    int saltLength = (int)ReadNetwrokByteOrder(hash, 9);

                    // read the salt: must be >= 128 bits
                    if (saltLength < 128 / 8)
                    {
                        return false;
                    }

                    byte[] salt = new byte[saltLength];

                    Buffer.BlockCopy(hash, 13, salt, 0, salt.Length);

                    // read the subkey (the rest of payload): must be >= 128 bits
                    int subKeyLength = hash.Length - 13 - salt.Length;
                    if (subKeyLength < 128 / 8)
                    {
                        return false;
                    }

                    byte[] expectedSubKey = new byte[subKeyLength];
                    Buffer.BlockCopy(hash, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

                    // Hash the incoming password and verify it
                    byte[] actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, subKeyLength);

#if NETSTANDARD2_0
                return ByteArraysEqual(actualSubkey, expectedSubkey);
#elif NETCOREAPP3_1
                    return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
#else
#error Update target frameworks
#endif
                }
                catch
                {
                    // This should never occur except in the case of a malformed payload, where
                    // we might go off the end of the array. Regardless, a malformed payload
                    // implies verification failed.
                    return false;
                }
            }

            private static uint ReadNetwrokByteOrder(byte[] buffer, int offset)
                => ((uint)buffer[offset + 0] << 24)
                    | ((uint)buffer[offset + 1] << 16)
                    | ((uint)buffer[offset + 2] << 8)
                    | buffer[offset + 3];

        }
    }
}