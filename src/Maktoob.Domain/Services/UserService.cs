using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Exceptions;
using Maktoob.CrossCuttingConcerns.Normalizers;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.CrossCuttingConcerns.Security;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Validators;

namespace Maktoob.Domain.Services
{
    public class UserService : CrudService<User> ,IUserService
    {

        public UserService(
            IUserRepository userRepository,
            GErrorDescriber errorDescriber,
            IKeyNormalizer keyNormalizer,
            IPasswordHasher passwordHasher,
            IEnumerable<IValidator<User>> validators
            ): base(userRepository, validators, errorDescriber)
        {
            KeyNormalizer = keyNormalizer;
            PasswordHasher = passwordHasher;
        }

        public IPasswordHasher PasswordHasher { get; }

        public IKeyNormalizer KeyNormalizer { get; }


        public override async Task<GResult> CreateAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new InvalidPasswordHashException($"password hash of {typeof(User)} null or empty");
            }
            user.NormalizedName = KeyNormalizer.Normalize(user.Name);
            user.NormalizedEmail = KeyNormalizer.Normalize(user.Email);

            return await base.CreateAsync(user);
        }
    }
}
