using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Normalizers;
using Maktoob.CrossCuttingConcerns.Options;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Services;
using Maktoob.Domain.Validators;
using Maktoob.Infrastructure.Security;
using Maktoob.Persistance.Contexts;
using Maktoob.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Xunit;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Maktoob.FunctionalTests
{
    public class SignInServiceTest
    {
        private readonly ISignInService _signInService;
        private readonly GErrorDescriber _errorDescriber;
        private readonly IUserService _userService;
        public SignInServiceTest()
        {
            var context = new MaktoobDbContext(new DbContextOptionsBuilder<MaktoobDbContext>().UseInMemoryDatabase("MaktoobDb").Options);
            _errorDescriber = new GErrorDescriber();
            var keyNormalizer = new NameNormalizer();
            var passwordHasher = new PasswordHasher();
            var unitOfWork = new UnitOfWork(context);
            var userRepository = new UserRepository(context);
            var userLoginRepository = new UserLoginRepository(context);
            var userValidator = new IValidator<User>[] { new UserValidator(userRepository, keyNormalizer, _errorDescriber) };
            _userService = new UserService(userRepository, unitOfWork, _errorDescriber, keyNormalizer, passwordHasher, userValidator);
            var jsonWebTokenOptions = new JsonWebTokenOptions
            {
                Issuer = "issuer",
                Audience = "audience",
                Algorithm = "HS256",
                Key = "super secret key",
                Expires = TimeSpan.Parse("00:00:01"), // 1 seconds
                RefreshToken = new RefreshTokenOptions
                {
                    Expires = TimeSpan.Parse("00:00:10"), // 10 seconds
                    UpdateRequired = TimeSpan.Parse("00:00:05") // 5 seconds
                }
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jsonWebTokenOptions.Key));
            JwtBearerOptions jwtBearerOptions = new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = securityKey,
                    ValidIssuer = jsonWebTokenOptions.Issuer,
                    ValidAudience = jsonWebTokenOptions.Audience,
                    ValidateAudience = !string.IsNullOrWhiteSpace(jsonWebTokenOptions.Audience),
                    ValidateIssuer = !string.IsNullOrWhiteSpace(jsonWebTokenOptions.Issuer),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                }
            };
            var jsonWebTokenCoder = new JsonWebTokenCoder(Options.Create(jsonWebTokenOptions));
            var userClaimsFactory = new UserClaimsFactory();
            var refreshTokenGenerator = new RefreshTokenGenerator();
            _signInService = new SignInService(_userService,
                jsonWebTokenCoder, userLoginRepository, unitOfWork,
                _errorDescriber, userClaimsFactory, refreshTokenGenerator, null,
                Options.Create(jsonWebTokenOptions));
        }


        [Fact]
        public async void RegisteredUserCanSignIn()
        {
            // Arrange
            var user = new User { Name = "user", Email = "user@email.com" };
            user.PasswordHash = _userService.PasswordHasher.Hash("user password");
            await _userService.CreateAsync(user);

            // Act
            var result = await _signInService.SignInAsync(user.Name, "user password", new UserLogin { });

            // Assert
            Assert.Equal(GResult.Success, result);
            Assert.NotNull(result.Outcome);
        }

        [Theory]
        [InlineData("user3", "user3@email.com", "password3")]
        [InlineData("user4", "user4@email.com", "password4")]

        public async void RefreshTokenAcceptedCases(string username, string email, string password)
        {
            // Arrange
            var user = new User { Name = username, Email = email };
            user.PasswordHash = _userService.PasswordHasher.Hash(password);
            await _userService.CreateAsync(user);

            // Act
            var result1 = await _signInService.SignInAsync(username, password, new UserLogin { });

            await Task.Delay(TimeSpan.FromSeconds(1)); // wait until token expired

            var result2 = await _signInService.RefreshTokenAsync(result1.Outcome.AccessToken, result1.Outcome.RefreshToken);
            await Task.Delay(TimeSpan.FromSeconds(1)); // wait until token expired

            var result3 = await _signInService.RefreshTokenAsync(result2.Outcome.AccessToken, result2.Outcome.RefreshToken);
            await Task.Delay(TimeSpan.FromSeconds(1)); // wait until token expired

            var result4 = await _signInService.RefreshTokenAsync(result3.Outcome.AccessToken, result3.Outcome.RefreshToken);
            await Task.Delay(TimeSpan.FromSeconds(1)); // wait until token expired

            var result5 = await _signInService.RefreshTokenAsync(result4.Outcome.AccessToken, result4.Outcome.RefreshToken);
            await Task.Delay(TimeSpan.FromSeconds(1)); // wait until token expired

            // Assert
            Assert.Equal(GResult.Success, result1);
            Assert.Equal(GResult.Success, result5);
            Assert.NotNull(result5.Outcome);
            Assert.NotNull(result5.Outcome.AccessToken);
            Assert.NotEmpty(result5.Outcome.AccessToken);
            Assert.NotEqual(result1.Outcome.AccessToken, result5.Outcome.AccessToken);
            Assert.Equal(result1.Outcome.RefreshToken, result5.Outcome.RefreshToken); // Before refresh token requires an update, must be equal

            // here refresh token will be updated
            var result6 = await _signInService.RefreshTokenAsync(result5.Outcome.AccessToken, result5.Outcome.RefreshToken);


            // Assert
            Assert.Equal(GResult.Success, result1);
            Assert.Equal(GResult.Success, result6);
            Assert.NotNull(result6.Outcome);
            Assert.NotNull(result6.Outcome.AccessToken);
            Assert.NotEmpty(result6.Outcome.AccessToken);
            Assert.NotEqual(result1.Outcome.AccessToken, result6.Outcome.AccessToken);
            Assert.NotEqual(result1.Outcome.RefreshToken, result6.Outcome.RefreshToken);// after updating refresh token, must be not equal

        }


        [Theory]
        [InlineData("user5", "user5@email.com", "password5")]
        [InlineData("user6", "user6@email.com", "password6")]
        public async void RefreshTokenAcceptedCasesWithUpdateRefreshToken(string username, string email, string password)
        {
            // Arrange
            var user = new User { Name = username, Email = email };
            user.PasswordHash = _userService.PasswordHasher.Hash(password);
            await _userService.CreateAsync(user);

            // Act
            var result1 = await _signInService.SignInAsync(username, password, new UserLogin { });

            await Task.Delay(TimeSpan.FromSeconds(6)); // wait until refresh token required update

            var result2 = await _signInService.RefreshTokenAsync(result1.Outcome.AccessToken, result1.Outcome.RefreshToken);

            // Assert
            Assert.Equal(GResult.Success, result1);
            Assert.Equal(GResult.Success, result2);
            Assert.NotNull(result2.Outcome);
            Assert.NotNull(result2.Outcome.AccessToken);
            Assert.NotEmpty(result2.Outcome.AccessToken);
            Assert.NotEqual(result1.Outcome.AccessToken, result2.Outcome.AccessToken);
            Assert.NotEqual(result1.Outcome.RefreshToken, result2.Outcome.RefreshToken);
        }



        [Theory]
        [InlineData("user7", "user7@email.com", "password7")]
        [InlineData("user8", "user8@email.com", "password8")]

        public async void RefreshTokenAfterRefreshTokenExpiredNotAllowed(string username, string email, string password)
        {
            // Arrange
            var user = new User { Name = username, Email = email };
            user.PasswordHash = _userService.PasswordHasher.Hash(password);
            var r = await _userService.CreateAsync(user);

            // Act
            var result1 = await _signInService.SignInAsync(username, password, new UserLogin { });

            await Task.Delay(TimeSpan.FromSeconds(11)); // wait until refresh token expired

            var result2 = await _signInService.RefreshTokenAsync(result1.Outcome.AccessToken, result1.Outcome.RefreshToken);

            // Assert
            Assert.Equal(GResult.Success, result1);
            Assert.NotEqual(GResult.Success, result2);
            Assert.Null(result2.Outcome);
        }
    }
}
