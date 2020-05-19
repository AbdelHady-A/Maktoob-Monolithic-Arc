using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Normalizers;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Services;
using Maktoob.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Validators
{
    public class UserValidator : IValidator<User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IKeyNormalizer _keyNormalizer;
        private readonly GErrorDescriber _errorDescriber;

        public UserValidator(IUserRepository userRepository, IKeyNormalizer keyNormalizer, GErrorDescriber errorDescriber)
        {
            _userRepository = userRepository;
            _keyNormalizer = keyNormalizer;
            _errorDescriber = errorDescriber;
        }

        public async Task<IEnumerable<GError>> ValidateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var errors = new List<GError>();
            await ValidateNameAsync(user, errors);
            await ValidateEmailAsync(user, errors);

            return errors;
        }

        private async Task ValidateEmailAsync(User user, List<GError> errors)
        {
            var userWithSameEmail = await _userRepository.GetAsync(new FindByEmailSpec<User>(user.Email, _keyNormalizer));

            if(userWithSameEmail != null)
            {
                errors.Add(_errorDescriber.DuplicateEmail(user.Email));
            }
        }

        private async Task ValidateNameAsync(User user, List<GError> errors)
        {
            var userWithSameName = await _userRepository.GetAsync(new FindByNameSpec<User>(user.Name, _keyNormalizer));

            if (userWithSameName != null)
            {
                errors.Add(_errorDescriber.DuplicateUserName(user.Name));
            }
        }
    }
}
