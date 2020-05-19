using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Options;
using Maktoob.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Validators
{
    public class TokenModelValidator : IValidator<TokenModel>
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly JwtBearerOptions _options;
        private readonly GErrorDescriber _errorDescriber;

        public TokenModelValidator(IOptions<JwtBearerOptions> options, GErrorDescriber errorDescriber)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _options = options?.Value;
            _errorDescriber = errorDescriber;
        }

        public async Task<IEnumerable<GError>> ValidateAsync(TokenModel tokenModel)
        {
            var errors = new List<GError>();
            try
            {
                var principal = _tokenHandler.ValidateToken(tokenModel.AccessToken, _options.TokenValidationParameters, out var securityToken);
                
                if (!IsValidSecurityToken(securityToken))
                {
                    throw new SecurityTokenException("Invalid security token");
                }
            }
            catch (SecurityTokenException ex)
            {
                errors.Add(new GError { Code = "InvalidSecurityToken", Description = ex.Message });
            }

            return await Task.FromResult(errors);
        }

        private bool IsValidSecurityToken(SecurityToken securityToken)
            => (securityToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }
}
