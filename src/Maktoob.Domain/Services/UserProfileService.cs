using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Domain.Validators;
using System.Collections.Generic;

namespace Maktoob.Domain.Services
{
    public class UserProfileService : CrudService<UserProfile>, IUserProfileService
    {
        public UserProfileService(
            IUserProfileRepository repository,
            IEnumerable<IValidator<UserProfile>> validators,
            GErrorDescriber errorDescriber
        ) : base(repository, validators, errorDescriber)
        {
        }
    }
}
