using Maktoob.CrossCuttingConcerns.Options;
using Maktoob.Domain.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Maktoob.Infrastructure.Security
{
    public class JsonWebTokenCoder : IJsonWebTokenCoder
    {
        private readonly JsonWebTokenOptions _options;
        private readonly SigningCredentials _signingCredentials;
        public JsonWebTokenCoder(IOptions<JsonWebTokenOptions> jsonWebTokenSettings)
        {
            _options = jsonWebTokenSettings?.Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jsonWebTokenSettings.Value.Key));

            _signingCredentials = new SigningCredentials(securityKey, _options.Algorithm);
        }

        public Dictionary<string, object> Decode(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token).Payload;
        }

        public string Encode(IEnumerable<Claim> claims)
        {
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            var jwtSecuritToken = new JwtSecurityToken(_options.Issuer,
                _options.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.Add(_options.Expires),
                _signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecuritToken);
        }
    }
}
