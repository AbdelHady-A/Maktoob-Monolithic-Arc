using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Exceptions;
using Maktoob.CrossCuttingConcerns.Normalizers;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Services;
using Maktoob.Domain.Specifications;
using Maktoob.Domain.Validators;
using Maktoob.Infrastructure.Security;
using Maktoob.Persistance.Contexts;
using Maktoob.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Maktoob.FunctionalTests
{
    public class UserServiceTest
    {
        private readonly IUserService _userService;
        private readonly GErrorDescriber _errorDescriber;
        public UserServiceTest()
        {
            var context = new MaktoobDbContext(new DbContextOptionsBuilder<MaktoobDbContext>().UseInMemoryDatabase("MaktoobDb").Options);
            _errorDescriber = new GErrorDescriber();
            var keyNormalizer = new NameNormalizer();
            var passwordHasher = new PasswordHasher();
            var unitOfWork = new UnitOfWork(context);
            var userRepository = new UserRepository(context, unitOfWork);
            var userValidator = new IValidator<User>[] { new UserValidator(userRepository, keyNormalizer, _errorDescriber) };
            _userService = new UserService(userRepository, _errorDescriber, keyNormalizer, passwordHasher, userValidator);
        }

        [Fact]
        public async void CreateValidUserAllowed()
        {

            var user = new User { Name = "bob", Email = "bob@mail.com" };

            user.PasswordHash = _userService.PasswordHasher.Hash("my_password");

            var result = await _userService.CreateAsync(user);

            Assert.Equal(GResult.Success, result);
        }

        [Fact]
        public async void CreateDublicateNameNotAllowed()
        {
            // Arrange
            var user1 = new User { Name = "user1", Email = "user1@mail.com" };
            var user2 = new User { Name = "user1", Email = "user2@mail.com" };
            user1.PasswordHash = _userService.PasswordHasher.Hash("user1_password");
            user2.PasswordHash = _userService.PasswordHasher.Hash("user2_password");

            // Act
            var result1 = await _userService.CreateAsync(user1);
            var result2 = await _userService.CreateAsync(user2);

            // Assert
            Assert.Equal(GResult.Failed(_errorDescriber.DuplicateUserName(user2.Name)), result2);
        }

        [Fact]
        public async void CreateDublicateEmailNotAllowed()
        {
            // Arrange
            var user1 = new User { Name = "user1", Email = "user1@mail.com" };
            var user2 = new User { Name = "user2", Email = "user1@mail.com" };
            user1.PasswordHash = _userService.PasswordHasher.Hash("user1_password");
            user2.PasswordHash = _userService.PasswordHasher.Hash("user2_password");

            // Act
            var result1 = await _userService.CreateAsync(user1);
            var result2 = await _userService.CreateAsync(user2);

            // Assert
            Assert.Equal(GResult.Failed(_errorDescriber.DuplicateEmail(user2.Email)), result2);
        }
        [Fact]
        public async void CreateDublicateNameAndEmailNotAllowed()
        {
            // Arrange
            var user1 = new User { Name = "user1", Email = "user1@mail.com" };
            var user2 = new User { Name = "user1", Email = "user1@mail.com" };
            user1.PasswordHash = _userService.PasswordHasher.Hash("user1_password");
            user2.PasswordHash = _userService.PasswordHasher.Hash("user2_password");

            // Act
            var result1 = await _userService.CreateAsync(user1);
            var result2 = await _userService.CreateAsync(user2);

            // Assert
            Assert.Equal(GResult.Failed(_errorDescriber.DuplicateUserName(user2.Name), _errorDescriber.DuplicateEmail(user2.Email)), result2);
        }

        [Fact]
        public async void CreateWithInvalidPasswordHashNotAllowed()
        {
            // Arrange
            var user = new User { Name = "user1", Email = "user1@mail.com" };

            var act = new Func<Task>(async () => await _userService.CreateAsync(user));

            // Assert
            await Assert.ThrowsAsync<InvalidPasswordHashException>(act);
        }

        [Fact]
        public async void ReadExistingUserSuccess()
        {
            // Arrange
            var user1 = new User { Name = "user1", Email = "user1@mail.com" };
            var user2 = new User { Name = "user1", Email = "user1@mail.com" };
            user1.PasswordHash = _userService.PasswordHasher.Hash("user1_password");
            user2.PasswordHash = _userService.PasswordHasher.Hash("user2_password");


            // Act
            await _userService.CreateAsync(user1);
            await _userService.CreateAsync(user2);

            var result = await _userService.ReadAsync(new FindByNameSpec<User>(user1.Name, _userService.KeyNormalizer));

            // Assert
            Assert.Equal(GResult.Success, result);
        }
    }

}
