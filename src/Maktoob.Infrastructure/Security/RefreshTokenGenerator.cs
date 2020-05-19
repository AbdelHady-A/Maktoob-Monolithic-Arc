using Maktoob.Domain.Entities;
using Maktoob.Domain.Infrastructure;
using System;
using System.Security.Cryptography;

namespace Maktoob.Infrastructure.Security
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate(User user)
        {
            string refreshToken;
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] rToken = new byte[64];
                rng.GetBytes(rToken);
                refreshToken = Convert.ToBase64String(rToken);
            }
            return refreshToken;
        }
    }
}
