using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Options;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.CrossCuttingConcerns.Security;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Infrastructure;
using Maktoob.Domain.Models;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Specifications;
using Maktoob.Domain.Validators;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Maktoob.Domain.Services
{
    public class SignInService : CrudService<UserLogin> ,ISignInService
    {
        private readonly IJsonWebTokenCoder _jsonWebTokenCoder;
        private readonly IUserClaimsFactory _userClaimsFactory;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly JsonWebTokenOptions _jsonWebTokenOptions;

        public SignInService(IUserService userService, 
            IJsonWebTokenCoder jsonWebTokenCoder, 
            IUserLoginRepository userLoginRepository, 
            IUnitOfWork unitOfWork,
            GErrorDescriber errorDescriber,
            IUserClaimsFactory userClaimsFactory,
            IRefreshTokenGenerator refreshTokenGenerator,
            IEnumerable<IValidator<UserLogin>> validators,
            IOptions<JsonWebTokenOptions> jsonWebTokenOptions) : base(userLoginRepository, validators, errorDescriber, unitOfWork)
        {
            UserService = userService;
            _jsonWebTokenCoder = jsonWebTokenCoder;
            _userClaimsFactory = userClaimsFactory;
            _refreshTokenGenerator = refreshTokenGenerator;
            _jsonWebTokenOptions = jsonWebTokenOptions?.Value;
        }

        public IUserService UserService { get; }

        public async Task<GResult<TokenModel>> RefreshTokenAsync(string expiredToken, string refreshToken)
        {
            ClaimsPrincipal principal = null; 
            var errors = new List<GError>();
            // Validate Jwt Token
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jsonWebTokenOptions.Key)),
                    ValidIssuer = _jsonWebTokenOptions.Issuer,
                    ValidAudience = _jsonWebTokenOptions.Audience,
                    ValidateAudience = !string.IsNullOrWhiteSpace(_jsonWebTokenOptions.Audience),
                    ValidateIssuer = !string.IsNullOrWhiteSpace(_jsonWebTokenOptions.Issuer),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out var securityToken);
                
                if (!IsValidSecurityToken(securityToken))
                {
                    throw new SecurityTokenException("Invalid security token");
                }

                var expiryDateUnix = long.Parse(principal.FindFirst(JwtClaimNames.Exp).Value);
                var expiryDateTimeUtc = DateTime.UnixEpoch.AddSeconds(expiryDateUnix);

                if (expiryDateTimeUtc > DateTime.UtcNow)
                {
                    return GResult<TokenModel>.Success(new TokenModel { AccessToken = expiredToken, RefreshToken = refreshToken});
                }
            }
            catch (SecurityTokenException ex)
            {
                errors.Add(ErrorDescriber.InvalidToken(ex.Message));
            }

            if (errors.Count > 0)
            {
                return GResult<TokenModel>.Failed(errors.ToArray());
            }
            // Validate Refresh Token
            else
            {
                var userId = Guid.Parse(principal?.FindFirst(ClaimTypes.NameIdentifier).Value);
                var jwtId = principal?.FindFirstValue(JwtClaimNames.Jti);
                // Check if refresh token already registered for this user
                var readUserLoginResult = await base.ReadAsync(new FindUserLogin<UserLogin>(userId, jwtId, refreshToken));
                if (readUserLoginResult.Succeeded)
                {
                    var userLogin = readUserLoginResult.Outcome;
                    if(DateTime.UtcNow > userLogin.ExpiryDate)
                    {
                        await base.DeleteAsync(userLogin);
                        return GResult<TokenModel>.Failed(ErrorDescriber.InvalidToken(""));
                    }
                    else
                    {
                        var readUserResult = await UserService.ReadAsync(new FindByIdSpec<User>(userId));
                        if (readUserResult.Succeeded)
                        {
                            var user = readUserResult.Outcome;
                            var claims = await _userClaimsFactory.CreateAsync(user);
                            var newJwtId = claims.FirstOrDefault(c => c.Type == JwtClaimNames.Jti).Value;
                            var newToken = _jsonWebTokenCoder.Encode(claims);
                            userLogin.JwtId = newJwtId;
                            // Check if refresh token required update
                            if (DateTime.UtcNow > userLogin.RequiredUpdateDate)
                            {
                                userLogin.RefreshToken = _refreshTokenGenerator.Generate(user);
                                userLogin.ExpiryDate = DateTime.UtcNow + _jsonWebTokenOptions.RefreshToken.Expires;
                                userLogin.RequiredUpdateDate = DateTime.UtcNow + _jsonWebTokenOptions.RefreshToken.UpdateRequired;   
                            }

                            var updateUserLoginResult = await base.UpdateAsync(userLogin);

                            if (!updateUserLoginResult.Succeeded)
                            {
                                return GResult<TokenModel>.Failed(updateUserLoginResult.Errors.ToArray());
                            }
                            return GResult<TokenModel>.Success(new TokenModel { AccessToken = newToken, RefreshToken = userLogin.RefreshToken });
                        }
                        else
                        {
                            return GResult<TokenModel>.Failed(ErrorDescriber.InvalidCredentials());
                        }
                    }
                }
                else
                {
                    return GResult<TokenModel>.Failed(ErrorDescriber.InvalidCredentials());
                }
            }
        }

        private bool IsValidSecurityToken(SecurityToken securityToken)
            => (securityToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(_jsonWebTokenOptions.Algorithm, StringComparison.InvariantCultureIgnoreCase);

        public async Task<GResult<TokenModel>> SignInAsync(string credentials, string password, UserLogin userLogin,bool emailCredentials = false)
        {
            var readUserResult = await UserService.ReadAsync(
                emailCredentials?
                (SingleResultSpec<User>)
                new FindByEmailSpec<User>(credentials, UserService.KeyNormalizer) :
                new FindByNameSpec<User>(credentials, UserService.KeyNormalizer)
                );

            if (readUserResult.Succeeded)
            {
                var user = readUserResult.Outcome;
                var verificationResult = UserService.PasswordHasher.Verify(password, user.PasswordHash);
                if(verificationResult == PasswordVerificationResult.SUCCESS)
                {
                    var claims = await _userClaimsFactory.CreateAsync(user);
                    
                    var token = _jsonWebTokenCoder.Encode(claims);

                    userLogin.JwtId = claims.FirstOrDefault(c => c.Type == JwtClaimNames.Jti).Value;

                    userLogin.RefreshToken = _refreshTokenGenerator.Generate(user);
                    userLogin.ExpiryDate = DateTime.UtcNow + _jsonWebTokenOptions.RefreshToken.Expires;
                    userLogin.RequiredUpdateDate = DateTime.UtcNow + _jsonWebTokenOptions.RefreshToken.UpdateRequired;
                    userLogin.UserId = user.Id;
                    var createUserLoginResult  = await CreateAsync(userLogin);

                    return createUserLoginResult.Succeeded ? 
                        GResult<TokenModel>.Success(new TokenModel { AccessToken = token, RefreshToken = userLogin.RefreshToken }) : 
                        GResult<TokenModel>.Failed(createUserLoginResult.Errors.ToArray());
                }
            }
            return GResult<TokenModel>.Failed(ErrorDescriber.InvalidCredentials());
        }


        public async Task<GResult> SignOutAsync(Guid userId, string jwtId, string refreshToken, bool logOutAll = false)
        {
            var readUserLoginResult = await base.ReadAsync(new FindUserLogin<UserLogin>(userId, jwtId, refreshToken));
            if (readUserLoginResult.Succeeded)
            {
                if (logOutAll)
                {
                    var readUserLoginsResult = await base.ReadAsync(new FindUserLogins<UserLogin>(userId));
                    if (readUserLoginResult.Succeeded)
                    {
                        var deleteUserLoginsResult = await DeleteAsync(readUserLoginsResult.Outcome);
                        if (deleteUserLoginsResult.Succeeded)
                        {
                            return GResult.Success;
                        }
                        return GResult.Failed(ErrorDescriber.SignOutFailed());
                    }
                }
                else
                {
                    var deleteUserLoginResult = await DeleteAsync(readUserLoginResult.Outcome);
                    if (deleteUserLoginResult.Succeeded)
                    {
                        return GResult.Success;
                    }
                    return GResult.Failed(ErrorDescriber.SignOutFailed());
                }
            }
            return GResult.Failed(ErrorDescriber.NotFound());
        }
    }
}
